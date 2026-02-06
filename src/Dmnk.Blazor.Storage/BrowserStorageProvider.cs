using System.Text.Json;
using Microsoft.JSInterop;

namespace Dmnk.Blazor.Storage;

/// <summary>
/// Base class for browser storage providers,
/// i.e. <see cref="LocalStorageProvider"/> and <see cref="SessionStorageProvider"/>.
/// </summary>
public abstract class BrowserStorageProvider(IJSRuntime js) : IStorageProvider
{
    /// <summary>
    /// The JavaScript name of the storage, e.g. "localStorage" or "sessionStorage".
    /// Used for executing the correct JS functions.
    /// </summary>
    protected abstract string StorageName { get; }

    /// <summary> <inheritdoc/> </summary>
    public ValueTask ClearAsync(CancellationToken ct = default) 
        => js.InvokeVoidAsync("localStorage.clear", ct);
    
    /// <summary> <inheritdoc/> </summary>
    public async ValueTask ClearAsync(string key, CancellationToken ct = default)
    {
        if (!await ContainsKeyAsync(key, ct)) throw new KeyNotFoundException(key);
        await js.InvokeVoidAsync("localStorage.removeItem", ct, key);
    }

    /// <summary> <inheritdoc/> </summary>
    public ValueTask<List<string>> KeysAsync(CancellationToken ct = default) 
        => js.InvokeAsync<List<string>>("eval", ct, $"Object.keys({StorageName})");

    /// <summary> <inheritdoc/> </summary>
    public async ValueTask<bool> ContainsKeyAsync(string key, CancellationToken ct = default)
        => await GetItemAsync(key, ct) != null;

    /// <summary> <inheritdoc/> </summary>
    public ValueTask<string?> GetItemAsync(string key, CancellationToken ct = default) 
        => js.InvokeAsync<string?>($@"{StorageName}.getItem", ct, key);

    /// <summary> <inheritdoc/> </summary>
    public async ValueTask<T?> GetItemAsync<T>(string key, CancellationToken ct = default)
    {
        var str = await GetItemAsync(key, ct);
        return str == null ? default : JsonSerializer.Deserialize<T>(str);
    }

    /// <summary> <inheritdoc/> </summary>
    public ValueTask SetItemAsync(string key, string content, CancellationToken ct = default) 
        => js.InvokeVoidAsync($"{StorageName}.setItem", ct, key, content);

    /// <summary> <inheritdoc/> </summary>
    public ValueTask SetItemAsync<T>(string key, T content, CancellationToken ct = default) 
        => SetItemAsync(key, JsonSerializer.Serialize(content), ct);
}

/// <summary>
/// In-process (aka synchronous) version of <see cref="BrowserStorageProvider"/>.
/// </summary>
public abstract class InProcessBrowserStorageProvider(IJSInProcessRuntime js) 
    : BrowserStorageProvider(js), IInProcessStorageProvider
{
    /// <summary> <inheritdoc/> </summary>
    public void Clear() => js.InvokeVoid("localStorage.clear");
    
    /// <summary> <inheritdoc/> </summary>
    public void Clear(string key) => js.InvokeVoid("localStorage.removeItem", key);
    
    /// <summary> <inheritdoc/> </summary>
    public List<string> Keys() => js.Invoke<List<string>>("eval", $"Object.keys({StorageName})");

    /// <summary> <inheritdoc/> </summary>
    public bool ContainsKey(string key) => TryGetItem(key, out var _);

    /// <summary> <inheritdoc/> </summary>
    public bool TryGetItem(string key, out string result)
    {
        result = "";
        var found = js.Invoke<string?>($"{StorageName}.getItem", key);
        if (found == null) return false;
        result = found;
        return true;
    }

    /// <summary> <inheritdoc/> </summary>
    public bool TryGetItem<T>(string key, out T result)
    {
        var found = TryGetItem(key, out var resultStr);
        if (found)
        {
            result = JsonSerializer.Deserialize<T>(resultStr)!;
            return true;
        }
        result = default!;
        return false;
    }

    /// <summary> <inheritdoc/> </summary>
    public void SetItem(string key, string content) 
        => js.InvokeVoid($"{StorageName}.setItem", key, content);

    /// <summary> <inheritdoc/> </summary>
    public void SetItem<T>(string key, T content) 
        => SetItem(key, JsonSerializer.Serialize(content));
}