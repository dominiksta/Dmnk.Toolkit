#if _WINDOWS
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.FileProviders;

namespace Dmnk.Blazor.InteractiveTests;

internal sealed class InteractiveTestBlazorWebView(DirectoryInfo testProjectDir) : BlazorWebView
{
    public override IFileProvider CreateFileProvider(string contentRootDir)
    {
        IFileProvider defaultProvider = base.CreateFileProvider(contentRootDir);

        IFileProvider assetsAndTestProjectFileProvider =
            AssetsAndTestProjectFileProviderFactory.Create(testProjectDir);
        
        var composite = new CompositeFileProvider(
            defaultProvider, 
            assetsAndTestProjectFileProvider);
        
        return composite;
    }
}
#endif