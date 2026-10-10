# Design — novolis-gaming

## Purpose

Libraries used when **creating and shipping** a game: scenes, menus, identity, multiplayer lobby patterns, procedural content helpers, Windows installer scripts, and thin humanoid clip banks. Sits on Simulation and Rendering. Does not own window hosts.

## Non-goals

- No `GameKit` monolith or Frank.GameEngine.Core port
- No host frameworks (`Novolis.Silk`, `Novolis.Raylib`, Avalonia, MAUI, Blazor)
- No new Planar / Modeling-named packages — dimension is not a layer
- No game domain (ships, factions, GalacticSim content)
- No PII types in public API (email, real name, provider subject strings)
- No SignalR in `novolis-transports` — game multiplayer lives here
- No live agent surface wire — that is `novolis-agent` (`Novolis.Agent.Core` / `.Surface`)
- No world / voxel stacks — consume `Novolis.Simulation.*` when needed; do not re-own them

## Dependency firewall

| Package | May reference |
|---------|----------------|
| `Scenes` | Rendering + Math (Appearance, Presentation, topology as needed) |
| `Procedural` | BCL only (apps wrap `NoiseHeightfield.SampleHeight` into Physics `IHeightSampler` if needed) |
| `Humanoid` | `Novolis.Simulation.Humanoid` (clip banks / body masks only) |
| `Identity.*` | BCL; abstractions chain |
| `MenuFlows` | `Identity.Abstractions` |
| `Multiplayer.*` | `Identity.Abstractions`; AspNetCore → `Microsoft.AspNetCore.App` |
| `Packaging.Inno` | BCL / MSBuild only |

Matches [gaming-layer-policy.md](https://github.com/Novolis-Platform/novolis-governance/blob/main/docs/gaming-layer-policy.md).

## PII split

- **Platform (`Novolis.Game.*`):** opaque `PlayerRef`, optional in-memory display names supplied by host
- **Product apps:** Steam/email/GDPR, persistence, Identity Server — implement `IExternalIdentityLinker` and auth

## Related repos

| Repo | Role |
|------|------|
| `novolis-commands` | Commands (intent) · `novolis-agent` | Agent Surface |
| `novolis-install` | Novolis platform package installer (`novolis` global tool) |
| `novolis-templates` | `dotnet new` scaffolds (general + MonoGame) |
| `novolis-workflows` | Shared GitHub Actions workflows for org CI/CD |
| `novolis-dogfooding` | Integration samples |

WorkflowEngine (Cron / Mapping / Messaging orchestration) is a future import into a dedicated **`novolis-workflow-engine`** repo — not `novolis-workflows`.

## Backlog

- `dotnet new` game templates (coordinate with `novolis-templates`)
- Full Inno compile integration (invoke ISCC)
- SignalR host migration and production auth samples
- Dogfood infinite-runner sample using `Novolis.Game.Procedural`
