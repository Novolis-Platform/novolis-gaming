using Novolis.Game.Identity;
using Novolis.Game.Identity.Abstractions;

namespace Novolis.Gaming.Unit.Identity;

public sealed class IdentityRefTests
{
    [Test]
    public async Task PlayerRef_ToString_Uses_Guid_Format()
    {
        var player = PlayerRef.FromGuid(Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"));
        await Assert.That(player.ToString()).IsEqualTo("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
    }

    [Test]
    public async Task DeviceRef_New_And_ToString()
    {
        var device = DeviceRef.New();
        await Assert.That(device.Value).IsNotEqualTo(Guid.Empty);
        await Assert.That(device.ToString()).IsEqualTo(device.Value.ToString("D"));
    }

    [Test]
    public async Task SessionRef_New_And_ToString()
    {
        var session = SessionRef.New();
        await Assert.That(session.Value).IsNotEqualTo(Guid.Empty);
        await Assert.That(session.ToString()).IsEqualTo(session.Value.ToString("D"));
    }

    [Test]
    public async Task ExternalProviderRef_ToString_Returns_Value()
    {
        var provider = new ExternalProviderRef("steam");
        await Assert.That(provider.ToString()).IsEqualTo("steam");
    }

    [Test]
    public async Task ExternalSubjectHash_ToString_Returns_Value()
    {
        var hash = new ExternalSubjectHash("abc123");
        await Assert.That(hash.ToString()).IsEqualTo("abc123");
    }

    [Test]
    public async Task ExternalIdentityLinker_Links_And_Resolves()
    {
        var linker = new InMemoryExternalIdentityLinker();
        var provider = new ExternalProviderRef("xbox");
        var subject = new ExternalSubjectHash("deadbeef");
        var player = PlayerRef.New();

        await Assert.That(linker.TryLink(provider, subject, out _)).IsFalse();

        linker.Link(provider, subject, player);
        await Assert.That(linker.TryLink(provider, subject, out var resolved)).IsTrue();
        await Assert.That(resolved).IsEqualTo(player);
    }

    [Test]
    public async Task PlayerDirectory_Remove_Clears_DisplayName()
    {
        var directory = new InMemoryPlayerDirectory();
        var player = PlayerRefFactory.CreateGuest(directory, "Guest-2");
        directory.Remove(player);
        await Assert.That(directory.TryGetDisplayName(player, out _)).IsFalse();
    }

    [Test]
    public async Task PlayerDirectory_Rejects_Blank_DisplayName()
    {
        var directory = new InMemoryPlayerDirectory();
        var player = PlayerRef.New();
        await Assert.ThrowsAsync<ArgumentException>(() =>
        {
            directory.SetDisplayName(player, "   ");
            return Task.CompletedTask;
        });
    }
}
