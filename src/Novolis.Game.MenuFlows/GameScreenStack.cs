namespace Novolis.Game.MenuFlows;

/// <summary>Stack-based menu flow without a UI framework dependency.</summary>
public sealed class GameScreenStack
{
    private readonly Stack<IGameScreen> _stack = new();

    /// <summary>Raised after a successful push or pop navigation.</summary>
    public event Action<GameScreenTransition>? Transitioned;

    /// <summary>Top of stack, or null when empty.</summary>
    public IGameScreen? Current => _stack.TryPeek(out var screen) ? screen : null;

    /// <summary>Pushes a screen and runs enter/exit lifecycle hooks.</summary>
    public async Task PushAsync(IGameScreen screen, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(screen);
        var from = Current?.ScreenId;
        if (Current is not null)
        {
            await Current.OnExitAsync(cancellationToken).ConfigureAwait(false);
        }

        _stack.Push(screen);
        await screen.OnEnterAsync(cancellationToken).ConfigureAwait(false);
        if (from is not null)
        {
            Transitioned?.Invoke(new GameScreenTransition(from, screen.ScreenId));
        }
    }

    /// <summary>Pops the top screen when present.</summary>
    /// <returns>True if a screen was popped.</returns>
    public async Task<bool> PopAsync(CancellationToken cancellationToken = default)
    {
        if (_stack.Count == 0)
        {
            return false;
        }

        var leaving = _stack.Pop();
        await leaving.OnExitAsync(cancellationToken).ConfigureAwait(false);
        if (Current is not null)
        {
            await Current.OnEnterAsync(cancellationToken).ConfigureAwait(false);
            Transitioned?.Invoke(new GameScreenTransition(leaving.ScreenId, Current.ScreenId));
        }

        return true;
    }
}
