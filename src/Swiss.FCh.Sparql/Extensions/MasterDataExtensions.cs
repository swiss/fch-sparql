using Swiss.FCh.Sparql.Models;
using VDS.RDF;

namespace Swiss.FCh.Sparql.Extensions;

internal static class MasterDataExtensions
{
    internal static void AddName(this MasterData masterData, LiteralNode? name)
    {
        if (name is null)
        {
            return;
        }

        switch (name.Language)
        {
            case "de":
                masterData.NameDe = name.Value;
                break;
            case "fr":
                masterData.NameFr = name.Value;
                break;
            case "it":
                masterData.NameIt = name.Value;
                break;
        }
    }

    internal static void AddAltName(this MasterData masterData, LiteralNode? altName)
    {
        if (altName is null)
        {
            return;
        }

        switch (altName.Language)
        {
            case "de":
                masterData.ShortNameDe = altName.Value;
                break;
            case "fr":
                masterData.ShortNameFr = altName.Value;
                break;
            case "it":
                masterData.ShortNameIt = altName.Value;
                break;
        }
    }
}
