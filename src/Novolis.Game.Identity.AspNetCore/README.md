# Novolis.Game.Identity.AspNetCore

Bridge ASP.NET `ClaimsPrincipal` to `PlayerRef`. Does not configure Identity Server or persist users.

## Install

```bash
dotnet add package Novolis.Game.Identity.AspNetCore
```

## Quick start

```csharp
if (User.TryGetPlayerRef(out var player)) { /* game logic */ }
```

## Related packages

| Package | When to use |
|---------|-------------|
| `Novolis.Game.Identity.Abstractions` | Ref types |
| `Novolis.Game.Multiplayer.AspNetCore` | SignalR lobbies |

## More documentation

- [Design](https://github.com/Novolis-Platform/novolis-gaming/blob/main/docs/design.md)
