using Microsoft.JSInterop;

namespace Dmnk.Blazor.Storage;

/// <summary>
/// Implementation of <see cref="IStorageProvider"/> for the browser's localStorage API.
/// </summary>
public sealed class LocalStorageProvider(IJSRuntime js) 
    : BrowserStorageProvider(js)
{
    /// <summary> <inheritdoc/> </summary>
    protected override string StorageName => "localStorage";
}

/// <summary>
/// In-process (aka synchronous) version of <see cref="LocalStorageProvider"/>.
/// </summary>
public sealed class InProcessLocalStorageProvider(IJSInProcessRuntime js) 
    : InProcessBrowserStorageProvider(js)
{
    /// <summary> <inheritdoc/> </summary>
    protected override string StorageName => "localStorage";
}
