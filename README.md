# Introduction
This library contains functionality related to RDF triplestores based on Sparql.

| :warning: WARNING           |
|:----------------------------|
| Don't use this library, instead use [Swiss.FCh.MasterData](https://github.com/swiss/fch-masterdata). This library will be deprecated and merged with fch-masterdata.  |

## Master Data
MasterData that is stored as a concept on LINDAS can be queryied by e generic query provided by the library.
The defined termset has to be provided as parameter.

To do so, the ```IMasterDataClient``` can be used.
This service can be registered in your DI container by calling ```IServiceCollection.AddSparqlClient()```.

Damit die generische Query richtig funktioniert, müssen folgende Parameter übergeben werden.

An example call can the look like this:
```csharp
var queryResult =
    await _masterDataClient.GetMasterData(
        DefinedTermsets.Countries,
        ct,
        additionalAttributes: additionalAttributes,
        endpointUrl: SparqlEndpoints.LdAdmin,
        dataModel: DataModels.SchemaOrg);
```

To make these queries more convenient, the library [Swiss.FCh.MasterData](https://github.com/swiss/fch-masterdata) was developed, which we recommend to end users.

# Contribution
See: https://github.com/swiss/fch-sparql/blob/main/CONTRIBUTING.md

# Security
See: https://github.com/swiss/fch-sparql/blob/main/SECURITY.md

# Development Workflow

To publish a new version of the NuGet package, proceed as follows.

* apply and push your changes
* define and describe the new version in ```CHANGELOG.md```
* push the corresponding label with ```git tag vx.x.x``` and ```git push origin vx.x.x```
* go to GitHub -> Actions -> 'Build and Publish to NuGet.org' and trigger a run while specifying the correct GIT label
