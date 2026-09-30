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
