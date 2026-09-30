namespace Novolis.Game.Procedural;

/// <summary>
/// Deterministic infinite-runner track generator. Call <see cref="Segment"/> for any index —
/// segments are independent given the world seed (real-time streaming friendly).
/// </summary>
public sealed class InfiniteTrackGenerator
{
    readonly ulong _seed;
    readonly float _segmentLength;
    readonly int _lanes;
    readonly DifficultyRamp _difficulty;

    /// <param name="seed">World seed.</param>
    /// <param name="segmentLength">Length of each segment along +X.</param>
    /// <param name="lanes">Lane count for features (0..lanes-1).</param>
    /// <param name="difficulty">Optional distance → intensity curve.</param>
    public InfiniteTrackGenerator(
        ulong seed,
        float segmentLength = 40f,
        int lanes = 3,
        DifficultyRamp? difficulty = null)
    {
        _seed = seed == 0 ? 1UL : seed;
        _segmentLength = Math.Max(4f, segmentLength);
        _lanes = Math.Clamp(lanes, 1, 8);
        _difficulty = difficulty ?? DifficultyRamp.Default;
    }

    /// <summary>Segment length along +X.</summary>
    public float SegmentLength => _segmentLength;

    /// <summary>Maps world X to segment index.</summary>
    public int IndexAt(float worldX) =>
        (int)MathF.Floor(worldX / _segmentLength);

    /// <summary>Builds (or rebuilds) the segment at <paramref name="index"/>.</summary>
    public TrackSegment Segment(int index)
    {
        var start = index * _segmentLength;
        var intensity = _difficulty.Evaluate(start);
        var rng = new SeededRng(SeededRng.Mix(_seed, index, 17));
        var features = new List<TrackFeature>(8);

        // Always a base platform stretch with occasional gaps.
        var cursor = 2f;
        while (cursor < _segmentLength - 2f)
        {
            var gapChance = 0.12f + intensity * 0.35f;
            if (rng.NextSingle() < gapChance && cursor > 4f)
            {
                var gapW = rng.NextSingle(2.5f, 4.5f + intensity * 2f);
                features.Add(new TrackFeature(TrackFeatureKind.Gap, cursor, gapW, 0f, 0));
                cursor += gapW + rng.NextSingle(1f, 3f);
                continue;
            }

            var platW = rng.NextSingle(4f, 10f);
            features.Add(new TrackFeature(TrackFeatureKind.Platform, cursor, platW, 1f, 0));
            cursor += platW;

            if (rng.NextSingle() < 0.35f + intensity * 0.3f)
            {
                var lane = rng.NextInt(0, _lanes);
                features.Add(new TrackFeature(
                    TrackFeatureKind.Hazard,
                    cursor - platW * 0.5f,
                    rng.NextSingle(1.2f, 2.4f),
                    1.5f,
                    lane));
            }

            if (rng.NextSingle() < 0.45f)
            {
                features.Add(new TrackFeature(
                    TrackFeatureKind.Collectible,
                    cursor - platW * rng.NextSingle(0.2f, 0.8f),
                    0.8f,
                    1f,
                    rng.NextInt(0, _lanes)));
            }
        }

        if (intensity > 0.55f && rng.NextSingle() < intensity)
        {
            features.Add(new TrackFeature(
                TrackFeatureKind.Gate,
                _segmentLength * 0.75f,
                2f,
                3f,
                rng.NextInt(0, _lanes)));
        }

        return new TrackSegment(index, start, _segmentLength, features);
    }
}
