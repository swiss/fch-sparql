using Swiss.FCh.Sparql.Models;

namespace Swiss.FCh.Sparql.Queries;

internal static class MasterDataQuery
{
    internal const string ColumnNameDefinedTermSet = "definedTermSet";
    internal const string ColumnNameDefinedTerm = "definedTerm";
    internal const string ColumnNameIdentifier = "identifier";
    internal const string ColumnNamePosition = "position";
    internal const string ColumnNameName = "name";
    internal const string ColumnNameStartDate = "startDate";
    internal const string ColumnNameEndDate = "endDate";
    internal const string ColumnNameValidFrom = "validFrom";
    internal const string ColumnNameValidTo = "validTo";
    internal const string ColumnNameAltName = "altName";

    internal static string GetMasterDataQuery(string definedTermSet, List<AdditionalAttribute>? additionalAttributes = null)
    {
         var query =
             @$"PREFIX schema: <http://schema.org/>
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>

                SELECT DISTINCT ?definedTermSet ?definedTerm ?identifier ?position ?name ?startDate ?validFrom ?endDate ?validTo ?altName {string.Join(" ", additionalAttributes?.Select(item => $"?{item.Identifier}") ?? Enumerable.Empty<string>())}
                WHERE {{
                  BIND (<{definedTermSet}> as ?definedTermSet)

                  ?definedTerm schema:inDefinedTermSet ?definedTermSet .

                  OPTIONAL {{
                    ?definedTerm schema:identifier ?identifier.
                    FILTER(isLiteral(?identifier) = true).
                  }}
                  OPTIONAL {{ ?definedTerm schema:position ?position. }}
                  OPTIONAL {{ ?definedTerm schema:startDate ?startDate. }}
                  OPTIONAL {{ ?definedTerm schema:validFrom ?validFrom . }}
                  OPTIONAL {{ ?definedTerm schema:endDate ?endDate. }}
                  OPTIONAL {{ ?definedTerm schema:validTo ?validTo . }}

                  {{
                    ?definedTerm schema:name ?name.
                    FILTER(lang(?name) = ""de"")
                    OPTIONAL {{
                      ?definedTerm schema:alternateName ?altName.
                      FILTER(lang(?altName) = ""de"")
                    }}
                  }}

                  UNION

                  {{
                    ?definedTerm schema:name ?name.
                    FILTER(lang(?name) = ""fr"")
                    OPTIONAL {{
                      ?definedTerm schema:alternateName ?altName.
                      FILTER(lang(?altName) = ""fr"")
                    }}
                  }}

                  UNION

                  {{
                    ?definedTerm schema:name ?name.
                    FILTER(lang(?name) = ""it"")
                    OPTIONAL {{
                      ?definedTerm schema:alternateName ?altName.
                      FILTER(lang(?altName) = ""it"")
                    }}
                  }}

                  {
                      string.Join("\n", additionalAttributes?.Select(item =>
                          @$"OPTIONAL {{
                               {(item.Values.Any() ? $"VALUES ?{item.Identifier} {{ {string.Join(" ", item.Values.Select(v => $"<{v}>"))} }}" : string.Empty)}
                               ?definedTerm <{item.Term}> ?{item.Identifier}
                             }}") ?? Enumerable.Empty<string>())
                  }
                }}
             ";

        return query;
    }
}
