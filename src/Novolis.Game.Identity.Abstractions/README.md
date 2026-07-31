# Novolis.Game.Identity.Abstractions

Pseudonymous identity primitives for games: opaque `PlayerRef`, session/device refs, and linker contracts without PII in the platform surface.

## Install

```bash
dotnet add package Novolis.Game.Identity.Abstractions
```

**Prerequisites:** [.NET 10 SDK](https://dotnet.microsoft.com/download) (`net10.0`).

## Quick start

```csharp
using Novolis.Game.Identity.Abstractions;

var player = PlayerRef.New();
directory.SetDisplayName(player, "Guest-42"); // display names live in your app layer
```

## API

| Type | Role |
|------|------|
| `PlayerRef` | `New()`, `FromGuid`, opaque `Value` |
| `SessionRef` | `New()`, opaque session id |
| `DeviceRef` | `New()`, opaque device id |
| `ExternalProviderRef` | Provider key string |
| `ExternalSubjectHash` | Hashed external subject |
| `IPlayerDirectory` | `TryGetDisplayName`, `SetDisplayName`, `Remove` |
| `IExternalIdentityLinker` | `TryLink`, `Link` provider subjects to `PlayerRef` |

## Related

| Package | When to use |
|---------|-------------|
| `Novolis.Game.Identity` | In-memory directory and linker implementations |
| `Novolis.Game.Identity.AspNetCore` | Map `ClaimsPrincipal` to `PlayerRef` |
| `Novolis.Game.Multiplayer.Abstractions` | Lobby slots keyed by `PlayerRef` |

## More documentation

- [Design](https://github.com/Novolis-Platform/novolis-gaming/blob/main/docs/design.md)
