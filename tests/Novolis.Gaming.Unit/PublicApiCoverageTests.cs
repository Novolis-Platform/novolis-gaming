using System.Security.Claims;
using Novolis.Game.Humanoid;
using Novolis.Game.Identity;
using Novolis.Game.Identity.Abstractions;
using Novolis.Game.Identity.AspNetCore;
using Novolis.Game.MenuFlows;
using Novolis.Game.Packaging.Inno;
using Novolis.Game.Procedural;
using Novolis.Simulation.Humanoid;

namespace Novolis.Gaming.Unit;

public sealed class PublicApiCoverageTests
{
    [Test]
    public async Task Identity_Directory_And_Factory_Cover_Optional_And_Overwrite_Paths()
    {
        var directory = new InMemoryPlayerDirectory();
        var unnamed = PlayerRefFactory.CreateGuest(directory, " ");
        var named = PlayerRefFactory.CreateGuest(directory, "First");

        directory.SetDisplayName(named, "Second");

        await Assert.That(directory.TryGetDisplayName(unnamed, out var missing)).IsFalse();
        await Assert.That(missing).IsNull();
        await Assert.That(directory.TryGetDisplayName(named, out var current)).IsTrue();
        await Assert.That(current).IsEqualTo("Second");
    }

    [Test]
    public async Task Identity_Linker_Separates_Keys_And_Allows_Rebinding()
    {
        var linker = new InMemoryExternalIdentityLinker();
        var first = PlayerRef.New();
        var second = PlayerRef.New();
        var provider = new ExternalProviderRef("provider-a");
        var subject = new ExternalSubjectHash("subject");

        linker.Link(provider, subject, first);
        linker.Link(provider, subject, second);

        await Assert.That(linker.TryLink(provider, subject, out var resolved)).IsTrue();
        await Assert.That(resolved).IsEqualTo(second);
        await Assert.That(linker.TryLink(new ExternalProviderRef("provider-b"), subject, out var absent)).IsFalse();
        await Assert.That(absent).IsEqualTo(default(PlayerRef));
    }

