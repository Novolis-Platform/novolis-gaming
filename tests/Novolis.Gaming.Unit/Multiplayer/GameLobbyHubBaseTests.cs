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

    [Test]
    public async Task SetReadyAsync_updates_lobby_and_broadcasts()
    {
        var lobby = new InMemoryLobbyState();
        var player = PlayerRef.New();
        lobby.TryAddPlayer(new LobbyPlayerSlot(player, false));
        var groupProxy = new RecordingClientProxy();
        var hub = new TestLobbyHub(lobby)
        {
            Context = new TestHubCallerContext(new ClaimsPrincipal(new ClaimsIdentity([player.ToPlayerRefClaim()]))),
            Clients = new TestHubCallerClients(groupProxy),
        };

        await hub.SetReadyAsync(lobby.Id.Value.ToString("D"), true);

        await Assert.That(lobby.Players[0].IsReady).IsTrue();
        await Assert.That(groupProxy.LastMethod).IsEqualTo("LobbyUpdated");
        await Assert.That(groupProxy.LastArgs).Count().IsEqualTo(1);
    }

    [Test]
    public async Task SetReadyAsync_throws_when_player_claim_missing()
    {
        var lobby = new InMemoryLobbyState();
        var hub = new TestLobbyHub(lobby)
        {
            Context = new TestHubCallerContext(new ClaimsPrincipal()),
            Clients = new TestHubCallerClients(new RecordingClientProxy()),
        };

        await Assert.ThrowsAsync<HubException>(async () =>
            await hub.SetReadyAsync(lobby.Id.Value.ToString("D"), true));
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

    private sealed class RecordingClientProxy : IClientProxy
    {
        public string? LastMethod { get; private set; }
        public object?[]? LastArgs { get; private set; }

        public Task SendCoreAsync(string methodName, object?[] args, CancellationToken cancellationToken = default)
        {
            LastMethod = methodName;
            LastArgs = args;
            return Task.CompletedTask;
        }
    }

    private sealed class TestHubCallerClients(IClientProxy groupProxy) : IHubCallerClients
    {
        public IClientProxy All => throw new NotSupportedException();
        public IClientProxy Caller => throw new NotSupportedException();
        public IClientProxy Others => throw new NotSupportedException();
        public IClientProxy AllExcept(IReadOnlyList<string> excludedConnectionIds) => throw new NotSupportedException();
        public IClientProxy Client(string connectionId) => throw new NotSupportedException();
        public IClientProxy Clients(IReadOnlyList<string> connectionIds) => throw new NotSupportedException();
        public IClientProxy Group(string groupName) => groupProxy;
        public IClientProxy Groups(IReadOnlyList<string> groupNames) => throw new NotSupportedException();
        public IClientProxy GroupExcept(string groupName, IReadOnlyList<string> excludedConnectionIds) => throw new NotSupportedException();
        public IClientProxy User(string userId) => throw new NotSupportedException();
        public IClientProxy Users(IReadOnlyList<string> userIds) => throw new NotSupportedException();
        public IClientProxy OthersInGroup(string groupName) => throw new NotSupportedException();
    }
}
