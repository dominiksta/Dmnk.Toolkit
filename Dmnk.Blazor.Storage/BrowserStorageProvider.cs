using System.Text.Json;
using Microsoft.JSInterop;

namespace Dmnk.Blazor.Storage;

public abstract class BrowserStorageProvider(IJSRuntime js) : IStorageProvider
{
    protected abstract string StorageName { get; }

    public ValueTask ClearAsync(CancellationToken ct = default) 
        => js.InvokeVoidAsync("localStorage.clear", ct);
    
    public async ValueTask ClearAsync(string key, CancellationToken ct = default)
    {
        if (!await ContainsKeyAsync(key, ct)) throw new KeyNotFoundException(key);
        await js.InvokeVoidAsync("localStorage.removeItem", ct, key);
    }

    public ValueTask<List<string>> KeysAsync(CancellationToken ct = default) 
        => js.InvokeAsync<List<string>>("eval", ct, $"Object.keys({StorageName})");

    public async ValueTask<bool> ContainsKeyAsync(string key, CancellationToken ct = default)
        => await GetItemAsync(key, ct) != null;

    public ValueTask<string?> GetItemAsync(string key, CancellationToken ct = default) 
        => js.InvokeAsync<string?>($@"{StorageName}.getItem", ct, key);

    public async ValueTask<T?> GetItemAsync<T>(string key, CancellationToken ct = default)
    {
        var str = await GetItemAsync(key, ct);
        return str == null ? default : JsonSerializer.Deserialize<T>(str);
    }

    public ValueTask SetItemAsync(string key, string content, CancellationToken ct = default) 
        => js.InvokeVoidAsync($"{StorageName}.setItem", ct, key, content);

    public ValueTask SetItemAsync<T>(string key, T content, CancellationToken ct = default) 
        => SetItemAsync(key, JsonSerializer.Serialize(content), ct);
}

public abstract class InProcessBrowserStorageProvider(IJSInProcessRuntime js) 
    : BrowserStorageProvider(js), IInProcessStorageProvider
{
    public void Clear() => js.InvokeVoid("localStorage.clear");
    
    public string[] Keys() => js.Invoke<string[]>("eval", $"Object.keys({StorageName})");

    public bool ContainsKey(string key) => TryGetItem(key, out var _);

    public bool TryGetItem(string key, out string result)
    {
        result = "";
        var found = js.Invoke<string?>($"{StorageName}.getItem", key);
        if (found == null) return false;
        result = found;
        return true;
    }

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

    public void SetItem(string key, string content) 
        => js.InvokeVoid($"{StorageName}.setItem", key, content);

    public void SetItem<T>(string key, T content) 
        => SetItem(key, JsonSerializer.Serialize(content));
}