using System.Xml;

namespace Swiss.FCh.Sparql.Extensions;

public static class SparqlStringExtensions
{
    // RDF Library seems to be XML based, so some unicode characters are not valid for text fields and need to be filtered
    public static string? AsSanitizedXmlString(this string? text)
    {
        return text is null ? null : new string(text.Where(XmlConvert.IsXmlChar).ToArray());
    }
}
