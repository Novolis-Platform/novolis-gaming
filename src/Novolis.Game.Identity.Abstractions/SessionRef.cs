namespace Novolis.Game.Identity.Abstractions;

/// <summary>Rotatable session scope (lobby, match, or login session).</summary>
/// <param name="Value">Underlying GUID.</param>
public readonly record struct SessionRef(Guid Value)
{
    /// <summary>Creates a new session ref.</summary>
    public static SessionRef New() => new(Guid.NewGuid());

    /// <inheritdoc />
    public override string ToString() => Value.ToString("D");
}
