<!-- novolis-pkg-brand:start -->
<p align="center">
  <a href="https://github.com/Novolis-Platform/novolis-gaming">
    <img src="https://raw.githubusercontent.com/Novolis-Platform/.github/main/brand/logo-icon.svg" width="72" alt="Novolis"/>
  </a>
</p>
<!-- novolis-pkg-brand:end -->

# Novolis.Game.Packaging.Inno

Per-user Inno Setup script generation for Novolis games — same contract as `Novolis.Avalonia.Packaging.Inno`.

Generates installer scripts with `PrivilegesRequired=lowest` (installs to `%LocalAppData%\Programs\…`, no admin), publisher `Novolis`, and optional MIT license and icon paths.

Avalonia desktop apps should use `Novolis.Avalonia.Packaging.Inno` instead.

## Install

```bash
dotnet add package Novolis.Game.Packaging.Inno
```

## Quick start

```csharp
using Novolis.Game.Packaging.Inno;

var script = new InnoScriptGenerator
{
    AppName = "My Game",
    AppVersion = "1.0.0",
    PublishDir = @"artifacts\publish",
    AppExeName = "MyGame.exe",
    OutputDir = @"artifacts\installer"
}.Generate();

File.WriteAllText("setup.iss", script);
```

MSBuild target `NovolisGenerateInnoScript` is available via `Novolis.Game.Packaging.Inno.targets` with matching `NovolisInno*` properties.

## API

| Type | Role |
|------|------|
| `InnoScriptGenerator` | Required: `AppName`, `AppVersion`, `PublishDir`, `AppExeName`, `OutputDir`. Optional: `AppId`, icons, license, publisher URLs. `Generate()` → `.iss` text |

## Related

| Package | When to use |
|---------|-------------|
| `Novolis.Avalonia.Packaging.Inno` | Avalonia desktop app installers |

