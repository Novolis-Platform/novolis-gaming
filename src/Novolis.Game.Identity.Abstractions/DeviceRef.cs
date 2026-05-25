namespace Novolis.Game.Identity.Abstractions;

/// <summary>Client installation or device binding (opaque).</summary>
/// <param name="Value">Underlying GUID.</param>
public readonly record struct DeviceRef(Guid Value)
{
    /// <summary>Creates a new device ref.</summary>
    public static DeviceRef New() => new(Guid.NewGuid());

    /// <inheritdoc />
    public override string ToString() => Value.ToString("D");
}
