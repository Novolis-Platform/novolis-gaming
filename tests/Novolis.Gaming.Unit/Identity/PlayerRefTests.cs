using Novolis.Game.Identity;
using Novolis.Game.Identity.Abstractions;

namespace Novolis.Gaming.Unit.Identity;

public class PlayerRefTests
{
    [Test]
    public async Task PlayerRef_RoundTrips_Guid()
    {
        var original = PlayerRef.New();
        var parsed = PlayerRef.FromGuid(original.Value);
        await Assert.That(parsed).IsEqualTo(original);
    }

    [Test]
    public async Task Directory_Stores_DisplayName_Not_Pii_Type()
    {
        var directory = new InMemoryPlayerDirectory();
        var player = PlayerRefFactory.CreateGuest(directory, "Guest-1");
        await Assert.That(directory.TryGetDisplayName(player, out var name)).IsTrue();
        await Assert.That(name).IsEqualTo("Guest-1");
    }
}
