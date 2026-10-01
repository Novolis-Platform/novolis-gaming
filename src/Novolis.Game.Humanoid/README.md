<!-- novolis-pkg-brand:start -->
[![Novolis](https://raw.githubusercontent.com/Novolis-Platform/.github/main/brand/logo-icon.png)](https://novolis-platform.github.io/.github/novolis-gaming/)

[Novolis](https://github.com/Novolis-Platform) · [Docs](https://novolis-platform.github.io/.github/novolis-gaming/) · [Source](https://github.com/Novolis-Platform/novolis-gaming)
<!-- novolis-pkg-brand:end -->

# Novolis.Game.Humanoid

Game-facing clip banks and body masks. The skeleton schema lives in `Novolis.Simulation.Humanoid`.

## Install

```bash
dotnet add package Novolis.Game.Humanoid
```

## Quick start

```csharp
using Novolis.Game.Humanoid;
using Novolis.Simulation.Humanoid;

var bank = new HumanoidClipBank()
    .Set(LocomotionClipKind.Walk, walkClip)
    .Set("slash", attackClip);

var pose = new HumanoidPose();
bank.Sample(LocomotionClipKind.Walk, time, pose, bind);

HumanoidBodyMask.ApplyMasked(locomotionPose, attackPose, layered, HumanoidBodyMask.IsUpperBody);
```

## API

| Type | Role |
|------|------|
| `LocomotionClipKind` | `Idle`, `Walk`, `Run`, `Jump`, `Fall`, `Land` |
| `HumanoidClipBank` | `Set(kind\|name, clip)`, `TryGet`, `Sample(kind, timeSeconds, pose, bind?)` |
| `HumanoidBodyMask` | `IsUpperBody`, `IsLowerBody`, `ApplyMasked(base, overlay, dest, include)` |

## Related

| Package | When to use |
|---------|-------------|
| `Novolis.Simulation.Humanoid` | `HumanoidPose`, `HumanoidAnimationClip`, `HumanoidBindPose` |
| `Novolis.Simulation.Humanoid.Import` | BVH/glTF clip import |

