namespace Novolis.Game.Identity.Abstractions;

/// <summary>One-way hash of provider subject; apps compute with their own salt/secret.</summary>
/// <param name="Value">Hash string.</param>
public readonly record struct ExternalSubjectHash(string Value)
{
    /// <inheritdoc />
    public override string ToString() => Value;
}
