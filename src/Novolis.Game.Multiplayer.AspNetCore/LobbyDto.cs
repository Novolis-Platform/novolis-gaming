using Novolis.Game.Identity.Abstractions;

namespace Novolis.Game.Multiplayer.AspNetCore;

/// <summary>Wire-friendly lobby snapshot.</summary>
/// <param name="LobbyId">Lobby GUID string.</param>
/// <param name="Players">Seated players.</param>
public sealed record LobbyDto(string LobbyId, IReadOnlyList<LobbyPlayerDto> Players);
