<!-- novolis-pkg-brand:start -->
[![Novolis](https://raw.githubusercontent.com/Novolis-Platform/.github/main/brand/logo-icon.png)](https://novolis-platform.github.io/.github/novolis-gaming/)

[Novolis](https://github.com/Novolis-Platform) · [Docs](https://novolis-platform.github.io/.github/novolis-gaming/) · [Source](https://github.com/Novolis-Platform/novolis-gaming)
<!-- novolis-pkg-brand:end -->

# Novolis.Game.Scenes

Game-facing scene helpers. Sits on Rendering (peer of Simulation). Window and UI hosts stay in Silk, Avalonia, MAUI, or Web — this package does not take those.

A game lists `Novolis.Game.Scenes` plus one host, not Math topology / Presentation / Appearance islands.

## Install

```bash
dotnet add package Novolis.Game.Scenes
```

## Quick start

```csharp
using Novolis.Game.Scenes;
using Novolis.Rendering.Planar;

var scene = new PlanarScene();
DenseGridPlatforms.AddSolidCells(scene, grid, cellSize: 1f, fillColor);
camera.ApplyTo(scene.Camera, width, height);
```

The Silk (or Avalonia) host tessellates and submits. Gaming does not take Silk.

## API

| Type | Role |
|------|------|
| `OrthoPanCamera` | Top-down pan + zoom for a render viewport |
| `ViewportFollow` | Look-ahead follow + trauma shake |
| `PlanarMove` | Axis buttons → unit XZ move |
| `ViewportAim` | Screen cursor → XZ aim delta |
| `FixedStepClock` | Fixed-tick accumulator |
| `DenseGridPlatforms` | Occupancy grid → platforms, colliders, square markers |

## Related

| Package | When to use |
|---------|-------------|
| `Novolis.Silk` | GLFW window host — compose at the game |
| `Novolis.Game.MenuFlows` | Host-agnostic screen stack |
