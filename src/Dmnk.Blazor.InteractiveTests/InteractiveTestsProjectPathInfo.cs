using System.Reflection;

namespace Dmnk.Blazor.InteractiveTests;

/// <summary>
/// Contains information about the test project, its configuration and target framework.
/// </summary>
public sealed record InteractiveTestsProjectPathInfo
{
    public required string TargetFrameworkDir { get; init; }
    public required string ConfigurationDir { get; init; }
    public required DirectoryInfo TestProjectDir { get; init; }
    public required string TestProjectName { get; init; }

    /// <summary>
    /// Constructs a <see cref="InteractiveTestsProjectPathInfo"/> instance from an assembly
    /// <b>that is located in an output folder in a solution</b>.
    /// <p>
    /// This will NOT work for assemblies that are outside a regular solution directory structure.
    /// </p>
    /// </summary>
    public static InteractiveTestsProjectPathInfo FromAssemblyInSolutionDir(Assembly assembly) => 
        FromAssemblyFileInSolutionDir(new FileInfo(assembly.Location));

    /// <summary>
    /// Like <see cref="FromAssemblyInSolutionDir"/>, but from a FileInfo pointing to an assembly
    /// instead of an assembly directly.
    /// <p>
    /// You typically shouldn't need to use this.
    /// </p>
    /// </summary>
    public static InteractiveTestsProjectPathInfo 
        FromAssemblyFileInSolutionDir(FileInfo assemblyFile) =>
        FromAssemblyFileInSolutionDir(assemblyFile, ".csproj");

    internal static InteractiveTestsProjectPathInfo FromAssemblyFileInSolutionDir(
        FileInfo assemblyFile, string csprojExt)
    {
        FileInfo? csprojFile = CsProjFileFinder.GetCsProjForAssembly(assemblyFile, csprojExt);
        if (csprojFile is not { Exists: true }) throw new FileNotFoundException(
            "csproj not found for assembly: " + assemblyFile.FullName);
        
        var testProjectDir = csprojFile.Directory!;
        
        if (testProjectDir is not { Exists: true })
            throw new DirectoryNotFoundException(testProjectDir.FullName);

        string[] split = assemblyFile.FullName.Split(Path.DirectorySeparatorChar);
        
        return new InteractiveTestsProjectPathInfo
        {
            TargetFrameworkDir = split[^2], 
            ConfigurationDir = split[^3],
            TestProjectDir = testProjectDir,
            TestProjectName = testProjectDir.Name,
        };
        
    }
}