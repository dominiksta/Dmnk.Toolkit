namespace Dmnk.Blazor.InteractiveTests.Tests;

[TestFixture]
public class CsProjFileFinderTests
{
    [Test]
    public void Finds_CsProjFile_Of_This_Assembly()
    {
        // Arrange
        const string project = "Dmnk.Blazor.InteractiveTests.Tests";
        
        // Act
        var csprojFile = CsProjFileFinder.GetCsProjForAssembly(GetType().Assembly);
        
        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(csprojFile, Is.Not.Null);
            Assert.That(csprojFile.Name, Is.EqualTo($"{project}.csproj"));
            Assert.That(csprojFile.FullName, 
                Does.EndWith(Path.Combine(project, $"{project}.csproj")));
        }
    }

    [Test]
    public void Finds_CsProjFile_In_Fake_Solution()
    {
        // Arrange
        const string project = "My.BlazorLibrary";
        const string ext = ".fakecsproj";
        string binDir = new FileInfo(GetType().Assembly.Location).DirectoryName!;
        
        // Act
        var csprojFile = CsProjFileFinder.GetCsProjForAssembly(
            new FileInfo(Path.Combine(
                binDir, Const.FakeSlnDir, "My.BlazorLibrary", 
                "bin", "Debug", "net10.0", "My.BlazorLibrary.fakedll")),
            extension: ext);
        
        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(csprojFile, Is.Not.Null);
            Assert.That(csprojFile.Name, Is.EqualTo($"{project}{ext}"));
            Assert.That(csprojFile.FullName,
                Does.EndWith(Path.Combine(project, $"{project}{ext}")));
        }
    }
}