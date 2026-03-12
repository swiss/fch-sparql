using Swiss.FCh.Sparql.Client.Configuration;
using Swiss.FCh.Sparql.Models;
using Swiss.FCh.Sparql.Queries;
using VDS.RDF.Nodes;
using VDS.RDF.Query;

namespace Swiss.FCh.Sparql.Client;

internal class AgentClient : IAgentClient
{
    private readonly HttpClient _client;

    public AgentClient(IHttpClientFactory factory)
    {
        _client = factory.CreateClient(SwissFChSparqlConstants.SparqlHttpClientName);
    }

    public async Task<IEnumerable<MasterData>> GetAgents(
        IEnumerable<string> uris,
        CancellationToken ct)
    {

        var query = AgentQuery.GetAgentsQuery(uris);

        ct.ThrowIfCancellationRequested();

        var queryClient = new SparqlQueryClient(_client, new Uri(SparqlEndpoints.LdAdmin));
        var results = await queryClient.QueryWithResultSetAsync(query, ct).ConfigureAwait(false);

        ct.ThrowIfCancellationRequested();

        var agents = results.Select(agent => new MasterData
        {
            DefinedTermSet = string.Empty,
            Identifier = agent[0].AsValuedNode().AsString(),
            DefinedTerm = agent[0].AsValuedNode().AsString(),
            NameDe = agent.FirstOrDefault(item => item.Key == "nameDe").Value.AsValuedNode()?.AsString() ?? null,
            NameFr = agent.FirstOrDefault(item => item.Key == "nameFr").Value.AsValuedNode()?.AsString() ?? null,
            NameIt = agent.FirstOrDefault(item => item.Key == "nameIt").Value.AsValuedNode()?.AsString() ?? null
        });

        return agents;
    }
}
