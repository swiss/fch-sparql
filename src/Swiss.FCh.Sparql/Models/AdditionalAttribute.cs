namespace Swiss.FCh.Sparql.Models;

public class AdditionalAttribute
{
    public required string Term { get; init; }
    public required string Identifier { get; init; }
    public IEnumerable<string> Values { get; init; } = Enumerable.Empty<string>();
}
