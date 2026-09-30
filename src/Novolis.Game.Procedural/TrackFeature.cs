namespace Novolis.Game.Procedural;

/// <summary>A local feature on a track segment (X along the run axis).</summary>
public readonly record struct TrackFeature(
    TrackFeatureKind Kind,
    float LocalX,
    float Width,
    float Height,
    int Lane);
