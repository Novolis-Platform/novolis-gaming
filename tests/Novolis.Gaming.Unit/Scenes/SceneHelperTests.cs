using System.Numerics;
using Novolis.Game.Scenes;
using Novolis.Math.Arrays;
using Novolis.Math.Geometry;
using Novolis.Rendering.Planar;

namespace Novolis.Gaming.Unit.Scenes;

public sealed class SceneHelperTests
{
    [Test]
    public async Task OrthoPan_KeepsPlanarYAndClampsZoom()
    {
        var camera = new OrthoPanCamera
        {
            Center = new System.Numerics.Vector3(1f, 4f, 2f),
            WorldUnitsPerPixel = 1f / 24f,
            MinWorldUnitsPerPixel = 1f / 48f,
            MaxWorldUnitsPerPixel = 1f / 12f,
        };

        camera.Pan(new System.Numerics.Vector3(3f, 9f, -1f));
        await Assert.That(camera.Center.X).IsEqualTo(4f);
        await Assert.That(camera.Center.Y).IsEqualTo(0f);
        await Assert.That(camera.Center.Z).IsEqualTo(1f);

        camera.Zoom(0f);
        await Assert.That(camera.WorldUnitsPerPixel).IsEqualTo(1f / 24f);

        camera.Zoom(1000f);
        await Assert.That(camera.WorldUnitsPerPixel).IsEqualTo(camera.MinWorldUnitsPerPixel);
    }

    [Test]
    public async Task DenseGrid_AddsOnePlatformPerSolidCell()
    {
        var grid = new DenseGrid<byte>(2, 2, 1);
        grid[0, 0, 0] = 1;
        grid[1, 1, 0] = 1;
        var scene = new PlanarScene();

        DenseGridPlatforms.AddSolidCells(scene, grid, 2f, new Rgba32(10, 20, 30));

        await Assert.That(scene.StaticPolygons.Count).IsEqualTo(2);
    }

    [Test]
    public async Task PlanarMove_NormalizesDiagonal()
    {
        var move = PlanarMove.FromAxes(left: true, right: false, backward: false, forward: true);
        await Assert.That(move.LengthXz()).IsEqualTo(1f).Within(1e-5f);
        await Assert.That(move.X).IsLessThan(0f);
        await Assert.That(move.Z).IsGreaterThan(0f);
    }

    [Test]
    public async Task FixedStepClock_EmitsWholeSteps()
    {
        var clock = new FixedStepClock(0.1f, 0.5f);
        await Assert.That(clock.Consume(0.25f)).IsEqualTo(2);
        await Assert.That(clock.Consume(0.04f)).IsEqualTo(0);
        await Assert.That(clock.Consume(0.06f)).IsEqualTo(1);
    }

    [Test]
    public async Task ViewportAim_UsesScreenToWorld()
    {
        var camera = new PlanarViewport
        {
            Position = Vector3.Zero,
            WorldUnitsPerPixel = 1f,
            ViewportWidth = 100,
            ViewportHeight = 100,
        };
        var delta = ViewportAim.DeltaXz(camera, 75f, 25f, Vector3.Zero);
        await Assert.That(delta.X).IsGreaterThan(0f);
        await Assert.That(delta.Z).IsGreaterThan(0f);
    }
}
