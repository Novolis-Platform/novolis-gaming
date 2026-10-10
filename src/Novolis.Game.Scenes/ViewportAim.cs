using System.Numerics;
using Novolis.Math.Geometry;
using Novolis.Rendering.Planar;

namespace Novolis.Game.Scenes;

/// <summary>Mouse / pointer aim on the XZ plane.</summary>
public static class ViewportAim
{
    /// <summary>World-space delta from <paramref name="from"/> to the cursor (Y = 0).</summary>
    public static Vector3 DeltaXz(PlanarViewport camera, float screenX, float screenY, Vector3 from)
    {
        ArgumentNullException.ThrowIfNull(camera);
        var world = camera.ScreenToWorld(screenX, screenY);
        return new Vector3(world.X - from.X, 0f, world.Z - from.Z);
    }
}
