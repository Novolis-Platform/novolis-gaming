namespace Novolis.Game.Scenes;

/// <summary>Accumulator that emits a fixed sim step (clamped so hitches skip ahead).</summary>
public sealed class FixedStepClock
{
    private float _accum;

    /// <summary>Creates a clock with <paramref name="stepSeconds"/> and optional hitch clamp.</summary>
    public FixedStepClock(float stepSeconds = 1f / 60f, float maxAccumSeconds = 0.12f)
    {
        StepSeconds = stepSeconds > 0f ? stepSeconds : 1f / 60f;
        MaxAccumSeconds = maxAccumSeconds > StepSeconds ? maxAccumSeconds : StepSeconds * 8f;
    }

    /// <summary>Fixed tick length.</summary>
    public float StepSeconds { get; }

    /// <summary>Max queued time so a hitch does not spiral.</summary>
    public float MaxAccumSeconds { get; }

    /// <summary>Adds frame delta. Returns how many steps to run this frame.</summary>
    public int Consume(float deltaSeconds)
    {
        _accum += MathF.Max(0f, deltaSeconds);
        if (_accum > MaxAccumSeconds)
        {
            _accum = MaxAccumSeconds;
        }

        var steps = 0;
        while (_accum >= StepSeconds)
        {
            _accum -= StepSeconds;
            steps++;
        }

        return steps;
    }

    /// <summary>Clears queued time.</summary>
    public void Reset() => _accum = 0f;
}
