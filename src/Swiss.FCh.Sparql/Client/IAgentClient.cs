using Swiss.FCh.Sparql.Models;

namespace Swiss.FCh.Sparql.Client;

public interface IAgentClient
{
    Task<IEnumerable<MasterData>> GetAgents(
        IEnumerable<string> uris,
        CancellationToken ct);
}
