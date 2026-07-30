# Design — novolis-gaming

## Purpose

Libraries used when **creating and shipping** a game: menus, pseudonymous player identity, multiplayer lobby patterns, procedural content helpers, Windows installer scripts. Not runtime simulation, physics, or rendering.

## Non-goals

- No `GameKit` monolith or Frank.GameEngine.Core port
- No `Novolis.Simulation` / `Novolis.Raylib` / `Novolis.Rendering` references in this repo
- No game domain (ships, factions, GalacticSim content)
- No PII types in public API (email, real name, provider subject strings)
- No SignalR in `novolis-transports` — game multiplayer lives here
- No live control session wire — that is `Novolis.Agent.Session` in `novolis-commands`

## Dependency firewall

| Package | May reference |
|---------|----------------|
| `Procedural` | BCL only |
| `Identity.*` | BCL; abstractions chain |
| `MenuFlows` | `Identity.Abstractions` |
| `Multiplayer.*` | `Identity.Abstractions`; AspNetCore → `Microsoft.AspNetCore.App` |
| `Packaging.Inno` | BCL / MSBuild only |

## PII split

- **Platform (`Novolis.Game.*`):** opaque `PlayerRef`, optional in-memory display names supplied by host
- **Product apps:** Steam/email/GDPR, persistence, Identity Server — implement `IExternalIdentityLinker` and auth

## Related repos

| Repo | Role |
|------|------|
| `novolis-commands` | Commands + `Novolis.Agent.Session` / `Novolis.Agent.Surface` |
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
