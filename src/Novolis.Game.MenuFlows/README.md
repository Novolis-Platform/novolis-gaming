# Novolis.Game.MenuFlows

Stack-based screen navigation for game menus (main, settings, pause) without Raylib or UI bindings.

## Install

```bash
dotnet add package Novolis.Game.MenuFlows
```

## Quick start

```csharp
using Novolis.Game.MenuFlows;

var stack = new GameScreenStack();
await stack.PushAsync(new MainMenuScreen());
// ...
await stack.PopAsync();
```

Implement `IGameScreen` with `ScreenId`, `OnEnterAsync`, and `OnExitAsync`. Subclass `PauseScreenBase` for pause overlays.

## API

| Type | Role |
|------|------|
| `IGameScreen` | `ScreenId`, `OnEnterAsync`, `OnExitAsync` |
| `GameScreenStack` | `Current`, `Transitioned` event, `PushAsync`, `PopAsync` |
| `GameScreenTransition` | `FromScreenId`, `ToScreenId` |
| `PauseScreenBase` | Abstract pause screen (`ScreenId = "pause"`) |

## Related

| Package | When to use |
|---------|-------------|
| `Novolis.Game.Identity.Abstractions` | Player context on screens |

## More documentation

- [Design](https://github.com/Novolis-Platform/novolis-gaming/blob/main/docs/design.md)
