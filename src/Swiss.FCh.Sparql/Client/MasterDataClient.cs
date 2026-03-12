using Swiss.FCh.Sparql.Client.Configuration;
using Swiss.FCh.Sparql.Extensions;
using Swiss.FCh.Sparql.Models;
using Swiss.FCh.Sparql.Queries;
using VDS.RDF;
using VDS.RDF.Nodes;
using VDS.RDF.Query;

namespace Swiss.FCh.Sparql.Client;

internal class MasterDataClient : IMasterDataClient
{
    private readonly HttpClient _client;

    public MasterDataClient(IHttpClientFactory factory)
    {
        _client = factory.CreateClient(SwissFChSparqlConstants.SparqlHttpClientName);
    }

    public async Task<IEnumerable<MasterData>> GetMasterData(
        string definedTermSet,
        CancellationToken ct,
        string endpointUrl = SparqlEndpoints.LdAdmin,
        string dataModel = DataModels.SchemaOrg,
        IEnumerable<AdditionalAttribute>? additionalAttributes = null)
    {
        var query = MasterDataQuery.GetMasterDataQuery(definedTermSet, additionalAttributes?.ToList());

        ct.ThrowIfCancellationRequested();

        var queryClient = new SparqlQueryClient(_client, new Uri(endpointUrl));
        var queryResult = await queryClient.QueryWithResultSetAsync(query, ct).ConfigureAwait(false);

        var aggregatedResult = new List<MasterData>();

        foreach (var result in queryResult)
        {
            ct.ThrowIfCancellationRequested();

            var termSet = result[MasterDataQuery.ColumnNameDefinedTermSet].AsValuedNode().AsString();
            var term = result[MasterDataQuery.ColumnNameDefinedTerm].AsValuedNode().AsString();
            var position = result[MasterDataQuery.ColumnNamePosition]?.AsValuedNode().AsString();
            var name = result[MasterDataQuery.ColumnNameName].AsValuedNode() as LiteralNode;
            var identifier = result[MasterDataQuery.ColumnNameIdentifier]?.AsValuedNode().AsString();
            var startDate = result[MasterDataQuery.ColumnNameStartDate]?.AsValuedNode().AsDateTime();
            var validFrom = result[MasterDataQuery.ColumnNameValidFrom]?.AsValuedNode().AsString(); //this should be a date, but on LINDAS, it isn't
            var endDate = result[MasterDataQuery.ColumnNameEndDate]?.AsValuedNode().AsDateTime();
            var validTo = result[MasterDataQuery.ColumnNameValidTo]?.AsValuedNode().AsString(); //this should be a date, but on LINDAS, it isn't
            var altName = result[MasterDataQuery.ColumnNameAltName]?.AsValuedNode() as LiteralNode;
            var tempAdditionalAttributes = additionalAttributes?.ToDictionary(
                item => item.Identifier,
                item => result[item.Identifier]?.AsValuedNode().AsString());

            var existing = aggregatedResult.FirstOrDefault(n => n.DefinedTerm == term);

            if (existing is not null)
            {
                //this record is already in the result -> only add the missing translations
                existing.AddName(name);
                existing.AddAltName(altName);
            }
            else
            {
                DateOnly? startDateValue = startDate.HasValue ? DateOnly.FromDateTime(startDate.Value) : null;
                DateOnly? endDateValue = endDate.HasValue ? DateOnly.FromDateTime(endDate.Value) : null;
                DateOnly? validFromValue = validFrom != null ? DateOnly.Parse(validFrom) : null;
                DateOnly? validToValue = validTo != null ? DateOnly.Parse(validTo) : null;

                var newRecord = new MasterData
                {
                    DefinedTermSet = termSet,
                    DefinedTerm = term,
                    Identifier = identifier,
                    Position = position,
                    Start = startDateValue ?? validFromValue,
                    End = endDateValue ?? validToValue,
                    AdditionalAttributes = tempAdditionalAttributes
                };

                newRecord.AddName(name);
                newRecord.AddAltName(altName);

                aggregatedResult.Add(newRecord);
            }
        }

        return aggregatedResult;
    }
}
