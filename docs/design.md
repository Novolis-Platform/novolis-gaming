# Design — novolis-gaming

## Purpose

Libraries used when **creating and shipping** a game: menus, pseudonymous player identity, multiplayer lobby patterns, Windows installer scripts. Not runtime simulation, physics, or rendering.

## Non-goals

- No `GameKit` monolith or Frank.GameEngine.Core port
- No `Novolis.Simulation` / `Novolis.Raylib` / `Novolis.Rendering` references in this repo
- No game domain (ships, factions, GalacticSim content)
- No PII types in public API (email, real name, provider subject strings)
- No SignalR in `novolis-transports` — game multiplayer lives here

## Dependency firewall

| Package | May reference |
|---------|----------------|
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
| `novolis-install` | Novolis platform package installer (`novolis` global tool) |
| `novolis-templates` | `dotnet new` scaffolds (general + MonoGame) |
| `novolis-workflows` | Backend workflow orchestration |
| `novolis-dogfooding` | Integration samples |

## Backlog

- `dotnet new` game templates (coordinate with `novolis-templates`)
- Full Inno compile integration (invoke ISCC)
- SignalR host migration and production auth samples
