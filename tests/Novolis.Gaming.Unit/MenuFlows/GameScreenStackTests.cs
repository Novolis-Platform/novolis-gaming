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

    [Test]
    public async Task PopAsync_Returns_False_When_Empty()
    {
        var stack = new GameScreenStack();
        await Assert.That(await stack.PopAsync()).IsFalse();
    }

    [Test]
    public async Task PushAsync_Raises_Transitioned()
    {
        var stack = new GameScreenStack();
        GameScreenTransition? transition = null;
        stack.Transitioned += t => transition = t;
        await stack.PushAsync(new TestScreen("main"));
        await stack.PushAsync(new TestScreen("settings"));
        await Assert.That(transition?.FromScreenId).IsEqualTo("main");
        await Assert.That(transition?.ToScreenId).IsEqualTo("settings");
    }

    private sealed class TestScreen(string id) : IGameScreen
    {
        public string ScreenId => id;

        public ValueTask OnEnterAsync(CancellationToken cancellationToken = default) => ValueTask.CompletedTask;

        public ValueTask OnExitAsync(CancellationToken cancellationToken = default) => ValueTask.CompletedTask;
    }
}
