using System.Security.Claims;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using Novolis.Game.Identity.Abstractions;
using Novolis.Game.Identity.AspNetCore;
using Novolis.Game.Multiplayer.Abstractions;
using Novolis.Game.Multiplayer.AspNetCore;

namespace Novolis.Gaming.Unit.Multiplayer;

public sealed class GameLobbyHubBaseTests
{
    [Test]
    public async Task JoinLobbyAsync_returns_snapshot_for_authenticated_player()
    {
        var lobby = new InMemoryLobbyState();
        var player = PlayerRef.New();
        var hub = new TestLobbyHub(lobby)
        {
            Context = new TestHubCallerContext(new ClaimsPrincipal(new ClaimsIdentity([player.ToPlayerRefClaim()])))
        };

        var dto = await hub.JoinLobbyAsync(lobby.Id.Value.ToString("D"));

        await Assert.That(dto.LobbyId).IsEqualTo(lobby.Id.Value.ToString("D"));
        await Assert.That(dto.Players).Count().IsEqualTo(1);
        await Assert.That(dto.Players[0].PlayerRef).IsEqualTo(player.Value.ToString("D"));
    }

    [Test]
    public async Task JoinLobbyAsync_throws_when_player_claim_missing()
    {
        var lobby = new InMemoryLobbyState();
        var hub = new TestLobbyHub(lobby)
        {
            Context = new TestHubCallerContext(new ClaimsPrincipal())
        };

        await Assert.ThrowsAsync<HubException>(async () =>
            await hub.JoinLobbyAsync(lobby.Id.Value.ToString("D")));
    }

    private sealed class TestLobbyHub(ILobbyState lobby) : GameLobbyHubBase
    {
        protected override ILobbyState GetLobby(LobbyId lobbyId)
        {
            _ = lobbyId;
            return lobby;
        }
    }

    private sealed class TestHubCallerContext(ClaimsPrincipal user) : HubCallerContext
    {
        public override string ConnectionId => "connection-1";
        public override string? UserIdentifier => user.Identity?.Name;
        public override ClaimsPrincipal? User => user;
        public override IDictionary<object, object?> Items { get; } = new Dictionary<object, object?>();
        public override CancellationToken ConnectionAborted => CancellationToken.None;
        public override IFeatureCollection Features { get; } = new FeatureCollection();

        public override void Abort()
        {
        }
    }
}
