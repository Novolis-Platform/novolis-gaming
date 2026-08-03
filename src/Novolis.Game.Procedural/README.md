<!-- novolis-pkg-brand:start -->
<p align="center">
  <a href="https://github.com/Novolis-Platform/novolis-gaming">
    <img src="https://raw.githubusercontent.com/Novolis-Platform/.github/main/brand/logo-icon.svg" width="72" alt="Novolis"/>
  </a>
</p>
<!-- novolis-pkg-brand:end -->

# Novolis.Game.Procedural

Seeded procedural tools for game authoring: noise terrain, real-time infinite chunks, infinite-runner tracks, weighted spawns, difficulty ramps.

**No** Simulation / Physics / Rendering references — BCL only. Apps compose with the stack at the product layer.

## Install

```bash
dotnet add package Novolis.Game.Procedural
```

## Quick start

```csharp
using Novolis.Game.Procedural;

var seed = 1001UL;
var terrain = new NoiseHeightfield(seed, frequency: 0.025f, amplitude: 18f);
var y = terrain.SampleHeight(120f, 40f);

// Real-time infinite chunks around the player
var stream = new InfiniteChunkStream(chunkSize: 32f, radius: 2);
stream.ChunkLoaded += c => { /* build mesh / colliders for chunk c */ };
stream.ChunkUnloaded += c => { /* dispose */ };
stream.Update(playerX, playerZ); // each tick

// Infinite runner strip along +X
var track = new InfiniteTrackGenerator(seed, segmentLength: 48f, lanes: 3);
var seg = track.Segment(track.IndexAt(playerX));
```

## Contents

| Type | Role |
|------|------|
| `SeededRng` | Deterministic PRNG + `Mix` for sub-streams |
| `Noise` | Value noise + FBM |
| `NoiseHeightfield` / `IHeightSampler` | Terrain heights |
| `InfiniteChunkStream` | Sliding chunk window (load/unload events) |
| `InfiniteTrackGenerator` | Runner segments (platforms, gaps, hazards…) |
| `DifficultyRamp` | Distance → intensity |
| `WeightedTable<T>` | Spawn / loot tables |
| `BiomeSampler` | Coarse biome from noise |

## Policy

Stays inside [gaming-layer-policy](https://github.com/Novolis-Platform/novolis-governance/blob/main/docs/gaming-layer-policy.md): authoring helpers only, no sim/render wiring.

