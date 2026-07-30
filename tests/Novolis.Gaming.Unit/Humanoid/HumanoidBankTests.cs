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
}
