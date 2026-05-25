# Getting started

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- GitHub Packages auth for `Novolis.*` (see [novolis-governance nuget setup](https://github.com/Novolis-Platform/novolis-governance/blob/main/docs/nuget-setup.md))

## Clone and build

```bash
git clone https://github.com/Novolis-Platform/novolis-gaming.git
cd novolis-gaming
dotnet restore
dotnet build Novolis.Gaming.slnx
dotnet test tests/Novolis.Gaming.Unit
```

## Consume from GPR

```bash
dotnet add package Novolis.Game.Identity.Abstractions --version 2026.1.*
```

Restore uses `nuget.org` + `https://nuget.pkg.github.com/Novolis-Platform/index.json` only.

## Sibling checkout

When cloned under `d:\novolis\` next to `novolis-governance`, MSBuild imports documentation and GPR props automatically. Cross-repo references remain **PackageReference** only in consumers.
