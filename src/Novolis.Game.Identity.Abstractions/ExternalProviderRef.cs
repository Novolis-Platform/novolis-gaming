namespace Novolis.Game.Identity.Abstractions;

/// <summary>Named external identity provider (Steam, Xbox, custom OIDC, etc.).</summary>
/// <param name="Value">Provider key.</param>
public readonly record struct ExternalProviderRef(string Value)
{
    /// <inheritdoc />
    public override string ToString() => Value;
}
