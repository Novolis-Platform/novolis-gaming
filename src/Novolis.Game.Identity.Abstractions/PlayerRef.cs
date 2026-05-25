namespace Novolis.Game.Identity.Abstractions;

/// <summary>Opaque stable player identifier. Never embed email, username, or provider subject.</summary>
/// <param name="Value">Underlying GUID.</param>
public readonly record struct PlayerRef(Guid Value)
{
    /// <summary>Creates a new random player ref.</summary>
    public static PlayerRef New() => new(Guid.NewGuid());

    /// <summary>Creates a ref from an existing GUID.</summary>
    public static PlayerRef FromGuid(Guid value) => new(value);

    /// <inheritdoc />
    public override string ToString() => Value.ToString("D");
}
