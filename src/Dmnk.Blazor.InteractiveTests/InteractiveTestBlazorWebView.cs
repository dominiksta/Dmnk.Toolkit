using System.Reflection;
using Dmnk.Blazor.InteractiveTests.Assets;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.FileProviders;

namespace Dmnk.Blazor.InteractiveTests;

internal sealed class InteractiveTestBlazorWebView(DirectoryInfo testProjectDir) : BlazorWebView
{
    public override IFileProvider CreateFileProvider(string contentRootDir)
    {
        var defaultProvider = base.CreateFileProvider(contentRootDir);
        
        var testProjectAssetsProvider = CreateStaticWebAssetFileProvider(testProjectDir);
        
        var assetsProjectAssetsProvider = 
            CreateStaticWebAssetFileProviderDebug(typeof(InteractiveTestsAssets).Assembly);

        if (!assetsProjectAssetsProvider.GetFileInfo("index.html").Exists) throw new FileNotFoundException(
            "index.html not found - file provider misconfigured?");
        
        var composite = new CompositeFileProvider(
            defaultProvider, 
            assetsProjectAssetsProvider,
            testProjectAssetsProvider);
        
        return composite;
    }

    private static ManifestStaticWebAssetFileProvider CreateStaticWebAssetFileProvider(
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
        
        return CreateStaticWebAssetFileProvider(runtimeFile);
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
    
    private static ManifestStaticWebAssetFileProvider CreateStaticWebAssetFileProviderDebug(
        Assembly projectAssembly)
    {
        
        string? assemblyDir = Path.GetDirectoryName(projectAssembly.Location);
        if (assemblyDir is null) throw new DirectoryNotFoundException(assemblyDir);
        
        string? projectName = projectAssembly.GetName().Name;
        if (projectName is null) throw new FileNotFoundException(assemblyDir);
        
        FileInfo runtimeFile = new FileInfo(Path.Combine(
            assemblyDir, $"{projectName}.staticwebassets.runtime.json"));
        
        return  CreateStaticWebAssetFileProvider(runtimeFile);
    }
    
    private static ManifestStaticWebAssetFileProvider CreateStaticWebAssetFileProviderObj(
        Assembly projectAssembly)
    {
        FileInfo csprojFile = BlazorInteractiveTests.GetCsProjForAssembly(projectAssembly);
        return  CreateStaticWebAssetFileProvider(csprojFile.Directory!);
    }
}
