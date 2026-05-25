# Novolis.Game.Multiplayer.Abstractions

Lobby identifiers and in-memory lobby state using `PlayerRef` slots.

## Install

```bash
dotnet add package Novolis.Game.Multiplayer.Abstractions
```

## Quick start

```csharp
var lobby = new InMemoryLobbyState();
lobby.TryAddPlayer(new LobbyPlayerSlot(PlayerRef.New(), isReady: false));
```

## Related packages

| Package | When to use |
|---------|-------------|
| `Novolis.Game.Multiplayer.AspNetCore` | SignalR hub bases |

## More documentation

- [Design](https://github.com/Novolis-Platform/novolis-gaming/blob/main/docs/design.md)
