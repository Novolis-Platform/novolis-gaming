namespace Novolis.Game.MenuFlows;

/// <summary>Optional base for pause overlays.</summary>
public abstract class PauseScreenBase : IGameScreen
{
    /// <inheritdoc />
    public virtual string ScreenId => "pause";

    /// <inheritdoc />
    public virtual ValueTask OnEnterAsync(CancellationToken cancellationToken = default) =>
        ValueTask.CompletedTask;

    /// <inheritdoc />
    public virtual ValueTask OnExitAsync(CancellationToken cancellationToken = default) =>
        ValueTask.CompletedTask;
}
