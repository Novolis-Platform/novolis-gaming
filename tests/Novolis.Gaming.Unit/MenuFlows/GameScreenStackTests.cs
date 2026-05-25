using Novolis.Game.MenuFlows;

namespace Novolis.Gaming.Unit.MenuFlows;

public class GameScreenStackTests
{
    [Test]
    public async Task Push_And_Pop_Navigates_Stack()
    {
        var stack = new GameScreenStack();
        var main = new TestScreen("main");
        var settings = new TestScreen("settings");
        await stack.PushAsync(main);
        await stack.PushAsync(settings);
        await Assert.That(stack.Current?.ScreenId).IsEqualTo("settings");
        await Assert.That(await stack.PopAsync()).IsTrue();
        await Assert.That(stack.Current?.ScreenId).IsEqualTo("main");
    }

    private sealed class TestScreen(string id) : IGameScreen
    {
        public string ScreenId => id;

        public ValueTask OnEnterAsync(CancellationToken cancellationToken = default) => ValueTask.CompletedTask;

        public ValueTask OnExitAsync(CancellationToken cancellationToken = default) => ValueTask.CompletedTask;
    }
}
