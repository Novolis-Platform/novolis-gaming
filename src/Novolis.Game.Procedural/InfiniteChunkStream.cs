namespace Novolis.Game.Procedural;

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
