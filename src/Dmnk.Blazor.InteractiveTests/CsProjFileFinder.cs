using System.Reflection;

namespace Dmnk.Blazor.InteractiveTests;

internal static class CsProjFileFinder
{
    internal static FileInfo GetCsProjForAssembly(Assembly testProjectAssembly)
    {
        var name = testProjectAssembly.GetName().Name;
        DirectoryInfo? location  = new FileInfo(testProjectAssembly.Location).Directory;
        FileInfo? csprojFile = null;
        
        while (location != null)
        {
            var found = location.EnumerateFiles("*.csproj").ToList();
            if (found.Count > 1) throw new InvalidOperationException(
                $"Multiple csproj files found at {location.FullName}");
            
            if (found.Count == 1)
            {
                csprojFile = found.First();
                break;
            }
            
            location = location.Parent;
        }
        
        if (csprojFile is null) throw new DirectoryNotFoundException(
            $".csproj not found for assembly {name}");
        
        return csprojFile;
    }
}