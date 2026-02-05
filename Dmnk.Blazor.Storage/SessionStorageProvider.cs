using Microsoft.JSInterop;

namespace Dmnk.Blazor.Storage;

public sealed class SessionStorageProvider(IJSRuntime js) 
    : BrowserStorageProvider(js)
{
    protected override string StorageName => "sessionStorage";
}

public sealed class InProcessSessionStorageProvider(IJSInProcessRuntime js) 
    : InProcessBrowserStorageProvider(js)
{
    protected override string StorageName => "sessionStorage";
}
