using System.Text;

namespace Swiss.FCh.Sparql.Queries;

internal static class AgentQuery
{
    internal static string GetAgentsQuery(IEnumerable<string> uris)
    {
        var sb = new StringBuilder();

        sb.Append("PREFIX schema: <http://schema.org/> ");
        sb.Append("SELECT DISTINCT * ");
        sb.Append("WHERE { ");
        sb.Append("VALUES ?agent { ");
        sb.Append(string.Join(string.Empty, uris.Select(item => $"<{item}> ")));
        sb.Append("} { ");
        sb.Append("?agent schema:name ?nameDe. ");
        sb.Append("?agent schema:name ?nameFr. ");
        sb.Append("?agent schema:name ?nameIt. ");
        sb.Append("FILTER ( lang(?nameDe) = '' ) FILTER ( lang(?nameFr) = '' ) FILTER ( lang(?nameIt) = '' ) ");
        sb.Append("} UNION { ");
        sb.Append("?agent schema:name ?nameDe. ");
        sb.Append("FILTER ( lang(?nameDe) = 'de' ) ");
        sb.Append("OPTIONAL { ?agent schema:name ?nameFr FILTER ( lang(?nameFr) = 'fr' ) } ");
        sb.Append("OPTIONAL { ?agent schema:name ?nameIt FILTER ( lang(?nameIt) = 'it' ) } ");
        sb.Append("} }");

        return sb.ToString();
    }
}
