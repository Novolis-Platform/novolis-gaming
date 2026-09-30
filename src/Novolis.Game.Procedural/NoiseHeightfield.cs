namespace Novolis.Game.Procedural;

/// <summary>FBM heightfield with configurable scale and amplitude.</summary>
public sealed class NoiseHeightfield : IHeightSampler
{
    readonly ulong _seed;
    readonly float _frequency;
    readonly float _amplitude;
    readonly float _baseHeight;
    readonly int _octaves;

    /// <param name="seed">World seed.</param>
    /// <param name="frequency">Noise frequency (world units → noise space).</param>
    /// <param name="amplitude">Peak height variation.</param>
    /// <param name="baseHeight">Mid-level height.</param>
    /// <param name="octaves">FBM octaves.</param>
    public NoiseHeightfield(
        ulong seed,
        float frequency = 0.02f,
        float amplitude = 12f,
        float baseHeight = 0f,
        int octaves = 4)
    {
        _seed = seed == 0 ? 1UL : seed;
        _frequency = Math.Max(1e-6f, frequency);
        _amplitude = amplitude;
        _baseHeight = baseHeight;
        _octaves = Math.Clamp(octaves, 1, 12);
    }

    /// <inheritdoc />
    public float SampleHeight(float x, float z) =>
        _baseHeight + Noise.Fbm2D(x * _frequency, z * _frequency, _seed, _octaves) * _amplitude;
}
