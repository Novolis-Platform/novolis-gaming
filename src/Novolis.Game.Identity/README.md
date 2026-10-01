<!-- novolis-pkg-brand:start -->
[![Novolis](https://raw.githubusercontent.com/Novolis-Platform/.github/main/brand/logo-icon.png)](https://novolis-platform.github.io/.github/novolis-gaming/)

[Novolis](https://github.com/Novolis-Platform) · [Docs](https://novolis-platform.github.io/.github/novolis-gaming/) · [Source](https://github.com/Novolis-Platform/novolis-gaming)
<!-- novolis-pkg-brand:end -->

# Novolis.Game.Identity

In-memory implementations of `IPlayerDirectory` and `IExternalIdentityLinker` for local games and tests.

## Install

```bash
dotnet add package Novolis.Game.Identity
```

## Quick start

```csharp
using Novolis.Game.Identity;

var directory = new InMemoryPlayerDirectory();
var player = PlayerRefFactory.CreateGuest(directory, "Guest-1");
```

## API

| Type | Role |
|------|------|
| `InMemoryPlayerDirectory` | Thread-safe `IPlayerDirectory` |
| `InMemoryExternalIdentityLinker` | Thread-safe `IExternalIdentityLinker` |
| `PlayerRefFactory` | `CreateGuest(directory, displayName?)` → `PlayerRef` |

## Related

| Package | When to use |
|---------|-------------|
| `Novolis.Game.Identity.Abstractions` | Contracts and ref types |
| `Novolis.Game.Identity.AspNetCore` | ASP.NET claim mapping |

## More documentation

- [Design](https://github.com/Novolis-Platform/novolis-gaming/blob/main/docs/design.md)

