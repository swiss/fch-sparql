namespace Swiss.FCh.Sparql.Models;

public class MasterData
{
    public required string DefinedTermSet { get; init; }

    /// <summary>
    /// Contains the URI of the triple.
    /// </summary>
    public required string DefinedTerm { get; init; }
    public string? Identifier { get; init; }
    public string? Position { get; init; }
    public string? NameDe { get; internal set; }
    public string? NameFr { get; internal set; }
    public string? NameIt { get; internal set; }
    public DateOnly? Start { get; init; }
    public DateOnly? End { get; init; }
    public string? ShortNameDe { get; internal set; }
    public string? ShortNameFr { get; internal set; }
    public string? ShortNameIt { get; internal set; }
    public IDictionary<string, string?>? AdditionalAttributes { get; internal set; }
}
