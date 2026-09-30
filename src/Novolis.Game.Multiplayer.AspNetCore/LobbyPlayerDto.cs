using Novolis.Game.Identity.Abstractions;

namespace Novolis.Game.Multiplayer.AspNetCore;

/// <summary>Wire-friendly player slot.</summary>
/// <param name="PlayerRef">Player GUID string.</param>
/// <param name="IsReady">Ready flag.</param>
public sealed record LobbyPlayerDto(string PlayerRef, bool IsReady);
