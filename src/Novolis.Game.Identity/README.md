# Novolis.Game.Identity

In-memory implementations of `IPlayerDirectory` and `IExternalIdentityLinker` for local games and tests.

## Install

```bash
dotnet add package Novolis.Game.Identity
```

## Quick start

```csharp
var directory = new InMemoryPlayerDirectory();
var player = PlayerRefFactory.CreateGuest(directory, "Guest-1");
```

## Related packages

| Package | When to use |
|---------|-------------|
| `Novolis.Game.Identity.Abstractions` | Contracts and ref types |
| `Novolis.Game.Identity.AspNetCore` | ASP.NET claim mapping |

## More documentation

- [Design](https://github.com/Novolis-Platform/novolis-gaming/blob/main/docs/design.md)
