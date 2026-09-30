namespace Novolis.Game.Procedural;

/// <summary>Maps run distance to a 0..1 difficulty intensity.</summary>
public sealed class DifficultyRamp
{
    readonly float _start;
    readonly float _fullAt;
    readonly float _ease;

    /// <summary>Gentle ramp that reaches ~1 around 2000 units.</summary>
    public static DifficultyRamp Default { get; } = new(0f, 2000f, 1.4f);

    /// <param name="startDistance">Distance where intensity begins rising.</param>
    /// <param name="fullAtDistance">Distance where intensity approaches 1.</param>
    /// <param name="ease">Exponent &gt; 1 eases in; &lt; 1 eases out.</param>
    public DifficultyRamp(float startDistance, float fullAtDistance, float ease = 1.2f)
    {
        _start = startDistance;
        _fullAt = Math.Max(startDistance + 1f, fullAtDistance);
        _ease = Math.Max(0.2f, ease);
    }

    /// <summary>Intensity in [0, 1] at the given world distance.</summary>
    public float Evaluate(float distance)
    {
        if (distance <= _start)
            return 0f;
        var t = Math.Clamp((distance - _start) / (_fullAt - _start), 0f, 1f);
        return MathF.Pow(t, _ease);
    }
}
