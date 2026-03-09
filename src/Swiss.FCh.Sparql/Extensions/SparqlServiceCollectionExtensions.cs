using Swiss.FCh.Sparql.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Swiss.FCh.Sparql.Extensions;

public static class SparqlServiceCollectionExtensions
{
    public static void AddSparqlClient(this IServiceCollection services)
    {
        services.AddTransient<IMasterDataClient, MasterDataClient>();
        services.AddTransient<IAgentClient, AgentClient>();
    }
}
