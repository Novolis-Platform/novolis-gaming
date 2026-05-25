namespace Novolis.Game.Identity.Abstractions;

/// <summary>Implemented by the game host to map external logins to <see cref="PlayerRef"/>.</summary>
public interface IExternalIdentityLinker
{
    /// <summary>Resolves an existing link, if any.</summary>
    bool TryLink(
        ExternalProviderRef provider,
        ExternalSubjectHash subjectHash,
        out PlayerRef player);

    /// <summary>Records a new provider subject to player mapping.</summary>
    void Link(
        ExternalProviderRef provider,
        ExternalSubjectHash subjectHash,
        PlayerRef player);
}
