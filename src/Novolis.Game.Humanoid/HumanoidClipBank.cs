using Novolis.Simulation.Humanoid;

namespace Novolis.Game.Humanoid;

/// <summary>Named bank of <see cref="HumanoidAnimationClip"/> assets for a character.</summary>
public sealed class HumanoidClipBank
{
    private readonly Dictionary<LocomotionClipKind, HumanoidAnimationClip> _locomotion = new();
    private readonly Dictionary<string, HumanoidAnimationClip> _named = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Registers a locomotion clip.</summary>
    public HumanoidClipBank Set(LocomotionClipKind kind, HumanoidAnimationClip clip)
    {
        ArgumentNullException.ThrowIfNull(clip);
        _locomotion[kind] = clip;
        return this;
    }

    /// <summary>Registers a free-form named clip (attacks, emotes, …).</summary>
    public HumanoidClipBank Set(string name, HumanoidAnimationClip clip)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(clip);
        _named[name] = clip;
        return this;
    }

    /// <summary>Gets a locomotion clip if registered.</summary>
    public bool TryGet(LocomotionClipKind kind, out HumanoidAnimationClip clip) =>
        _locomotion.TryGetValue(kind, out clip!);

    /// <summary>Gets a named clip if registered.</summary>
    public bool TryGet(string name, out HumanoidAnimationClip clip) =>
        _named.TryGetValue(name, out clip!);

    /// <summary>Samples a locomotion clip into <paramref name="pose"/>.</summary>
    public bool Sample(LocomotionClipKind kind, float timeSeconds, HumanoidPose pose, HumanoidBindPose? bind = null)
    {
        if (!_locomotion.TryGetValue(kind, out var clip))
            return false;
        clip.Sample(timeSeconds, pose, bind);
        return true;
    }
}
