namespace Novolis.Game.Procedural;

/// <summary>Samples a height at world XZ (Y-up, planar XZ).</summary>
public interface IHeightSampler
{
    /// <summary>World-space height at (<paramref name="x"/>, <paramref name="z"/>).</summary>
    float SampleHeight(float x, float z);
}

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

/// <summary>Integer chunk coordinate on an XZ grid.</summary>
public readonly record struct ChunkCoord(int X, int Z);

/// <summary>
/// Real-time infinite chunk window around a focus point. Load/unload callbacks fire as the
/// focus moves — suitable for open terrain or strip runners (use Z=0 chunks).
/// </summary>
public sealed class InfiniteChunkStream
{
    readonly HashSet<ChunkCoord> _loaded = [];
    readonly List<ChunkCoord> _scratch = [];

    /// <summary>World-space size of one chunk edge.</summary>
    public float ChunkSize { get; }

    /// <summary>How many chunks to keep around the focus (Chebyshev radius).</summary>
    public int Radius { get; set; }

    /// <summary>Currently loaded chunk coords.</summary>
    public IReadOnlyCollection<ChunkCoord> Loaded => _loaded;

    /// <summary>Raised when a chunk enters the window.</summary>
    public event Action<ChunkCoord>? ChunkLoaded;

    /// <summary>Raised when a chunk leaves the window.</summary>
    public event Action<ChunkCoord>? ChunkUnloaded;

    /// <summary>Creates a stream with the given chunk size and keep-radius.</summary>
    public InfiniteChunkStream(float chunkSize = 32f, int radius = 2)
    {
        ChunkSize = Math.Max(1f, chunkSize);
        Radius = Math.Max(0, radius);
    }

    /// <summary>Maps a world XZ position to a chunk coordinate.</summary>
    public ChunkCoord WorldToChunk(float x, float z) =>
        new((int)MathF.Floor(x / ChunkSize), (int)MathF.Floor(z / ChunkSize));

    /// <summary>
    /// Updates the loaded set around (<paramref name="focusX"/>, <paramref name="focusZ"/>).
    /// Call every frame / tick from the game loop.
    /// </summary>
    public void Update(float focusX, float focusZ)
    {
        var center = WorldToChunk(focusX, focusZ);
        _scratch.Clear();
        for (var dz = -Radius; dz <= Radius; dz++)
        for (var dx = -Radius; dx <= Radius; dx++)
            _scratch.Add(new ChunkCoord(center.X + dx, center.Z + dz));

        foreach (var coord in _scratch)
        {
            if (_loaded.Add(coord))
                ChunkLoaded?.Invoke(coord);
        }

        List<ChunkCoord>? toRemove = null;
        foreach (var coord in _loaded)
        {
            var keep = Math.Abs(coord.X - center.X) <= Radius
                       && Math.Abs(coord.Z - center.Z) <= Radius;
            if (keep)
                continue;
            toRemove ??= [];
            toRemove.Add(coord);
        }

        if (toRemove is null)
            return;

        foreach (var coord in toRemove)
        {
            _loaded.Remove(coord);
            ChunkUnloaded?.Invoke(coord);
        }
    }
}
