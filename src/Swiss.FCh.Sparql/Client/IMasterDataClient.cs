using Swiss.FCh.Sparql.Client.Configuration;
using Swiss.FCh.Sparql.Models;

namespace Swiss.FCh.Sparql.Client;

public interface IMasterDataClient
{
    /// <summary>
    /// Generic query for all sorts of master data.
    /// </summary>
    /// <param name="definedTermSet">For possible values: <see cref="Swiss.FCh.Sparql.Client.Configuration.DefinedTermsets"/></param>
    /// <param name="ct">A cancellation token</param>
    /// <param name="endpointUrl">For possible values: <see cref="Swiss.FCh.Sparql.Client.Configuration.SparqlEndpoints"/></param>
    /// <param name="dataModel">For possible values: <see cref="Swiss.FCh.Sparql.Client.Configuration.DataModels"/></param>
    /// <param name="additionalAttributes">Optional additional properties to include in query</param>
    /// <returns>The results of the query as <see cref="Swiss.FCh.Sparql.Models.MasterData"/></returns>
    Task<IEnumerable<MasterData>> GetMasterData(
        string definedTermSet,
        CancellationToken ct,
        string endpointUrl = SparqlEndpoints.LdAdmin,
        string dataModel = DataModels.SchemaOrg,
        IEnumerable<AdditionalAttribute>? additionalAttributes = null);
}
