namespace Novolis.Game.Procedural;

/// <summary>Deterministic 64-bit SplitMix-style PRNG (fast, seedable, no allocations).</summary>
public struct SeededRng
{
    ulong _state;

    /// <summary>Creates a generator from a 64-bit seed (0 is remapped).</summary>
    public SeededRng(ulong seed) =>
        _state = seed == 0 ? 0x9E3779B97F4A7C15UL : seed;

    /// <summary>Creates a generator from a signed seed.</summary>
    public SeededRng(long seed) : this(unchecked((ulong)seed))
    {
    }

    /// <summary>Next raw 64-bit value.</summary>
    public ulong NextUInt64()
    {
        unchecked
        {
            _state += 0x9E3779B97F4A7C15UL;
            var z = _state;
            z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
            z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
            return z ^ (z >> 31);
        }
    }

    /// <summary>Uniform float in [0, 1).</summary>
    public float NextSingle() =>
        (NextUInt64() >> 40) * (1f / (1 << 24));

    /// <summary>Uniform float in [<paramref name="minInclusive"/>, <paramref name="maxExclusive"/>).</summary>
    public float NextSingle(float minInclusive, float maxExclusive) =>
        minInclusive + NextSingle() * (maxExclusive - minInclusive);

    /// <summary>Uniform int in [<paramref name="minInclusive"/>, <paramref name="maxExclusive"/>).</summary>
    public int NextInt(int minInclusive, int maxExclusive)
    {
        if (maxExclusive <= minInclusive)
            return minInclusive;
        var span = (uint)(maxExclusive - minInclusive);
        return minInclusive + (int)(NextUInt64() % span);
    }

    /// <summary>Derives a child seed for a stable sub-stream (chunk coords, feature ids).</summary>
    public static ulong Mix(ulong seed, long a, long b = 0) =>
        unchecked
        {
            var x = seed ^ ((ulong)a * 0xD6E8FEB86659FD93UL) ^ ((ulong)b * 0xC2B2AE3D27D4EB4FUL);
            x ^= x >> 33;
            x *= 0xFF51AFD7ED558CCDUL;
            x ^= x >> 33;
            return x == 0 ? 1UL : x;
        }
}
