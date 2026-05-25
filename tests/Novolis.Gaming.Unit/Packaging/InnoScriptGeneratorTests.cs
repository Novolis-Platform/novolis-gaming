using Novolis.Game.Packaging.Inno;

namespace Novolis.Gaming.Unit.Packaging;

public class InnoScriptGeneratorTests
{
    [Test]
    public async Task Generate_Includes_AppName()
    {
        var script = new InnoScriptGenerator
        {
            AppName = "TestGame",
            AppVersion = "1.0.0",
            PublishDir = @"C:\publish"
        }.Generate();
        await Assert.That(script).Contains("TestGame");
    }
}
