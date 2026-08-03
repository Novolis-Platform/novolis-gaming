<!-- novolis-pkg-brand:start -->
<p align="center">
  <a href="https://github.com/Novolis-Platform/novolis-gaming">
    <img src="https://raw.githubusercontent.com/Novolis-Platform/.github/main/brand/logo-icon.svg" width="72" alt="Novolis"/>
  </a>
</p>
<!-- novolis-pkg-brand:end -->

# Novolis.Game.Multiplayer.AspNetCore

SignalR hub base and lobby DTO mapping for ASP.NET game servers.

## Install

```bash
dotnet add package Novolis.Game.Multiplayer.AspNetCore
```

## Quick start

```csharp
using Novolis.Game.Multiplayer.AspNetCore;

public sealed class MyLobbyHub : GameLobbyHubBase
{
    protected override ILobbyState GetLobby(LobbyId id) => /* your store */;
}
```

Clients call `JoinLobbyAsync(lobbyId)` and `SetReadyAsync(lobbyId, isReady)`. The hub resolves the caller's `PlayerRef` from claims.

## API

| Type | Role |
|------|------|
| `GameLobbyHubBase` | `JoinLobbyAsync`, `SetReadyAsync`; override `GetLobby`; protected `TryGetCallerPlayer` |
| `LobbyDto` | Wire DTO: `LobbyId`, `Players` |
| `LobbyPlayerDto` | `PlayerRef`, `IsReady` |
| `LobbyMapping` | `ToDto(lobby)`, `TryParsePlayerRef(value, out player)` |

## Related

| Package | When to use |
|---------|-------------|
| `Novolis.Game.Multiplayer.Abstractions` | Lobby model |
| `Novolis.Game.Identity.AspNetCore` | Player claims |

## More documentation

- [Design](https://github.com/Novolis-Platform/novolis-gaming/blob/main/docs/design.md)

