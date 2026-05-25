namespace Novolis.Game.Multiplayer.Abstractions;

/// <summary>Identifies a multiplayer lobby instance.</summary>
/// <param name="Value">Underlying GUID.</param>
public readonly record struct LobbyId(Guid Value)
{
    /// <summary>Creates a new lobby id.</summary>
    public static LobbyId New() => new(Guid.NewGuid());

    /// <inheritdoc />
    public override string ToString() => Value.ToString("D");
}
