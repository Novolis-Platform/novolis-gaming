using Novolis.Game.Identity.Abstractions;

namespace Novolis.Game.Multiplayer.AspNetCore;

/// <summary>Maps between domain lobby state and DTOs.</summary>
public static class LobbyMapping
{
    /// <summary>Projects lobby state to a DTO.</summary>
    public static LobbyDto ToDto(Abstractions.ILobbyState lobby) =>
        new(
            lobby.Id.Value.ToString("D"),
            lobby.Players.Select(p => new LobbyPlayerDto(p.Player.Value.ToString("D"), p.IsReady)).ToList());

    /// <summary>Parses a player ref from a GUID string.</summary>
    public static bool TryParsePlayerRef(string value, out PlayerRef player)
    {
        if (Guid.TryParse(value, out var id))
        {
            player = new PlayerRef(id);
            return true;
        }

        player = default;
        return false;
    }
}
