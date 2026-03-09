using Swiss.FCh.Sparql.Client;
using Swiss.FCh.Sparql.Models;
using NSubstitute;
using NSubstitute.ClearExtensions;

namespace Swiss.FCh.Sparql.Tests.Client;

[TestFixture]
internal sealed class AgentClientTests
{
    private AgentClient _agentClient = null!;
    private HttpClient _httpClient = null!;
    private readonly IHttpClientFactory _factory = Substitute.For<IHttpClientFactory>();

    private HttpClientHandler _handler;

    [SetUp]
    public void Setup()
    {

        _handler = new HttpClientHandler
        {
            //Add proxy here, if needed
        };

        _httpClient = new HttpClient(_handler);

        _factory.CreateClient(SwissFChSparqlConstants.SparqlHttpClientName).Returns(_httpClient);

        _agentClient = new AgentClient(_factory);
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
        _factory.ClearSubstitute();
        _handler.Dispose();
    }

    [TestCase("https://politics.ld.admin.ch/council/S", "https://politics.ld.admin.ch/council/N")]
    [TestCase("https://politics.ld.admin.ch/council/S", "https://ld.admin.ch/PS", "https://ld.admin.ch/FCh", "https://ld.admin.ch/FC")]
    public async Task GetAgents_ShouldDeliverData(params string[] uris)
    {
        var agents = (await _agentClient.GetAgents(uris, CancellationToken.None)).ToList();

        Assert.That(agents, Is.Not.Null);
        Assert.That(agents, Is.InstanceOf<IEnumerable<MasterData>>());
        Assert.That(agents, Has.Count.EqualTo(uris.Length));

        foreach (var agent in agents)
        {
            Assert.Multiple(() =>
            {
                Assert.That(agent.DefinedTerm, Is.Not.Empty);
                Assert.That(agent.Identifier, Is.Not.Empty);
                Assert.That(agent.NameDe, Is.Not.Empty);
                Assert.That(agent.NameFr, Is.Not.Empty);
                Assert.That(agent.NameIt, Is.Not.Empty);
            });
        }
    }

    [TestCase("https://ld.admin.ch/FCh", "Bundeskanzlei", "Chancellerie fédérale", "Cancelleria federale")]
    [TestCase("https://ld.admin.ch/FC", "Bundesrat", "Conseil fédéral", "Consiglio federale")]
    [TestCase("https://politics.ld.admin.ch/council/S", "Ständerat", "Conseil des États", "Consiglio degli Stati")]
    [TestCase("https://politics.ld.admin.ch/person/3912", "Christian Wasserfallen", "Christian Wasserfallen", "Christian Wasserfallen")]
    [TestCase("https://politics.ld.admin.ch/person/3910", "Erich von Siebenthal", "Erich von Siebenthal", "Erich von Siebenthal")]
    [TestCase("https://politics.ld.admin.ch/council/committee/56", "Kommission 91.032-NR", "Commission 91.032-CN", null)]
    [TestCase("https://politics.ld.admin.ch/council/committee/17", "Aussenpolitische Kommission Ständerat", "Commission de politique extérieure Conseil des États", "Commissione della politica estera Consiglio degli Stati")]
    [TestCase("https://politics.ld.admin.ch/council/committee/632", "Kommission 04.080-SR", "Commission 04.080-CE", null)]
    public async Task GetAgents_ForUri_ShouldReturnName(string uri, string expectedTextDe, string expectedTextFr, string? expectedTextIt)
    {
        var agents = (await _agentClient.GetAgents(new[] { uri }, CancellationToken.None)).ToList();

        Assert.That(agents, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(agents[0].NameDe, Is.EqualTo(expectedTextDe));
            Assert.That(agents[0].NameFr, Is.EqualTo(expectedTextFr));
            Assert.That(agents[0].NameIt, Is.EqualTo(expectedTextIt));
        });
    }
}
