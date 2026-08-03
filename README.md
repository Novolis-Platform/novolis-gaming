<!-- novolis-marketing:start -->
<p align="center">
  <a href="https://github.com/Novolis-Platform">
    <img src="https://raw.githubusercontent.com/Novolis-Platform/.github/main/brand/logo-brand-transparent.svg" width="360" alt="Novolis"/>
  </a>
</p>

<p align="center">
  <img src="https://raw.githubusercontent.com/Novolis-Platform/.github/main/brand/banners/novolis-gaming.svg" width="100%" alt="novolis-gaming"/>
</p>

<p align="center">
  <strong>Game authoring above simulation</strong><br/>
  Game identity, humanoids, menu flows, packaging — no Avalonia in this layer.
</p>

<p align="center">
  <a href="https://github.com/Novolis-Platform/novolis-gaming/actions"><img src="https://img.shields.io/github/actions/workflow/status/Novolis-Platform/novolis-gaming/merge.yml?branch=main&label=merge&logo=github" alt="merge"/></a>
  <a href="https://github.com/orgs/Novolis-Platform/packages?repo_name=novolis-gaming"><img src="https://img.shields.io/badge/packages-GitHub%20Packages-0a7ea3?logo=nuget" alt="packages"/></a>
  <a href="https://github.com/Novolis-Platform"><img src="https://img.shields.io/badge/org-Novolis--Platform-111827" alt="org"/></a>
</p>

<p align="center">
  <a href="https://nuget.pkg.github.com/Novolis-Platform/index.json"><code>https://nuget.pkg.github.com/Novolis-Platform/index.json</code></a>
  ·
  <a href="https://github.com/Novolis-Platform/.github/blob/main/profile/README.md">Org landing</a>
  ·
  <a href="https://github.com/Novolis-Platform/novolis-governance">Governance</a>
</p>

---
<!-- novolis-marketing:end -->
<!-- novolis-package-index:start -->
> **GitHub Packages shows this repository README on every package page** (upstream limitation).
> Open the **package README** for install and quick start — embedded in each .nupkg and linked below.

## Published packages

| Package | Install | Package README |
|---------|---------|----------------|
| `Novolis.Game.Humanoid` | `dotnet add package Novolis.Game.Humanoid` | [README](https://github.com/Novolis-Platform/novolis-gaming/blob/main/src/Novolis.Game.Humanoid/README.md) |
| `Novolis.Game.Identity` | `dotnet add package Novolis.Game.Identity` | [README](https://github.com/Novolis-Platform/novolis-gaming/blob/main/src/Novolis.Game.Identity/README.md) |
| `Novolis.Game.Identity.Abstractions` | `dotnet add package Novolis.Game.Identity.Abstractions` | [README](https://github.com/Novolis-Platform/novolis-gaming/blob/main/src/Novolis.Game.Identity.Abstractions/README.md) |
| `Novolis.Game.Identity.AspNetCore` | `dotnet add package Novolis.Game.Identity.AspNetCore` | [README](https://github.com/Novolis-Platform/novolis-gaming/blob/main/src/Novolis.Game.Identity.AspNetCore/README.md) |
| `Novolis.Game.MenuFlows` | `dotnet add package Novolis.Game.MenuFlows` | [README](https://github.com/Novolis-Platform/novolis-gaming/blob/main/src/Novolis.Game.MenuFlows/README.md) |
| `Novolis.Game.Multiplayer.Abstractions` | `dotnet add package Novolis.Game.Multiplayer.Abstractions` | [README](https://github.com/Novolis-Platform/novolis-gaming/blob/main/src/Novolis.Game.Multiplayer.Abstractions/README.md) |
| `Novolis.Game.Multiplayer.AspNetCore` | `dotnet add package Novolis.Game.Multiplayer.AspNetCore` | [README](https://github.com/Novolis-Platform/novolis-gaming/blob/main/src/Novolis.Game.Multiplayer.AspNetCore/README.md) |
| `Novolis.Game.Packaging.Inno` | `dotnet add package Novolis.Game.Packaging.Inno` | [README](https://github.com/Novolis-Platform/novolis-gaming/blob/main/src/Novolis.Game.Packaging.Inno/README.md) |
| `Novolis.Game.Procedural` | `dotnet add package Novolis.Game.Procedural` | [README](https://github.com/Novolis-Platform/novolis-gaming/blob/main/src/Novolis.Game.Procedural/README.md) |

For NuGet.org and Visual Studio, the **embedded** README.md inside each package is authoritative.

<!-- novolis-package-index:end -->
# novolis-gaming

Game **authoring and shipping** libraries for Novolis: pseudonymous identity, menu flows, multiplayer lobby glue, procedural content tools, and Inno Setup packaging helpers.

This repo is **not** the simulation/render stack (`novolis-math`, `novolis-simulation`, `novolis-raylib`, `novolis-rendering`). Product games compose those packages with `Novolis.Game.*` at the app layer.

Live agent surfaces are **`Novolis.Agent.Core` / `Novolis.Agent.Surface`** in [`novolis-agent`](https://github.com/Novolis-Platform/novolis-agent).

## Packages

| Package | Purpose |
|---------|---------|
| `Novolis.Game.Procedural` | Seeded noise terrain, infinite chunks, runner tracks, spawn tables |
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

See [docs/getting-started.md](docs/getting-started.md) and [docs/design.md](docs/design.md).

## Policy

- [NuGet-only cross-repo dependencies](https://github.com/Novolis-Platform/novolis-governance/blob/main/docs/nuget-only-policy.md)
- [Gaming layer policy](https://github.com/Novolis-Platform/novolis-governance/blob/main/docs/gaming-layer-policy.md)

