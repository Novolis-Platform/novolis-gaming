using Novolis.Game.Identity.Abstractions;

namespace Novolis.Game.Identity;

/// <summary>In-memory external identity links for tests and offline games.</summary>
public sealed class InMemoryExternalIdentityLinker : IExternalIdentityLinker
{
    private readonly Dictionary<(string Provider, string Hash), Guid> _links = new();

    /// <inheritdoc />
    public bool TryLink(
        ExternalProviderRef provider,
        ExternalSubjectHash subjectHash,
        out PlayerRef player)
    {
        lock (_links)
        {
            if (_links.TryGetValue((provider.Value, subjectHash.Value), out var id))
            {
                player = new PlayerRef(id);
                return true;
            }

            player = default;
            return false;
        }
    }

    /// <inheritdoc />
    public void Link(
        ExternalProviderRef provider,
        ExternalSubjectHash subjectHash,
        PlayerRef player)
    {
        lock (_links)
        {
            _links[(provider.Value, subjectHash.Value)] = player.Value;
        }
    }
}
