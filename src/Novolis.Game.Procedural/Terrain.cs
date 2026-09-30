namespace Novolis.Game.Procedural;

/// <summary>Samples a height at world XZ (Y-up, planar XZ).</summary>
public interface IHeightSampler
{
    /// <summary>World-space height at (<paramref name="x"/>, <paramref name="z"/>).</summary>
    float SampleHeight(float x, float z);
}
