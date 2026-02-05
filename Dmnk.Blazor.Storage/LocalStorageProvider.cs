using Microsoft.JSInterop;

namespace Dmnk.Blazor.Storage;

public sealed class LocalStorageProvider(IJSRuntime js) 
    : BrowserStorageProvider(js)
{
    protected override string StorageName => "localStorage";
}

public sealed class InProcessLocalStorageProvider(IJSInProcessRuntime js) 
    : InProcessBrowserStorageProvider(js)
{
    protected override string StorageName => "localStorage";
}
