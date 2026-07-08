#if _WINDOWS
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.FileProviders;

namespace Dmnk.Blazor.InteractiveTests;

internal sealed class InteractiveTestBlazorWebView(InteractiveTestsProjectPathInfo pathInfo) : BlazorWebView
{
    public override IFileProvider CreateFileProvider(string contentRootDir)
    {
        IFileProvider defaultProvider = base.CreateFileProvider(contentRootDir);

        IFileProvider assetsAndTestProjectFileProvider =
            AssetsAndTestProjectFileProviderFactory.Create(pathInfo);
        
        var composite = new CompositeFileProvider(
            defaultProvider, 
            assetsAndTestProjectFileProvider);
        
        return composite;
    }
}
#endif