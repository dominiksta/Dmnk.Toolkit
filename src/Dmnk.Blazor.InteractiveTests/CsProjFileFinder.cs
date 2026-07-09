using System.Reflection;

namespace Dmnk.Blazor.InteractiveTests;

internal static class CsProjFileFinder
{
    internal static FileInfo? GetCsProjForAssembly(
        FileInfo testProjectAssemblyFile, string extension = ".csproj")
    {
        DirectoryInfo? location  = testProjectAssemblyFile.Directory;
        FileInfo? csprojFile = null;
        
        while (location != null)
        {
            var found = location.EnumerateFiles()
                .Where(l => l.Extension == extension).ToList();
            if (found.Count > 1) throw new InvalidOperationException(
                $"Multiple csproj files found at {location.FullName}");
            
            if (found.Count == 1)
            {
                csprojFile = found.First();
                break;
            }
            
            location = location.Parent;
        }
        
        return csprojFile;
    }
    
    internal static FileInfo GetCsProjForAssembly(
        Assembly testProjectAssembly, string extension = ".csproj")
    {
        var name = testProjectAssembly.GetName().Name;
        
        var csprojFile = GetCsProjForAssembly(new FileInfo(testProjectAssembly.Location), extension);
        
        if (csprojFile is null) throw new DirectoryNotFoundException(
            $".csproj not found for assembly {name}");
        
        return csprojFile;
    }
}