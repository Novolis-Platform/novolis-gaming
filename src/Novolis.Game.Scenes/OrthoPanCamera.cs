using System.Numerics;
using Novolis.Math.Geometry;
using Novolis.Rendering.Planar;

namespace Novolis.Game.Scenes;

/// <summary>Top-down orthographic pan + zoom for a render viewport.</summary>
public sealed class OrthoPanCamera
{
    /// <summary>World-space view center on the XZ plane.</summary>
    public Vector3 Center { get; set; }

    /// <summary>World units represented by one screen pixel.</summary>
    public float WorldUnitsPerPixel { get; set; } = 1f / 24f;

    /// <summary>Closest zoom (smallest world units per pixel).</summary>
    public float MinWorldUnitsPerPixel { get; set; } = 1f / 48f;

    /// <summary>Farthest zoom (largest world units per pixel).</summary>
    public float MaxWorldUnitsPerPixel { get; set; } = 1f / 12f;

    /// <summary>Copies pan/zoom into the viewport.</summary>
    public void ApplyTo(PlanarViewport camera, int viewportWidth, int viewportHeight)
    {
        camera.Position = Center;
        camera.WorldUnitsPerPixel = WorldUnitsPerPixel;
        camera.ViewportWidth = viewportWidth;
        camera.ViewportHeight = viewportHeight;
    }

    /// <summary>Shifts the view on the XZ plane.</summary>
    public void Pan(Vector3 delta)
    {
        Center = new Vector3(Center.X + delta.X, 0f, Center.Z + delta.Z);
    }

    /// <summary>Scales world units per pixel, clamped to min/max.</summary>
    public void Zoom(float wheelSteps)
    {
        if (MathF.Abs(wheelSteps) < 1e-6f)
        {
            return;
        }

        var factor = MathF.Pow(1.08f, -wheelSteps);
        WorldUnitsPerPixel = System.Math.Clamp(WorldUnitsPerPixel * factor, MinWorldUnitsPerPixel, MaxWorldUnitsPerPixel);
    }

    /// <summary>World-space view center (Y = 0).</summary>
    public Vector3 ViewCenterWorld() => Center;

    /// <summary>Maps a screen pixel through the viewport.</summary>
    public Vector3 ScreenToWorld(PlanarViewport camera, float screenX, float screenY) =>
        camera.ScreenToWorld(screenX, screenY);
}
