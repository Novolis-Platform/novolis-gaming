namespace Novolis.Game.Procedural;

/// <summary>One finite strip of an infinite track along +X.</summary>
public readonly record struct TrackSegment(
    int Index,
    float StartX,
    float Length,
    IReadOnlyList<TrackFeature> Features);
