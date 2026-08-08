using Novolis.Game.Procedural;

namespace Novolis.Gaming.Unit.Procedural;

public sealed class ProceduralTests
{
    [Test]
    public async Task SeededRng_Is_Deterministic()
    {
        var a = new SeededRng(42);
        var b = new SeededRng(42);
        for (var i = 0; i < 32; i++)
            await Assert.That(a.NextUInt64()).IsEqualTo(b.NextUInt64());
    }

    [Test]
    public async Task Noise_Fbm_Is_Finite()
    {
        var v = Noise.Fbm2D(3.2f, -1.1f, seed: 7, octaves: 5);
        await Assert.That(float.IsFinite(v)).IsTrue();
        await Assert.That(MathF.Abs(v)).IsLessThanOrEqualTo(1.01f);
    }

    [Test]
    public async Task InfiniteChunkStream_Loads_And_Unloads()
    {
        var stream = new InfiniteChunkStream(chunkSize: 10f, radius: 1);
        var loaded = new List<ChunkCoord>();
        var unloaded = new List<ChunkCoord>();
        stream.ChunkLoaded += c => loaded.Add(c);
        stream.ChunkUnloaded += c => unloaded.Add(c);

        stream.Update(5f, 5f);
        await Assert.That(stream.Loaded.Count).IsEqualTo(9);

        stream.Update(100f, 5f);
        await Assert.That(unloaded.Count).IsGreaterThan(0);
        await Assert.That(stream.Loaded.Any(c => c.X >= 9)).IsTrue();
    }

    [Test]
    public async Task InfiniteTrack_Is_Stable_Per_Index()
    {
        var gen = new InfiniteTrackGenerator(99, segmentLength: 40f);
        var a = gen.Segment(3);
        var b = gen.Segment(3);
        await Assert.That(a.StartX).IsEqualTo(b.StartX);
        await Assert.That(a.Features.Count).IsEqualTo(b.Features.Count);
        await Assert.That(a.Features[0]).IsEqualTo(b.Features[0]);
    }

    [Test]
    public async Task WeightedTable_Respects_Weights()
    {
        var table = new WeightedTable<string>().Add("a", 1).Add("b", 1000);
        var rng = new SeededRng(1);
        var hits = 0;
        for (var i = 0; i < 50; i++)
        {
            if (table.Pick(ref rng) == "b")
                hits++;
        }

        await Assert.That(hits).IsGreaterThan(40);
    }

    [Test]
    public async Task WeightedTable_TryPick_Fails_When_Empty()
    {
        var table = new WeightedTable<string>();
        var rng = new SeededRng(1);
        await Assert.That(table.TryPick(ref rng, out _)).IsFalse();
    }

    [Test]
    public async Task WeightedTable_TryPick_Succeeds_When_Populated()
    {
        var table = new WeightedTable<string>().Add("only", 1);
        var rng = new SeededRng(7);
        await Assert.That(table.TryPick(ref rng, out var item)).IsTrue();
        await Assert.That(item).IsEqualTo("only");
    }

    [Test]
    public async Task WeightedTable_Pick_Throws_When_Empty()
    {
        var table = new WeightedTable<string>();
        var rng = new SeededRng(1);
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
        {
            _ = table.Pick(ref rng);
            return Task.CompletedTask;
        });
    }

    [Test]
    public async Task NoiseHeightfield_Samples_Finite_Heights()
    {
        var field = new NoiseHeightfield(seed: 12, baseHeight: 5f);
        var height = field.SampleHeight(10f, -3f);
        await Assert.That(float.IsFinite(height)).IsTrue();
    }

    [Test]
    public async Task BiomeSampler_Returns_Stable_Biome()
    {
        var first = BiomeSampler.Sample(100f, 200f, worldSeed: 7);
        var second = BiomeSampler.Sample(100f, 200f, worldSeed: 7);
        await Assert.That(first).IsEqualTo(second);
    }

    [Test]
    public async Task InfiniteTrack_IndexAt_And_High_Difficulty()
    {
        var ramp = new DifficultyRamp(startDistance: 0f, fullAtDistance: 100f, ease: 1f);
        var gen = new InfiniteTrackGenerator(seed: 123, segmentLength: 20f, difficulty: ramp);
        await Assert.That(gen.SegmentLength).IsEqualTo(20f);
        await Assert.That(gen.IndexAt(45f)).IsEqualTo(2);

        var segment = gen.Segment(10);
        await Assert.That(segment.Features.Count).IsGreaterThan(0);
        await Assert.That(segment.Features.Any(f => f.Kind == TrackFeatureKind.Gate)).IsTrue();
    }

    [Test]
    public async Task SeededRng_NextInt_Handles_Equal_Bounds()
    {
        var rng = new SeededRng(5L);
        await Assert.That(rng.NextInt(3, 3)).IsEqualTo(3);
    }

    [Test]
    public async Task WeightedTable_Ignores_NonPositive_Weights()
    {
        var table = new WeightedTable<string>().Add("skip", 0).Add("only", 1);
        await Assert.That(table.Count).IsEqualTo(1);
        var rng = new SeededRng(1);
        await Assert.That(table.Pick(ref rng)).IsEqualTo("only");
    }

    [Test]
    public async Task WeightedTable_Fallthrough_Returns_Last_Entry()
    {
        // Many equal weights exercise the accumulator loop; last entry remains reachable.
        var table = new WeightedTable<string>()
            .Add("a", 1f)
            .Add("b", 1f)
            .Add("c", 1f);
        var counts = new Dictionary<string, int>(StringComparer.Ordinal);
        for (ulong seed = 1; seed <= 300; seed++)
        {
            var rng = new SeededRng(seed);
            var item = table.Pick(ref rng);
            counts[item] = counts.GetValueOrDefault(item) + 1;
        }

        await Assert.That(counts.ContainsKey("a")).IsTrue();
        await Assert.That(counts.ContainsKey("c")).IsTrue();
    }

    [Test]
    public async Task BiomeSampler_Covers_Multiple_Bands()
    {
        var biomes = new HashSet<BiomeKind>();
        for (var i = 0; i < 400; i++)
            biomes.Add(BiomeSampler.Sample(i * 17.3f, i * 11.7f, worldSeed: 42));
        await Assert.That(biomes.Count).IsGreaterThanOrEqualTo(3);
    }

    [Test]
    public async Task DifficultyRamp_Returns_Zero_Before_Start()
    {
        var ramp = new DifficultyRamp(startDistance: 100f, fullAtDistance: 500f);
        await Assert.That(ramp.Evaluate(50f)).IsEqualTo(0f);
    }

    [Test]
    public async Task Noise_Value1D_Is_Finite()
    {
        var value = Noise.Value1D(1.5f, seed: 99);
        await Assert.That(float.IsFinite(value)).IsTrue();
    }
}
