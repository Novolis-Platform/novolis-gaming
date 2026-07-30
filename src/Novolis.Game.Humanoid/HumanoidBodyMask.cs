using Novolis.Simulation.Humanoid;

namespace Novolis.Game.Humanoid;

/// <summary>Body-part masks for layering upper/lower animation.</summary>
public static class HumanoidBodyMask
{
    /// <summary>True for spine, head, and arms (not legs).</summary>
    public static bool IsUpperBody(HumanoidBone bone) => bone switch
    {
        HumanoidBone.Hips => false,
        HumanoidBone.LeftUpLeg or HumanoidBone.LeftLeg or HumanoidBone.LeftFoot or HumanoidBone.LeftToeBase => false,
        HumanoidBone.RightUpLeg or HumanoidBone.RightLeg or HumanoidBone.RightFoot or HumanoidBone.RightToeBase => false,
        HumanoidBone.Count => false,
        _ => true,
    };

    /// <summary>True for hips and legs.</summary>
    public static bool IsLowerBody(HumanoidBone bone) =>
        bone is HumanoidBone.Hips
            or HumanoidBone.LeftUpLeg or HumanoidBone.LeftLeg or HumanoidBone.LeftFoot or HumanoidBone.LeftToeBase
            or HumanoidBone.RightUpLeg or HumanoidBone.RightLeg or HumanoidBone.RightFoot or HumanoidBone.RightToeBase;

    /// <summary>
    /// Writes <paramref name="overlay"/> locals onto <paramref name="destination"/> for bones where
    /// <paramref name="include"/> returns true; other bones keep <paramref name="basePose"/>.
    /// </summary>
    public static void ApplyMasked(
        HumanoidPose basePose,
        HumanoidPose overlay,
        HumanoidPose destination,
        Func<HumanoidBone, bool> include)
    {
        ArgumentNullException.ThrowIfNull(include);
        destination.RootTranslation = basePose.RootTranslation;
        for (var i = 0; i < (int)HumanoidBone.Count; i++)
        {
            var bone = (HumanoidBone)i;
            destination[bone] = include(bone) ? overlay[bone] : basePose[bone];
        }
    }
}
