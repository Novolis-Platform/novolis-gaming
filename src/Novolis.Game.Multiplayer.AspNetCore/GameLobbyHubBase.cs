using Microsoft.AspNetCore.SignalR;
using Novolis.Game.Identity.Abstractions;
using Novolis.Game.Identity.AspNetCore;
using Novolis.Game.Multiplayer.Abstractions;

namespace Novolis.Game.Multiplayer.AspNetCore;

/// <summary>Base hub: resolves <see cref="PlayerRef"/> from claims and broadcasts lobby snapshots.</summary>
public abstract class GameLobbyHubBase : Hub
{
    /// <summary>Resolves lobby state for the given id (host supplies storage).</summary>
    protected abstract ILobbyState GetLobby(LobbyId lobbyId);

    /// <summary>Gets the caller player from SignalR user claims.</summary>
    protected bool TryGetCallerPlayer(out PlayerRef player)
    {
        if (Context.User?.TryGetPlayerRef(out player) == true)
        {
            return true;
        }

        player = default;
        return false;
    }

    /// <summary>Joins a lobby and returns the current snapshot.</summary>
    public Task<LobbyDto> JoinLobbyAsync(string lobbyId)
    {
        if (!TryGetCallerPlayer(out var player))
        {
            throw new HubException("Missing player claim.");
        }

        var lobby = GetLobby(new LobbyId(Guid.Parse(lobbyId)));
        lobby.TryAddPlayer(new LobbyPlayerSlot(player, false));
        return Task.FromResult(LobbyMapping.ToDto(lobby));
    }

    /// <summary>Sets ready state and notifies the lobby group.</summary>
    public Task SetReadyAsync(string lobbyId, bool isReady)
    {
        if (!TryGetCallerPlayer(out var player))
        {
            throw new HubException("Missing player claim.");
        }

        var lobby = GetLobby(new LobbyId(Guid.Parse(lobbyId)));
        lobby.TrySetReady(player, isReady);
        return Clients.Group(lobbyId).SendAsync("LobbyUpdated", LobbyMapping.ToDto(lobby));
    }
}
