# Novolis.Game.Identity.Abstractions

Pseudonymous identity primitives for games: opaque `PlayerRef`, session/device refs, and linker contracts without PII in the platform surface.

## Install

```bash
dotnet add package Novolis.Game.Identity.Abstractions
```

**Prerequisites:** [.NET 10 SDK](https://dotnet.microsoft.com/download) (`net10.0`).

## Quick start

```csharp
var player = PlayerRef.New();
directory.SetDisplayName(player, "Guest-42"); // display names live in your app layer
```

## Related packages

| Package | When to use |
|---------|-------------|
| `Novolis.Game.Identity` | In-memory directory and linker implementations |
| `Novolis.Game.Identity.AspNetCore` | Map `ClaimsPrincipal` to `PlayerRef` |

## More documentation

- [Getting started](https://github.com/Novolis-Platform/novolis-gaming/blob/main/docs/getting-started.md)
- [Design](https://github.com/Novolis-Platform/novolis-gaming/blob/main/docs/design.md)

## Support

Pre-release (`2026.1.*`). GDPR-sensitive data stays in product repos, not in this package.
