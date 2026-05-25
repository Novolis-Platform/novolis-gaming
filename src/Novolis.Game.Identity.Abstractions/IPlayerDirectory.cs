namespace Novolis.Game.Identity.Abstractions;

/// <summary>In-process player registry. Does not persist PII; optional display names supplied by the host app.</summary>
public interface IPlayerDirectory
{
    /// <summary>Gets a display name if the host previously set one.</summary>
    bool TryGetDisplayName(PlayerRef player, out string? displayName);

    /// <summary>Sets a non-PII display label for UI (nickname, guest label).</summary>
    void SetDisplayName(PlayerRef player, string displayName);

    /// <summary>Removes player metadata from memory.</summary>
    void Remove(PlayerRef player);
}
