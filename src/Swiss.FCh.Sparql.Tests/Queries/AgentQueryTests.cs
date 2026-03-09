using Swiss.FCh.Sparql.Queries;

namespace Swiss.FCh.Sparql.Tests.Queries;

[TestFixture]
internal sealed class AgentQueryTests
{
    [Test]
    public void GetAgentQuery_ShouldDeliverData()
    {
        string[] uris = { "https://politics.ld.admin.ch/council/S", "https://politics.ld.admin.ch/council/N" };
        var query = AgentQuery.GetAgentsQuery(uris);

        Assert.That(query, Is.Not.Null);
        Assert.That(query, Is.EqualTo("PREFIX schema: <http://schema.org/> SELECT DISTINCT * WHERE { VALUES ?agent { <https://politics.ld.admin.ch/council/S> <https://politics.ld.admin.ch/council/N> } { ?agent schema:name ?nameDe. ?agent schema:name ?nameFr. ?agent schema:name ?nameIt. FILTER ( lang(?nameDe) = '' ) FILTER ( lang(?nameFr) = '' ) FILTER ( lang(?nameIt) = '' ) } UNION { ?agent schema:name ?nameDe. FILTER ( lang(?nameDe) = 'de' ) OPTIONAL { ?agent schema:name ?nameFr FILTER ( lang(?nameFr) = 'fr' ) } OPTIONAL { ?agent schema:name ?nameIt FILTER ( lang(?nameIt) = 'it' ) } } }"));
    }
}
