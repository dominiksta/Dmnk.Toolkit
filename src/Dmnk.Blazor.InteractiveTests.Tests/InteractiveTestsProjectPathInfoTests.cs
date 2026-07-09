namespace Dmnk.Blazor.InteractiveTests.Tests;

[TestFixture]
public class InteractiveTestsProjectPathInfoTests
{
    [Test]
    public void Extracts_Project_Name_Configuration_And_TargetFramework_From_Fake_Solution_Assembly_Path()
    {
        var assemblyFile = new FileInfo(Path.Combine(
            new FileInfo(GetType().Assembly.Location).DirectoryName!,
            Const.FakeSlnDir,
            "My.BlazorLibrary",
            "bin",
            "Release",
            "net8.0",
            "My.BlazorLibrary.fakedll"));

        var pathInfo = InteractiveTestsProjectPathInfo.FromAssemblyFileInSolutionDir(
            assemblyFile,
            ".fakecsproj");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(pathInfo.TestProjectName, Is.EqualTo("My.BlazorLibrary"));
            Assert.That(pathInfo.ConfigurationDir, Is.EqualTo("Release"));
            Assert.That(pathInfo.TargetFrameworkDir, Is.EqualTo("net8.0"));
            Assert.That(pathInfo.TestProjectDir.FullName, Does.EndWith("My.BlazorLibrary"));
        }
    }

    [Test]
    public void Throws_When_CsProj_Cannot_Be_Found()
    {
        using var temp = new TemporaryDirectory();
        var assemblyFile = temp.CreateFile(Path.Combine("bin", "Debug", "net10.0", "TestAssembly.dll"));

        Assert.That(
            () => InteractiveTestsProjectPathInfo.FromAssemblyFileInSolutionDir(
                assemblyFile,
                ".fakecsproj"),
            Throws.InstanceOf<FileNotFoundException>()
                .With.Message.Contains("csproj not found for assembly"));
    }
}
