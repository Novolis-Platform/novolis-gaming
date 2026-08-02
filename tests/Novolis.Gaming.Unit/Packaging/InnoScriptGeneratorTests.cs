using Novolis.Game.Packaging.Inno;

namespace Novolis.Gaming.Unit.Packaging;

public class InnoScriptGeneratorTests
{
    [Test]
    public async Task Generate_Uses_Per_User_Path_And_Novolis_Brand()
    {
        var script = new InnoScriptGenerator
        {
            AppName = "TestGame",
            AppVersion = "1.0.0",
            PublishDir = @"C:\publish",
            AppExeName = "TestGame.exe",
            OutputDir = @"C:\publish\installer",
            AppId = "Novolis.TestGame",
            InstallDirName = @"Novolis\TestGame",
            LicenseFile = @"C:\repo\LICENSE",
        }.Generate();

        await Assert.That(script).Contains("TestGame");
        await Assert.That(script).Contains("PrivilegesRequired=lowest");
        await Assert.That(script).Contains("{localappdata}\\Programs\\Novolis\\TestGame");
        await Assert.That(script).Contains("AppPublisher=Novolis");
        await Assert.That(script).Contains("AppPublisherURL=https://github.com/Novolis-Platform");
        await Assert.That(script).Contains(@"LicenseFile=C:\repo\LICENSE");
        await Assert.That(script).DoesNotContain("{autopf}");
    }

    [Test]
    public async Task Generate_Includes_Optional_Urls_And_Icon()
    {
        var script = new InnoScriptGenerator
        {
            AppName = "OptionalUrls",
            AppVersion = "2.0.0",
            PublishDir = @"D:\out",
            AppExeName = "OptionalUrls.exe",
            OutputDir = @"D:\out\installer",
            AppSupportUrl = "https://example.com/support",
            AppUpdatesUrl = "https://example.com/updates",
            SetupIconFile = @"D:\assets\icon.ico",
        }.Generate();

        await Assert.That(script).Contains("AppSupportURL=https://example.com/support");
        await Assert.That(script).Contains("AppUpdatesURL=https://example.com/updates");
        await Assert.That(script).Contains(@"SetupIconFile=D:\assets\icon.ico");
    }
}
