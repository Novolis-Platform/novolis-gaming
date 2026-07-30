namespace Novolis.Game.Humanoid;

/// <summary>Common locomotion clip slots for game banks.</summary>
public enum LocomotionClipKind
{
    /// <summary>Standing / breathing idle.</summary>
    Idle,

    /// <summary>Walk cycle.</summary>
    Walk,

    /// <summary>Run cycle.</summary>
    Run,

    /// <summary>Jump / leave ground.</summary>
    Jump,

    /// <summary>Falling / in-air.</summary>
    Fall,

    /// <summary>Land recovery.</summary>
    Land,
}
