# Novolis.Game.MenuFlows

Stack-based screen navigation for game menus (main, settings, pause) without Raylib or UI bindings.

## Install

```bash
dotnet add package Novolis.Game.MenuFlows
```

## Quick start

```csharp
var stack = new GameScreenStack();
await stack.PushAsync(new MainMenuScreen());
```

## Related packages

| Package | When to use |
|---------|-------------|
| `Novolis.Game.Identity.Abstractions` | Player context on screens |

## More documentation

- [Design](https://github.com/Novolis-Platform/novolis-gaming/blob/main/docs/design.md)
