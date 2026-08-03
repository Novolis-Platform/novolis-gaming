<!-- novolis-pkg-brand:start -->
<p align="center">
  <a href="https://github.com/Novolis-Platform/novolis-gaming">
    <img src="https://raw.githubusercontent.com/Novolis-Platform/.github/main/brand/logo-icon.svg" width="72" alt="Novolis"/>
  </a>
</p>
<!-- novolis-pkg-brand:end -->

# Novolis.Game.Multiplayer.Abstractions

Lobby identifiers and in-memory lobby state using `PlayerRef` slots.

## Install

```bash
dotnet add package Novolis.Game.Multiplayer.Abstractions
```

## Quick start

```csharp
using Novolis.Game.Identity.Abstractions;
using Novolis.Game.Multiplayer.Abstractions;

var lobby = new InMemoryLobbyState();
lobby.TryAddPlayer(new LobbyPlayerSlot(PlayerRef.New(), isReady: false));
lobby.TrySetReady(player, isReady: true);
```

## API

| Type | Role |
|------|------|
| `LobbyId` | `New()`, opaque `Value` |
| `LobbyPlayerSlot` | `Player`, `IsReady` |
| `ILobbyState` | `Id`, `Players`, `TryAddPlayer`, `TryRemovePlayer`, `TrySetReady` |
| `InMemoryLobbyState` | In-process `ILobbyState` |

## Related

| Package | When to use |
|---------|-------------|
| `Novolis.Game.Multiplayer.AspNetCore` | SignalR hub bases |
| `Novolis.Game.Identity.Abstractions` | `PlayerRef` for slots |

## More documentation

- [Design](https://github.com/Novolis-Platform/novolis-gaming/blob/main/docs/design.md)

