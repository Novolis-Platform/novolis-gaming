using System.Security.Claims;
using Novolis.Game.Identity.Abstractions;
using Novolis.Game.Identity.AspNetCore;

namespace Novolis.Gaming.Unit.Identity;

public class ClaimsPrincipalTests
{
    [Test]
    public async Task TryGetPlayerRef_Reads_Custom_Claim()
    {
        var player = PlayerRef.New();
        var principal = new ClaimsPrincipal(new ClaimsIdentity([player.ToPlayerRefClaim()]));
        await Assert.That(principal.TryGetPlayerRef(out var parsed)).IsTrue();
        await Assert.That(parsed).IsEqualTo(player);
    }
}
