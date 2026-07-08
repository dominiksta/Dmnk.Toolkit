using Dmnk.Blazor.InteractiveTests.Assets;

namespace Dmnk.Blazor.InteractiveTests.Tests;

[TestFixture]
public class AssetsAndTestProjectFileProviderFactoryTests
{
    [Test]
    public void Finds_StaticWebAssetFile_Of_Referenced_Project_Next_To_Assembly()
    {
        // Arrange
        const string project = "Dmnk.Blazor.InteractiveTests.Assets";
        var assembly = typeof(InteractiveTestsAssets).Assembly;
        
        // Act
        var found = 
            AssetsAndTestProjectFileProviderFactory.FindStaticWebAssetFileNextToAssembly(assembly);
        
        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(found, Is.Not.Null);
            Assert.That(found.Name, Is.EqualTo($"{project}.staticwebassets.runtime.json"));
            Assert.That(found.FullName, Does.Contain("bin"));
            Assert.That(found.FullName, Does.Contain(Const.Debug ? "Debug" : "Release"));
        }
    }

    private static IEnumerable<TestCaseData> Finds_StaticWebAssetsFile_In_FakeSolution_Obj_Dir_Cases()
    {
        yield return new TestCaseData("Debug", "net8.0");
        yield return new TestCaseData("Release", "net8.0");
        yield return new TestCaseData("Debug", "net10.0");
        yield return new TestCaseData("Release", "net10.0");
    }

    [Test, TestCaseSource(nameof(Finds_StaticWebAssetsFile_In_FakeSolution_Obj_Dir_Cases))]
    public void Finds_StaticWebAssetsFile_In_FakeSolution_Obj_Dir(
        string configuration, string targetFramework)
    {
        // Arrange
        var assemblyFile = new FileInfo(Path.Combine(
            new FileInfo(GetType().Assembly.Location).DirectoryName!,
            Const.FakeSlnDir, "My.BlazorLibrary", "bin", configuration, targetFramework,
            "My.BlazorLibrary.fakedll"));
        
        const string expectedName = "staticwebassets.development.json";
        
        // Act
        var pathInfo = InteractiveTestsProjectPathInfo.FromAssemblyFileInSolutionDir(
            assemblyFile, csprojExt: ".fakecsproj");
        var found = 
            AssetsAndTestProjectFileProviderFactory.FindStaticWebAssetFileFromObjDir(pathInfo);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(pathInfo.TestProjectDir.FullName, Does.EndWith("My.BlazorLibrary"));
            Assert.That(pathInfo.ConfigurationDir, Is.EqualTo(configuration));
            Assert.That(pathInfo.TargetFrameworkDir, Is.EqualTo(targetFramework));
            
            Assert.That(found, Is.Not.Null);
            Assert.That(found.Name, Is.EqualTo(expectedName));
            Assert.That(found.FullName, Does.Contain(configuration));
            Assert.That(found.FullName, Does.Contain(targetFramework));
        }
    }
}