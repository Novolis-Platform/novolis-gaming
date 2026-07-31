# Novolis.Game.Identity.AspNetCore

Bridge ASP.NET `ClaimsPrincipal` to `PlayerRef`. Does not configure Identity Server or persist users.

## Install

```bash
dotnet add package Novolis.Game.Identity.AspNetCore
```

## Quick start

```csharp
using Novolis.Game.Identity.AspNetCore;

if (User.TryGetPlayerRef(out var player)) { /* game logic */ }

// Issue a claim when signing in:
var claim = player.ToPlayerRefClaim();
```

## API

| Type | Role |
|------|------|
| `GamingClaimTypes` | `PlayerRef` claim type (`novolis:player_ref`) |
| `ClaimsPrincipalExtensions` | `TryGetPlayerRef`, `ToPlayerRefClaim` |

## Related

| Package | When to use |
|---------|-------------|
| `Novolis.Game.Identity.Abstractions` | Ref types |
| `Novolis.Game.Multiplayer.AspNetCore` | SignalR lobbies with player claims |

## More documentation

- [Design](https://github.com/Novolis-Platform/novolis-gaming/blob/main/docs/design.md)
