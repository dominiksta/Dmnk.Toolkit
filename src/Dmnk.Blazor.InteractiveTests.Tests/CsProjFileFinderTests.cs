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

    [Test]
    public void Returns_Null_When_No_Project_File_Can_Be_Found()
    {
        using var temp = new TemporaryDirectory();
        var assemblyFile = temp.CreateFile(Path.Combine("bin", "Debug", "net10.0", "NoProject.dll"));

        var csprojFile = CsProjFileFinder.GetCsProjForAssembly(assemblyFile, ".fakecsproj");

        Assert.That(csprojFile, Is.Null);
    }

    [Test]
    public void Throws_When_Multiple_Project_Files_Exist_In_Same_Directory()
    {
        using var temp = new TemporaryDirectory();
        temp.CreateFile(Path.Combine("Project", "A.fakecsproj"));
        temp.CreateFile(Path.Combine("Project", "B.fakecsproj"));
        var assemblyFile = temp.CreateFile(Path.Combine(
            "Project", "bin", "Debug", "net10.0", "MultipleProjects.dll"));

        Assert.That(
            () => CsProjFileFinder.GetCsProjForAssembly(assemblyFile, ".fakecsproj"),
            Throws.InstanceOf<InvalidOperationException>()
                .With.Message.Contains("Multiple csproj files found"));
    }

    [Test]
    public void Assembly_Overload_Uses_Given_Extension()
    {
        Assert.That(
            () => CsProjFileFinder.GetCsProjForAssembly(GetType().Assembly, ".fakecsproj"),
            Throws.InstanceOf<DirectoryNotFoundException>()
                .With.Message.Contains(".csproj not found"));
    }
}