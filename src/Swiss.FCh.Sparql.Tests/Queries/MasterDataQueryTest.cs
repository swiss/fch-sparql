using Swiss.FCh.Sparql.Queries;

namespace Swiss.FCh.Sparql.Tests.Queries;

[TestFixture]
internal sealed class MasterDataQueryTest
{
    [Test]
    public void GetMasterDataQuery_WithDefinedTermSet_ReturnsQueryCorrectly()
    {
        var query = MasterDataQuery.GetMasterDataQuery("testTermSet");

        Assert.That(query, Is.Not.Empty);

        Assert.That(query, Does.StartWith("PREFIX"));
        Assert.That(query, Does.Contain("BIND (<testTermSet> as ?definedTermSet)"));
        Assert.That(query, Has.Length.GreaterThan(500));
    }
}
