# Novolis.Game.Humanoid

Game-facing clip banks and body masks. The skeleton schema lives in `Novolis.Simulation.Humanoid`.

## Install

```bash
dotnet add package Novolis.Game.Humanoid
```

## Quick start

```csharp
var bank = new HumanoidClipBank()
    .Set(LocomotionClipKind.Walk, walkClip)
    .Set("slash", attackClip);

var pose = new HumanoidPose();
bank.Sample(LocomotionClipKind.Walk, time, pose, bind);

HumanoidBodyMask.ApplyMasked(locomotionPose, attackPose, layered, HumanoidBodyMask.IsUpperBody);
```
