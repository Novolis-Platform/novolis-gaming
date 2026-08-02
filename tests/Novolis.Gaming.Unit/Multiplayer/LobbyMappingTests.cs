using Novolis.Game.Identity.Abstractions;
using Novolis.Game.Multiplayer.Abstractions;
using Novolis.Game.Multiplayer.AspNetCore;

namespace Novolis.Gaming.Unit.Multiplayer;

public sealed class LobbyMappingTests
{
    [Test]
    public async Task ToDto_projects_lobby_id_and_ready_flags()
    {
        var lobby = new InMemoryLobbyState();
        var player = PlayerRef.New();
        lobby.TryAddPlayer(new LobbyPlayerSlot(player, true));

        var dto = LobbyMapping.ToDto(lobby);

        await Assert.That(dto.LobbyId).IsEqualTo(lobby.Id.Value.ToString("D"));
        await Assert.That(dto.Players).Count().IsEqualTo(1);
        await Assert.That(dto.Players[0].PlayerRef).IsEqualTo(player.Value.ToString("D"));
        await Assert.That(dto.Players[0].IsReady).IsTrue();
    }

    [Test]
    public async Task TryParsePlayerRef_parses_guid_strings()
    {
        var player = PlayerRef.New();
        var parsed = LobbyMapping.TryParsePlayerRef(player.Value.ToString("D"), out var result);

        await Assert.That(parsed).IsTrue();
        await Assert.That(result).IsEqualTo(player);
    }

    [Test]
    public async Task TryParsePlayerRef_rejects_invalid_values()
    {
        var parsed = LobbyMapping.TryParsePlayerRef("not-a-guid", out _);
        await Assert.That(parsed).IsFalse();
    }
}
