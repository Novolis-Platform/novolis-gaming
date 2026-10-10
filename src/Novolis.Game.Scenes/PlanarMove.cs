using System.Numerics;
using Novolis.Math.Geometry;

namespace Novolis.Game.Scenes;

/// <summary>WASD-style planar move without host key types.</summary>
public static class PlanarMove
{
    /// <summary>XZ vector from four axis buttons, clamped to unit length.</summary>
    public static Vector3 FromAxes(bool left, bool right, bool backward, bool forward)
    {
        var x = 0f;
        var z = 0f;
        if (right)
        {
            x += 1f;
        }

        if (left)
        {
            x -= 1f;
        }

        if (forward)
        {
            z += 1f;
        }

        if (backward)
        {
            z -= 1f;
        }

        var move = new Vector3(x, 0f, z);
        return move.LengthSquaredXz() > 1f ? move.NormalizeXz() : move;
    }
}
