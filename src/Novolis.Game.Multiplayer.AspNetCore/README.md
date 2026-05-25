# Novolis.Game.Multiplayer.AspNetCore

SignalR hub base and lobby DTO mapping for ASP.NET game servers.

## Install

```bash
dotnet add package Novolis.Game.Multiplayer.AspNetCore
```

## Quick start

```csharp
public sealed class MyLobbyHub : GameLobbyHubBase { /* supply ILobbyState */ }
```

## Related packages

| Package | When to use |
|---------|-------------|
| `Novolis.Game.Multiplayer.Abstractions` | Lobby model |
| `Novolis.Game.Identity.AspNetCore` | Player claims |

## More documentation

- [Design](https://github.com/Novolis-Platform/novolis-gaming/blob/main/docs/design.md)
