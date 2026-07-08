using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace Dmnk.Blazor.InteractiveTests;

internal static class AssetsAndTestProjectFileProviderFactory
{
    public static IFileProvider Create(DirectoryInfo testProjectDir)
    {
        var testProjectAssetsProvider = CreateStaticWebAssetFileProviderFromObjDir(testProjectDir);
        
        var assetsProjectAssetsProvider = CreateStaticWebAssetFileProviderNextToAssembly(
            typeof(AssetsAndTestProjectFileProviderFactory).Assembly);

        if (!assetsProjectAssetsProvider.GetFileInfo("index.html").Exists) throw new FileNotFoundException(
            "index.html not found - file provider misconfigured?");

        return new CompositeFileProvider(assetsProjectAssetsProvider, testProjectAssetsProvider);
    }
    
    private static ManifestStaticWebAssetFileProvider CreateStaticWebAssetFileProviderFromObjDir(
        DirectoryInfo projectDir)
    {
        const string webAssetsFile = "staticwebassets.development.json";
        
        var objDir =  new DirectoryInfo(Path.Combine(projectDir.FullName, "obj"));
        if (objDir is not { Exists: true }) throw new DirectoryNotFoundException(objDir.FullName);

        string[] found = Directory.GetFiles(
            objDir.FullName, webAssetsFile, SearchOption.AllDirectories);

        if (found.Length == 0) throw new FileNotFoundException(webAssetsFile);
        if (found.Length > 1) throw new InvalidOperationException($"Found multiple of {webAssetsFile}");
        
        FileInfo runtimeFile = new FileInfo(found[0]);
        if (!runtimeFile.Exists) throw new FileNotFoundException(runtimeFile.FullName);
        
        return CreateStaticWebAssetFileProviderFromFile(runtimeFile);
    }
    
    private static ManifestStaticWebAssetFileProvider CreateStaticWebAssetFileProviderFromFile(
        FileInfo runtimeFile)
    {
        if (runtimeFile is not { Exists: true }) 
            throw new FileNotFoundException(runtimeFile.FullName);
        
        using var runtimeStream = runtimeFile.OpenRead();
            
        var staticWebAssetManifest = 
            ManifestStaticWebAssetFileProvider.StaticWebAssetManifest.Parse(runtimeStream);

        var staticWebAssetManifestProvider = new ManifestStaticWebAssetFileProvider(
            staticWebAssetManifest, path => new PhysicalFileProvider(path));

        return staticWebAssetManifestProvider;
    }
    
    private static ManifestStaticWebAssetFileProvider 
        CreateStaticWebAssetFileProviderNextToAssembly(Assembly projectAssembly)
    {
        
        string? assemblyDir = Path.GetDirectoryName(projectAssembly.Location);
        if (assemblyDir is null) throw new DirectoryNotFoundException(assemblyDir);
        
        string? projectName = projectAssembly.GetName().Name;
        if (projectName is null) throw new FileNotFoundException(assemblyDir);
        
        FileInfo runtimeFile = new FileInfo(Path.Combine(
            assemblyDir, $"{projectName}.staticwebassets.runtime.json"));
        
        return  CreateStaticWebAssetFileProviderFromFile(runtimeFile);
    }
}