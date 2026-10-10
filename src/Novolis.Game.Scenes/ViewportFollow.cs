using System.Numerics;
using Novolis.Math.Geometry;
using Novolis.Rendering.Planar;

namespace Novolis.Game.Scenes;

/// <summary>Look-ahead follow plus decaying trauma on a planar viewport.</summary>
public sealed class ViewportFollow
{
    /// <summary>How far ahead of facing to bias the view (world meters).</summary>
    public float LookAhead { get; set; } = 2.3f;

    /// <summary>Exponential follow rate.</summary>
    public float FollowHz { get; set; } = 10f;

    /// <summary>Current shake amplitude (decays in <see cref="Tick"/>).</summary>
    public float Shake { get; set; }

    /// <summary>Shake decay per second.</summary>
    public float ShakeDecay { get; set; } = 4.2f;

    /// <summary>World meters of shake at amplitude 1.</summary>
    public float ShakeMeters { get; set; } = 0.4f;

    /// <summary>Adds trauma, capped.</summary>
    public void AddShake(float amount) => Shake = MathF.Min(1.4f, Shake + amount);

    /// <summary>Moves the viewport toward the subject plus aim, with optional shake.</summary>
    public void Tick(PlanarViewport camera, Vector3 subject, Vector3 look, float dt)
    {
        ArgumentNullException.ThrowIfNull(camera);
        var dir = look.LengthSquaredXz() > 0.01f ? look.NormalizeXz() : Vector3.Zero;
        var target = subject + dir * LookAhead;
        if (Shake > 0.01f)
        {
            target += new Vector3(
                (Random.Shared.NextSingle() - 0.5f) * ShakeMeters * Shake,
                0f,
                (Random.Shared.NextSingle() - 0.5f) * ShakeMeters * Shake);
        }

        var t = 1f - MathF.Exp(-FollowHz * dt);
        camera.Position = Vector3.Lerp(camera.Position, target, t);
        Shake = MathF.Max(0f, Shake - dt * ShakeDecay);
    }
}
