using Novolis.Game.Identity.Abstractions;

namespace Novolis.Game.Multiplayer.Abstractions;

/// <summary>A player occupying a lobby slot.</summary>
/// <param name="Player">Pseudonymous player ref.</param>
/// <param name="IsReady">Whether the player marked ready.</param>
public sealed record LobbyPlayerSlot(PlayerRef Player, bool IsReady);
