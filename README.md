# novolis-gaming

Game **authoring and shipping** libraries for Novolis: pseudonymous identity, menu flows, multiplayer lobby glue, player session protocol, and Inno Setup packaging helpers.

This repo is **not** the simulation/render stack (`novolis-math`, `novolis-simulation`, `novolis-raylib`, `novolis-rendering`). Product games compose those packages with `Novolis.Game.*` at the app layer.

## Packages

| Package | Purpose |
|---------|---------|
| `Novolis.Game.Session` | Decision-point session protocol (`session.*`) + LocalIpc/stdio hosts |
| `Novolis.Game.Identity.Abstractions` | `PlayerRef`, session/device refs, linker contracts |
| `Novolis.Game.Identity` | In-memory directory and linker |
| `Novolis.Game.Identity.AspNetCore` | Claims → `PlayerRef` |
| `Novolis.Game.MenuFlows` | Screen stack navigation |
| `Novolis.Game.Multiplayer.Abstractions` | Lobby state |
| `Novolis.Game.Multiplayer.AspNetCore` | SignalR hub base |
| `Novolis.Game.Packaging.Inno` | Inno Setup script + MSBuild targets |

## Build

```bash
dotnet restore
dotnet build Novolis.Gaming.slnx
dotnet test tests/Novolis.Gaming.Unit
```

See [docs/getting-started.md](docs/getting-started.md), [docs/design.md](docs/design.md), and [docs/session-protocol.md](docs/session-protocol.md).

## Policy

- [NuGet-only cross-repo dependencies](https://github.com/Novolis-Platform/novolis-governance/blob/main/docs/nuget-only-policy.md)
- [Gaming layer policy](https://github.com/Novolis-Platform/novolis-governance/blob/main/docs/gaming-layer-policy.md)