    [Test]
    public async Task ClaimsPrincipal_Rejects_Missing_And_Invalid_Player_Claims()
    {
        var missing = new ClaimsPrincipal(new ClaimsIdentity());
        var invalid = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(GamingClaimTypes.PlayerRef, "not-a-guid")]));

        await Assert.That(missing.TryGetPlayerRef(out var missingPlayer)).IsFalse();
        await Assert.That(missingPlayer).IsEqualTo(default(PlayerRef));
        await Assert.That(invalid.TryGetPlayerRef(out var invalidPlayer)).IsFalse();
        await Assert.That(invalidPlayer).IsEqualTo(default(PlayerRef));
    }

    [Test]
    public async Task ScreenStack_Invokes_Lifecycle_And_Pop_Transitions()
    {
        var trace = new List<string>();
        var stack = new GameScreenStack();
        var transitions = new List<GameScreenTransition>();
        stack.Transitioned += transitions.Add;
        var first = new RecordingScreen("first", trace);
        var second = new RecordingScreen("second", trace);

        await stack.PushAsync(first);
        await stack.PushAsync(second);
        await Assert.That(await stack.PopAsync()).IsTrue();
        await Assert.That(await stack.PopAsync()).IsTrue();

        await Assert.That(trace).IsEquivalentTo(new[]
        {
            "first-enter",
            "first-exit",
            "second-enter",
            "second-exit",
            "first-enter",
            "first-exit",
        });
        await Assert.That(transitions.Count).IsEqualTo(2);
        await Assert.That(transitions[1]).IsEqualTo(new GameScreenTransition("second", "first"));
        await Assert.That(stack.Current).IsNull();
    }

    [Test]
    public async Task ScreenStack_Rejects_Null_Screen()
    {
        var stack = new GameScreenStack();
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await stack.PushAsync(null!));
    }

    [Test]
    public async Task Procedural_Public_APIs_Clamp_Inputs_And_Handle_Boundaries()
    {
        var zeroSeed = new SeededRng(0UL);
        var signedSeed = new SeededRng(-1L);
        var first = zeroSeed.NextSingle();
        var ranged = signedSeed.NextSingle(-4, -2);
        var reversed = zeroSeed.NextInt(7, 2);
        var ramp = new DifficultyRamp(10, 5, 0);
        var generator = new InfiniteTrackGenerator(0, segmentLength: 1, lanes: 99, difficulty: ramp);
        var stream = new InfiniteChunkStream(chunkSize: 0, radius: -2);
        var field = new NoiseHeightfield(0, frequency: 0, amplitude: 0, baseHeight: 3, octaves: 99);

        stream.Update(-0.1f, -1.1f);

        await Assert.That(first).IsGreaterThanOrEqualTo(0);
        await Assert.That(first).IsLessThan(1);
        await Assert.That(ranged).IsGreaterThanOrEqualTo(-4);
        await Assert.That(ranged).IsLessThan(-2);
        await Assert.That(reversed).IsEqualTo(7);
        await Assert.That(ramp.Evaluate(11)).IsEqualTo(1);
        await Assert.That(generator.SegmentLength).IsEqualTo(4);
        await Assert.That(generator.IndexAt(-0.1f)).IsEqualTo(-1);
        await Assert.That(stream.ChunkSize).IsEqualTo(1);
        await Assert.That(stream.Radius).IsEqualTo(0);
        await Assert.That(stream.Loaded.Single()).IsEqualTo(new ChunkCoord(-1, -2));
        await Assert.That(field.SampleHeight(100, 100)).IsEqualTo(3);
        await Assert.That(float.IsFinite(Noise.Value2D(-2.5f, 3.25f, 0))).IsTrue();
    }

    [Test]
    public async Task Humanoid_APIs_Cover_Named_Lookup_And_Validation()
    {
        var clip = new HumanoidAnimationClip("Wave");
        var bank = new HumanoidClipBank().Set("Wave", clip);

        await Assert.That(bank.TryGet("wave", out var resolved)).IsTrue();
        await Assert.That(resolved).IsSameReferenceAs(clip);
        await Assert.That(bank.TryGet("missing", out _)).IsFalse();
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
        {
            bank.Set(LocomotionClipKind.Idle, null!);
            return Task.CompletedTask;
        });
        await Assert.ThrowsAsync<ArgumentException>(() =>
        {
            bank.Set(" ", clip);
            return Task.CompletedTask;
        });
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
        {
            bank.Set("valid", null!);
            return Task.CompletedTask;
        });
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
        {
            HumanoidBodyMask.ApplyMasked(new HumanoidPose(), new HumanoidPose(), new HumanoidPose(), null!);
            return Task.CompletedTask;
        });
    }

    [Test]
    [Arguments("AppName")]
    [Arguments("AppVersion")]
    [Arguments("PublishDir")]
    [Arguments("AppExeName")]
    [Arguments("OutputDir")]
    [Arguments("AppId")]
    [Arguments("InstallDirName")]
    public async Task Inno_Generator_Validates_Required_Values(string blankProperty)
    {
        var generator = ValidGenerator(blankProperty);

        await Assert.ThrowsAsync<ArgumentException>(() =>
        {
            _ = generator.Generate();
            return Task.CompletedTask;
        });
    }

    [Test]
    public async Task Inno_Generator_Trims_Publish_Trailing_Separators()
    {
        var script = ValidGenerator().Generate();

        await Assert.That(script).Contains("""Source: "C:\publish\*" """.TrimEnd());
        await Assert.That(script).DoesNotContain(@"C:\publish\\*");
    }

    private static InnoScriptGenerator ValidGenerator(string? blankProperty = null) => new()
    {
        AppName = blankProperty == nameof(InnoScriptGenerator.AppName) ? " " : "Game",
        AppVersion = blankProperty == nameof(InnoScriptGenerator.AppVersion) ? " " : "1.0",
        PublishDir = blankProperty == nameof(InnoScriptGenerator.PublishDir) ? " " : @"C:\publish\\",
        AppExeName = blankProperty == nameof(InnoScriptGenerator.AppExeName) ? " " : "Game.exe",
        OutputDir = blankProperty == nameof(InnoScriptGenerator.OutputDir) ? " " : @"C:\output",
        AppId = blankProperty == nameof(InnoScriptGenerator.AppId) ? " " : "Novolis.Game",
        InstallDirName = blankProperty == nameof(InnoScriptGenerator.InstallDirName) ? " " : @"Novolis\Game",
    };

    private sealed class RecordingScreen(string id, List<string> trace) : IGameScreen
    {
        public string ScreenId => id;

        public ValueTask OnEnterAsync(CancellationToken cancellationToken = default)
        {
            trace.Add($"{id}-enter");
            return ValueTask.CompletedTask;
        }

        public ValueTask OnExitAsync(CancellationToken cancellationToken = default)
        {
            trace.Add($"{id}-exit");
            return ValueTask.CompletedTask;
        }
    }
}
