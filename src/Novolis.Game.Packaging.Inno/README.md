# Novolis.Game.Packaging.Inno

Generate Inno Setup `.iss` scripts and MSBuild targets for Windows game installers.

## Install

```bash
dotnet add package Novolis.Game.Packaging.Inno
```

## Quick start

```csharp
var iss = new InnoScriptGenerator
{
    AppName = "MyGame",
    AppVersion = "1.0.0",
    PublishDir = @"bin\Release\net10.0\publish\win-x64"
}.Generate();
```

Import targets: `<PackageReference Include="Novolis.Game.Packaging.Inno" />` then set `NovolisInnoAppName`, `NovolisInnoPublishDir` before `dotnet publish`.

## More documentation

- [Design](https://github.com/Novolis-Platform/novolis-gaming/blob/main/docs/design.md)
