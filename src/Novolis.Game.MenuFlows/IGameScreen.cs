namespace Novolis.Game.MenuFlows;

/// <summary>A single navigable screen (main menu, settings, pause overlay, etc.).</summary>
public interface IGameScreen
{
    /// <summary>Stable screen identifier for transitions and logging.</summary>
    string ScreenId { get; }

    /// <summary>Called when the screen becomes active.</summary>
    ValueTask OnEnterAsync(CancellationToken cancellationToken = default);

    /// <summary>Called when the screen is covered or removed from the stack.</summary>
    ValueTask OnExitAsync(CancellationToken cancellationToken = default);
}
