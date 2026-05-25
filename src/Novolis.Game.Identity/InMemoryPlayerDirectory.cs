using Novolis.Game.Identity.Abstractions;

namespace Novolis.Game.Identity;

/// <summary>Thread-safe in-memory directory; display names are optional and not persisted to disk.</summary>
public sealed class InMemoryPlayerDirectory : IPlayerDirectory
{
    private readonly Dictionary<Guid, string> _displayNames = new();

    /// <inheritdoc />
    public bool TryGetDisplayName(PlayerRef player, out string? displayName)
    {
        lock (_displayNames)
        {
            return _displayNames.TryGetValue(player.Value, out displayName);
        }
    }

    /// <inheritdoc />
    public void SetDisplayName(PlayerRef player, string displayName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
        lock (_displayNames)
        {
            _displayNames[player.Value] = displayName;
        }
    }

    /// <inheritdoc />
    public void Remove(PlayerRef player)
    {
        lock (_displayNames)
        {
            _displayNames.Remove(player.Value);
        }
    }
}
