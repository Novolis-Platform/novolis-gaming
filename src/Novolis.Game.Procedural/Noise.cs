namespace Novolis.Game.Procedural;

/// <summary>Value noise and fractal Brownian motion (BCL-only, deterministic).</summary>
public static class Noise
{
    /// <summary>1D value noise in roughly [-1, 1].</summary>
    public static float Value1D(float x, ulong seed)
    {
        var x0 = (int)MathF.Floor(x);
        var t = x - x0;
        t = Smooth(t);
        var a = HashToFloat(seed, x0);
        var b = HashToFloat(seed, x0 + 1);
        return Lerp(a, b, t);
    }

    /// <summary>2D value noise in roughly [-1, 1].</summary>
    public static float Value2D(float x, float z, ulong seed)
    {
        var x0 = (int)MathF.Floor(x);
        var z0 = (int)MathF.Floor(z);
        var tx = Smooth(x - x0);
        var tz = Smooth(z - z0);
        var n00 = HashToFloat(seed, x0, z0);
        var n10 = HashToFloat(seed, x0 + 1, z0);
        var n01 = HashToFloat(seed, x0, z0 + 1);
        var n11 = HashToFloat(seed, x0 + 1, z0 + 1);
        var nx0 = Lerp(n00, n10, tx);
        var nx1 = Lerp(n01, n11, tx);
        return Lerp(nx0, nx1, tz);
    }

    /// <summary>2D FBM layered value noise. Amplitude ≈ sum of <paramref name="persistence"/>^i.</summary>
    public static float Fbm2D(
        float x,
        float z,
        ulong seed,
        int octaves = 4,
        float lacuna = 2f,
        float persistence = 0.5f)
    {
        octaves = Math.Clamp(octaves, 1, 12);
        var sum = 0f;
        var amp = 1f;
        var freq = 1f;
        var norm = 0f;
        for (var i = 0; i < octaves; i++)
        {
            sum += Value2D(x * freq, z * freq, SeededRng.Mix(seed, i)) * amp;
            norm += amp;
            amp *= persistence;
            freq *= lacuna;
        }

        return norm > 1e-6f ? sum / norm : 0f;
    }

    static float Smooth(float t) => t * t * (3f - 2f * t);

    static float Lerp(float a, float b, float t) => a + (b - a) * t;

    static float HashToFloat(ulong seed, int x, int z = 0)
    {
        var h = SeededRng.Mix(seed, x, z);
        // Map to [-1, 1]
        return ((h & 0xFFFFFF) / (float)0xFFFFFF) * 2f - 1f;
    }
}
