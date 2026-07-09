using Microsoft.Extensions.FileProviders;

namespace Dmnk.Blazor.InteractiveTests.Tests;

[TestFixture]
public class AssetsAndTestProjectFileProviderFactoryTests
{
    private const string FakeSolutionRootToken = "__FAKE_SOLUTION_ROOT__";
    
    /// <summary>
    /// Rewrite the manifest files (in the *output* directory, not the ones commited to the repo)
    /// such that <see cref="FakeSolutionRootToken"/> is replaced by the actual full path on disk.
    ///
    /// The manifest file provider does not seem to play nice with relative paths in there.
    /// </summary>
    [SetUp]
    public void MaterializeFakeSolutionStaticWebAssets()
    {
        var fakeSolutionDir = FakeSolutionDir();
        var escapedRoot = EscapeJsonPath(fakeSolutionDir.FullName);

        foreach (var manifestPath in Directory.EnumerateFiles(
                     fakeSolutionDir.FullName,
                     "staticwebassets.development.json",
                     SearchOption.AllDirectories))
        {
            var text = File.ReadAllText(manifestPath);
            if (!text.Contains(FakeSolutionRootToken)) continue;

            File.WriteAllText(
                manifestPath,
                text.Replace(FakeSolutionRootToken, escapedRoot));
        }
    }

    private static IEnumerable<TestCaseData> Finds_StaticWebAssetsFile_In_FakeSolution_Obj_Dir_Cases()
    {
        yield return new TestCaseData("Debug", "net8.0");
        yield return new TestCaseData("Release", "net8.0");
        yield return new TestCaseData("Debug", "net10.0");
        yield return new TestCaseData("Release", "net10.0");
    }

    private DirectoryInfo FakeSolutionDir() =>
        new(Path.Combine(
            new FileInfo(GetType().Assembly.Location).DirectoryName!,
            Const.FakeSlnDir));

    private FileInfo MyBlazorLibraryAssemblyFile(string configuration, string targetFramework) =>
        new(Path.Combine(
            FakeSolutionDir().FullName, "My.BlazorLibrary", "bin", 
            configuration, targetFramework, "My.BlazorLibrary.fakedll"));
    
    [Test, TestCaseSource(nameof(Finds_StaticWebAssetsFile_In_FakeSolution_Obj_Dir_Cases))]
    public void Finds_StaticWebAssetsFile_In_FakeSolution_Obj_Dir(
        string configuration, string targetFramework)
    {
        // Arrange
        var assemblyFile = MyBlazorLibraryAssemblyFile(configuration, targetFramework);
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

    [Test]
    public void Can_Create_Composite_Provider()
    {
        // Arrange
        var assemblyFile = MyBlazorLibraryAssemblyFile("Debug", "net10.0");
        var pathInfo = InteractiveTestsProjectPathInfo.FromAssemblyFileInSolutionDir(
            assemblyFile, ".fakecsproj");

        var provider = AssetsAndTestProjectFileProviderFactory.Create(pathInfo);

        using (Assert.EnterMultipleScope())
        {
            var indexHtml = provider.GetFileInfo("index.html");
            Assert.That(indexHtml.Exists, Is.True);
            Assert.That(indexHtml, Is.Not.InstanceOf<NotFoundFileInfo>());

            var doesNotExist = provider.GetFileInfo("does-not-exist.html");
            Assert.That(doesNotExist.Exists, Is.False);
            Assert.That(doesNotExist, Is.InstanceOf<NotFoundFileInfo>());
            
            Assert.That(provider.GetFileInfo("data/test.json").Exists, Is.True);
            
            Assert.That(
                ReadAllText(provider.GetFileInfo("data/test.json")), 
                Does.Contain("fake-project-data"));
            
            var referencedLibraryCssFile =
                provider.GetFileInfo("_content/My.BlazorLibrary.Tests/lib.css");
            Assert.That(referencedLibraryCssFile.Exists, Is.True);
            Assert.That(ReadAllText(referencedLibraryCssFile), Does.Contain("referenced library"));
            
            var referencedLibraryJsDir =
                provider.GetDirectoryContents("_content/My.BlazorLibrary.Tests/js");
            Assert.That(referencedLibraryJsDir.Exists, Is.True);
            Assert.That(
                referencedLibraryJsDir.Select(file => file.Name),
                Contains.Item("widget.js"));
        }
    }

    private static string EscapeJsonPath(string path) => path.Replace("\\", "\\\\");

    private static string ReadAllText(IFileInfo fileInfo)
    {
        using var stream = fileInfo.CreateReadStream();
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}