using Novolis.Game.Identity.Abstractions;

namespace Novolis.Game.Identity;

/// <summary>Helpers for creating player refs in hosts and samples.</summary>
public static class PlayerRefFactory
{
    /// <summary>Creates a guest player and optional display label.</summary>
    public static PlayerRef CreateGuest(IPlayerDirectory directory, string? displayName = null)
    {
        var player = PlayerRef.New();
        if (!string.IsNullOrWhiteSpace(displayName))
        {
            directory.SetDisplayName(player, displayName);
        }

        return player;
    }
}
