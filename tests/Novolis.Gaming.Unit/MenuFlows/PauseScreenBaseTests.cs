using Novolis.Game.MenuFlows;

namespace Novolis.Gaming.Unit.MenuFlows;

public sealed class PauseScreenBaseTests
{
    [Test]
    public async Task PauseScreenBase_Uses_Default_Lifecycle()
    {
        var screen = new TestPauseScreen();
        await Assert.That(screen.ScreenId).IsEqualTo("pause");
        await screen.OnEnterAsync();
        await screen.OnExitAsync();
    }

    private sealed class TestPauseScreen : PauseScreenBase;
}
