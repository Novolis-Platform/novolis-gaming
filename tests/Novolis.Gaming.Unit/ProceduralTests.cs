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
}
