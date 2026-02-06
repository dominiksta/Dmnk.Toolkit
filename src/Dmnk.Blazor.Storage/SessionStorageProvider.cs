using Microsoft.JSInterop;

namespace Dmnk.Blazor.Storage;

/// <summary>
/// Implementation of <see cref="IStorageProvider"/> for the browser's sessionStorage API.
/// </summary>
public sealed class SessionStorageProvider(IJSRuntime js) 
    : BrowserStorageProvider(js)
{
    /// <summary> <inheritdoc/> </summary>
    protected override string StorageName => "sessionStorage";
}

/// <summary>
/// In-process (aka synchronous) version of <see cref="SessionStorageProvider"/>.
/// </summary>
public sealed class InProcessSessionStorageProvider(IJSInProcessRuntime js) 
    : InProcessBrowserStorageProvider(js)
{
    /// <summary> <inheritdoc/> </summary>
    protected override string StorageName => "sessionStorage";
}
