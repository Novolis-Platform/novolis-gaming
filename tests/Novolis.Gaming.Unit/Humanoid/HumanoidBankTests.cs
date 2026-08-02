using System.Numerics;
using Novolis.Game.Humanoid;
using Novolis.Simulation.Humanoid;

namespace Novolis.Gaming.Unit.Humanoid;

public class HumanoidBankTests
{
    [Test]
    public async Task ClipBank_SamplesWalk()
    {
        var clip = new HumanoidAnimationClip("walk")
            .AddKey(new HumanoidKeyframe { TimeSeconds = 0f })
            .AddKey(new HumanoidKeyframe
            {
                TimeSeconds = 1f,
                LocalRotations =
                {
                    [HumanoidBone.LeftUpLeg] = Quaternion.CreateFromAxisAngle(Vector3.UnitX, 0.2f),
                },
            });
        var bank = new HumanoidClipBank().Set(LocomotionClipKind.Walk, clip);
        var pose = new HumanoidPose();
        await Assert.That(bank.Sample(LocomotionClipKind.Walk, 0.5f, pose)).IsTrue();
    }

    [Test]
    public async Task BodyMask_AppliesUpperOnly()
    {
        var basePose = new HumanoidPose();
        var overlay = new HumanoidPose();
        overlay[HumanoidBone.LeftArm] = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, 1f);
        overlay[HumanoidBone.LeftUpLeg] = Quaternion.CreateFromAxisAngle(Vector3.UnitX, 1f);
        var dest = new HumanoidPose();
        HumanoidBodyMask.ApplyMasked(basePose, overlay, dest, HumanoidBodyMask.IsUpperBody);

        await Assert.That(dest[HumanoidBone.LeftArm]).IsEqualTo(overlay[HumanoidBone.LeftArm]);
        await Assert.That(dest[HumanoidBone.LeftUpLeg]).IsEqualTo(Quaternion.Identity);
    }

    [Test]
    public async Task BodyMask_Classifies_Lower_Body()
    {
        await Assert.That(HumanoidBodyMask.IsLowerBody(HumanoidBone.Hips)).IsTrue();
        await Assert.That(HumanoidBodyMask.IsLowerBody(HumanoidBone.LeftFoot)).IsTrue();
        await Assert.That(HumanoidBodyMask.IsLowerBody(HumanoidBone.LeftArm)).IsFalse();
        await Assert.That(HumanoidBodyMask.IsUpperBody(HumanoidBone.Count)).IsFalse();
    }

    [Test]
    public async Task ClipBank_Registers_Named_Clips()
    {
        var clip = new HumanoidAnimationClip("emote");
        var bank = new HumanoidClipBank().Set("wave", clip);
        await Assert.That(bank.TryGet("wave", out var resolved)).IsTrue();
        await Assert.That(resolved).IsSameReferenceAs(clip);
    }

    [Test]
    public async Task ClipBank_TryGet_Locomotion()
    {
        var clip = new HumanoidAnimationClip("walk");
        var bank = new HumanoidClipBank().Set(LocomotionClipKind.Walk, clip);
        await Assert.That(bank.TryGet(LocomotionClipKind.Walk, out var resolved)).IsTrue();
        await Assert.That(resolved).IsSameReferenceAs(clip);
    }

    [Test]
    public async Task ClipBank_Sample_Returns_False_When_Missing()
    {
        var bank = new HumanoidClipBank();
        var pose = new HumanoidPose();
        await Assert.That(bank.Sample(LocomotionClipKind.Run, 0f, pose)).IsFalse();
    }
}
