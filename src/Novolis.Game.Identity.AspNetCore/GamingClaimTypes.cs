namespace Novolis.Game.Identity.AspNetCore;

/// <summary>Claim types used by Novolis game hosts (set in your auth pipeline).</summary>
public static class GamingClaimTypes
{
    /// <summary>GUID string for <see cref="Abstractions.PlayerRef"/>.</summary>
    public const string PlayerRef = "novolis:player_ref";
}
