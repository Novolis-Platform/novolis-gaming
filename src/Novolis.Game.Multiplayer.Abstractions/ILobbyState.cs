using Novolis.Game.Identity.Abstractions;

namespace Novolis.Game.Multiplayer.Abstractions;

/// <summary>Mutable lobby membership and ready flags.</summary>
public interface ILobbyState
{
    /// <summary>Lobby identifier.</summary>
    LobbyId Id { get; }

    /// <summary>Current players in the lobby.</summary>
    IReadOnlyList<LobbyPlayerSlot> Players { get; }

    /// <summary>Adds a player if not already present.</summary>
    bool TryAddPlayer(LobbyPlayerSlot slot);

    /// <summary>Removes a player from the lobby.</summary>
    bool TryRemovePlayer(PlayerRef player);

    /// <summary>Updates ready state for a seated player.</summary>
    bool TrySetReady(PlayerRef player, bool isReady);
}
