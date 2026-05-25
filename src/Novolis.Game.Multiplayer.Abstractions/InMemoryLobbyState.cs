using Novolis.Game.Identity.Abstractions;

namespace Novolis.Game.Multiplayer.Abstractions;

/// <summary>Thread-safe in-memory lobby for tests and prototypes.</summary>
public sealed class InMemoryLobbyState : ILobbyState
{
    private readonly List<LobbyPlayerSlot> _players = new();

    /// <summary>Creates a lobby with optional fixed id.</summary>
    public InMemoryLobbyState(LobbyId? id = null) => Id = id ?? LobbyId.New();

    /// <inheritdoc />
    public LobbyId Id { get; }

    /// <inheritdoc />
    public IReadOnlyList<LobbyPlayerSlot> Players
    {
        get
        {
            lock (_players)
            {
                return _players.ToList();
            }
        }
    }

    /// <inheritdoc />
    public bool TryAddPlayer(LobbyPlayerSlot slot)
    {
        lock (_players)
        {
            if (_players.Any(p => p.Player.Value == slot.Player.Value))
            {
                return false;
            }

            _players.Add(slot);
            return true;
        }
    }

    /// <inheritdoc />
    public bool TryRemovePlayer(PlayerRef player)
    {
        lock (_players)
        {
            return _players.RemoveAll(p => p.Player.Value == player.Value) > 0;
        }
    }

    /// <inheritdoc />
    public bool TrySetReady(PlayerRef player, bool isReady)
    {
        lock (_players)
        {
            var index = _players.FindIndex(p => p.Player.Value == player.Value);
            if (index < 0)
            {
                return false;
            }

            _players[index] = _players[index] with { IsReady = isReady };
            return true;
        }
    }
}
