using Microsoft.Extensions.FileProviders;

namespace Dmnk.Blazor.InteractiveTests;

internal static class AssetsAndTestProjectFileProviderFactory
{
    public static IFileProvider Create(InteractiveTestsProjectPathInfo pathInfo)
    {
        var testProjectAssetsProvider = 
            CreateStaticWebAssetFileProvider(
                FindStaticWebAssetFileFromObjDir(pathInfo));

        var thisAsm = typeof(AssetsAndTestProjectFileProviderFactory).Assembly;

        var assetsProjectAssetsProvider = new EmbeddedFileProvider(
            thisAsm, $"{thisAsm.GetName().Name}.wwwroot");
        
        if (!assetsProjectAssetsProvider.GetFileInfo("index.html").Exists) throw new FileNotFoundException(
            "index.html not found - file provider misconfigured?");

        return new CompositeFileProvider(assetsProjectAssetsProvider, testProjectAssetsProvider);
    }
    
    private static ManifestStaticWebAssetFileProvider CreateStaticWebAssetFileProvider(
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
    
    internal static FileInfo FindStaticWebAssetFileFromObjDir(
        InteractiveTestsProjectPathInfo pathInfo)
    {
        const string webAssetsFile = "staticwebassets.development.json";
        
        var objDir =  new DirectoryInfo(Path.Combine(pathInfo.TestProjectDir.FullName, "obj"));
        if (objDir is not { Exists: true }) throw new DirectoryNotFoundException(objDir.FullName);

        var fullObjDir = new DirectoryInfo(Path.Combine(objDir.FullName, 
            pathInfo.ConfigurationDir, pathInfo.TargetFrameworkDir));
        
        if (!fullObjDir.Exists) throw new DirectoryNotFoundException(fullObjDir.FullName);

        string[] found = Directory.GetFiles(
            fullObjDir.FullName, webAssetsFile, SearchOption.AllDirectories);

        if (found.Length == 0) throw new FileNotFoundException(webAssetsFile);
        if (found.Length > 1) throw new InvalidOperationException(
            $"Found multiple of {webAssetsFile}: {string.Join(", ", found)}");
        
        FileInfo runtimeFile = new FileInfo(found[0]);
        if (!runtimeFile.Exists) throw new FileNotFoundException(runtimeFile.FullName);
        
        return runtimeFile;
    }
}