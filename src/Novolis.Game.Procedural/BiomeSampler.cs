namespace Novolis.Game.Procedural;

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
