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
}
