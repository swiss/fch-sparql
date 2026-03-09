using System.Net;
using Swiss.FCh.Sparql.Client;
using Swiss.FCh.Sparql.Client.Configuration;
using Swiss.FCh.Sparql.Models;
using NSubstitute;
using NSubstitute.ClearExtensions;

namespace Swiss.FCh.Sparql.Tests.Client;

[TestFixture]
internal sealed class MasterDataClientTests
{
    private readonly IHttpClientFactory _factory = Substitute.For<IHttpClientFactory>();
    private HttpClient _httpClient;
    private MasterDataClient _masterDataClient = null!;
    private HttpClientHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _handler = new HttpClientHandler
        {
            Proxy = new WebProxy("http://prxp01.admin.ch:8080", false),
            UseProxy = true
        };

        _httpClient = new HttpClient(_handler);

        _factory.CreateClient(SwissFChSparqlConstants.SparqlHttpClientName).Returns(_httpClient);
        _masterDataClient = new MasterDataClient(_factory);
    }

    [TearDown]
    public void TearDown()
    {
        _factory.ClearSubstitute();
        _httpClient.Dispose();
        _handler.Dispose();
    }

    [TestCase(DefinedTermsets.Offices)]
    [TestCase(DefinedTermsets.Committees)]
    [TestCase(DefinedTermsets.Departments)]
    [TestCase(DefinedTermsets.Sessions)]
    [TestCase(DefinedTermsets.Cantons)]
    [TestCase(DefinedTermsets.Countries)]
    public async Task GetMasterData_ForTermSets_ShouldDeliverData(string termSetUri)
    {
        var data = (await _masterDataClient.GetMasterData(termSetUri, CancellationToken.None)).ToList();

        Assert.That(data, Is.Not.Null);
        Assert.That(data, Is.InstanceOf<List<MasterData>>());
    }

    [Test]
    public async Task GetMasterData_ForCountries_ShouldDelieverAnEndDate()
    {
        var countries = (await _masterDataClient.GetMasterData(DefinedTermsets.Countries, CancellationToken.None)).ToList();

        Assert.That(countries.Any(x => x.End.HasValue && x.End.Value > DateOnly.MinValue), Is.True);
    }

    [Test]
    public async Task GetMasterData_ForOffices_ShouldDeliverPositionAttribute()
    {
        var offices = (await _masterDataClient.GetMasterData(DefinedTermsets.Offices, CancellationToken.None)).ToList();

        Assert.That(offices, Is.Not.Null);
        Assert.That(offices, Is.InstanceOf<List<MasterData>>());
        Assert.That(offices.Any(item => !string.IsNullOrWhiteSpace(item.Position)), Is.True);
    }

    [Test]
    public async Task GetMasterData_ForOfficesWithAdditionalAttributes_ShouldDeliverAdditionalAttributes()
    {
        var additionalAttributes = new List<AdditionalAttribute>
        {
            new() { Term = "http://schema.org/parentOrganization", Identifier = "departmentUri" },
            new() { Term = "http://schema.org/additionalType", Identifier = "generalSecretariat", Values = new []{"https://register.ld.admin.ch/termdat/441817"}},
            new() { Term = "http://schema.org/additionalType", Identifier = "centralFederalAdministration", Values = new []{"https://register.ld.admin.ch/termdat/57178", "https://register.ld.admin.ch/termdat/57179"}}
        };

        var offices =
            (await _masterDataClient.GetMasterData(DefinedTermsets.Offices, CancellationToken.None, additionalAttributes: additionalAttributes)).ToList();

        Assert.That(offices, Is.Not.Null);
        Assert.That(offices, Is.InstanceOf<List<MasterData>>());

        foreach (var office in offices)
        {
            Assert.That(office.AdditionalAttributes, Is.Not.Null);
            Assert.That(office.AdditionalAttributes, Has.Count.EqualTo(3));
            Assert.That(office.AdditionalAttributes.ContainsKey(additionalAttributes[0].Identifier), Is.True);
            Assert.That(office.AdditionalAttributes.ContainsKey(additionalAttributes[1].Identifier), Is.True);
            Assert.That(office.AdditionalAttributes.ContainsKey(additionalAttributes[2].Identifier), Is.True);
        }
    }
}
