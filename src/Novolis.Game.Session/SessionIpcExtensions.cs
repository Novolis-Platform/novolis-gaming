using Novolis.Transports.LocalIpc;

namespace Novolis.Game.Session;

public static class SessionIpcExtensions
{
    public static ValueTask SendMessageAsync<T>(
        this ILocalIpcConnection connection,
        long sequence,
        string kind,
        string name,
        T payload,
        CancellationToken cancellationToken = default) =>
        connection.SendAsync(
            new LocalIpcFrame(sequence, kind, name, SessionProtocolCodec.Serialize(payload)),
            cancellationToken);
}
