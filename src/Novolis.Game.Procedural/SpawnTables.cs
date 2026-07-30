namespace Novolis.Game.Procedural;

/// <summary>Weighted random pick table (spawn tables, loot, biome props).</summary>
public sealed class WeightedTable<T>
{
    readonly List<(T Item, float Weight)> _entries = [];
    float _total;

    /// <summary>Adds an entry with non-negative weight.</summary>
    public WeightedTable<T> Add(T item, float weight)
    {
        if (weight <= 0)
            return this;
        _entries.Add((item, weight));
        _total += weight;
        return this;
    }

    /// <summary>Number of entries.</summary>
    public int Count => _entries.Count;

    /// <summary>Picks one item using <paramref name="rng"/>. Throws if empty.</summary>
    public T Pick(ref SeededRng rng)
    {
        if (_entries.Count == 0 || _total <= 0)
            throw new InvalidOperationException("WeightedTable is empty.");

        var roll = rng.NextSingle() * _total;
        var acc = 0f;
        for (var i = 0; i < _entries.Count; i++)
        {
            acc += _entries[i].Weight;
            if (roll < acc)
                return _entries[i].Item;
        }

        return _entries[^1].Item;
    }

    /// <summary>Tries to pick; returns false when empty.</summary>
    public bool TryPick(ref SeededRng rng, out T item)
    {
        if (_entries.Count == 0 || _total <= 0)
        {
            item = default!;
            return false;
        }

        item = Pick(ref rng);
        return true;
    }
}

/// <summary>Discrete biome band from a noise sample (for terrain prop / palette selection).</summary>
public enum BiomeKind
{
    Plains = 0,
    Hills = 1,
    Forest = 2,
    Wetland = 3,
    Barrens = 4
}

/// <summary>Maps FBM moisture/height into a <see cref="BiomeKind"/>.</summary>
public static class BiomeSampler
{
    /// <summary>Samples biome at world XZ using separate height and moisture seeds.</summary>
    public static BiomeKind Sample(float x, float z, ulong worldSeed, float frequency = 0.01f)
    {
        var height = Noise.Fbm2D(x * frequency, z * frequency, worldSeed, octaves: 3);
        var moist = Noise.Fbm2D(x * frequency * 1.3f, z * frequency * 1.3f, SeededRng.Mix(worldSeed, 99), octaves: 3);
        if (height > 0.45f)
            return BiomeKind.Hills;
        if (moist > 0.35f && height < 0.1f)
            return BiomeKind.Wetland;
        if (moist > 0.15f)
            return BiomeKind.Forest;
        if (height < -0.35f)
            return BiomeKind.Barrens;
        return BiomeKind.Plains;
    }
}
