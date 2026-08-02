using Novolis.Game.Identity.Abstractions;
using Novolis.Game.Multiplayer.Abstractions;

namespace Novolis.Gaming.Unit.Multiplayer;

public class LobbyStateTests
{
    [Test]
    public async Task Lobby_Tracks_Ready_State()
    {
        var lobby = new InMemoryLobbyState();
        var player = PlayerRef.New();
        lobby.TryAddPlayer(new LobbyPlayerSlot(player, false));
        lobby.TrySetReady(player, true);
        await Assert.That(lobby.Players[0].IsReady).IsTrue();
    }

    [Test]
    public async Task LobbyId_ToString_Uses_Guid_Format()
    {
        var id = LobbyId.New();
        await Assert.That(id.ToString()).IsEqualTo(id.Value.ToString("D"));
    }

    [Test]
    public async Task Lobby_Rejects_Duplicate_Players()
    {
        var lobby = new InMemoryLobbyState();
        var player = PlayerRef.New();
        await Assert.That(lobby.TryAddPlayer(new LobbyPlayerSlot(player, false))).IsTrue();
        await Assert.That(lobby.TryAddPlayer(new LobbyPlayerSlot(player, true))).IsFalse();
    }

    [Test]
    public async Task Lobby_Removes_Players()
    {
        var lobby = new InMemoryLobbyState();
        var player = PlayerRef.New();
        lobby.TryAddPlayer(new LobbyPlayerSlot(player, false));
        await Assert.That(lobby.TryRemovePlayer(player)).IsTrue();
        await Assert.That(lobby.Players).IsEmpty();
    }

    [Test]
    public async Task Lobby_SetReady_Fails_For_Unknown_Player()
    {
        var lobby = new InMemoryLobbyState();
        await Assert.That(lobby.TrySetReady(PlayerRef.New(), true)).IsFalse();
    }
}
