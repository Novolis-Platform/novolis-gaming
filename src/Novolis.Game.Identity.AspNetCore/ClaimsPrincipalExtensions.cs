using System.Security.Claims;
using Novolis.Game.Identity.Abstractions;

namespace Novolis.Game.Identity.AspNetCore;

/// <summary>Maps ASP.NET principals to <see cref="PlayerRef"/>.</summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>Reads <see cref="GamingClaimTypes.PlayerRef"/> when present.</summary>
    public static bool TryGetPlayerRef(this ClaimsPrincipal principal, out PlayerRef player)
    {
        var claim = principal.FindFirst(GamingClaimTypes.PlayerRef)?.Value;
        if (claim is not null && Guid.TryParse(claim, out var id))
        {
            player = new PlayerRef(id);
            return true;
        }

        player = default;
        return false;
    }

    /// <summary>Creates a claim carrying a player ref GUID.</summary>
    public static Claim ToPlayerRefClaim(this PlayerRef player) =>
        new(GamingClaimTypes.PlayerRef, player.Value.ToString("D"), ClaimValueTypes.String);
}
