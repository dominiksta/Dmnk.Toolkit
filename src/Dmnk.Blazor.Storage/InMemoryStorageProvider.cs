using System.Collections.Concurrent;
using System.Text.Json;

namespace Dmnk.Blazor.Storage;

public class InMemoryStorageProvider : IInProcessStorageProvider
{
    private readonly ConcurrentDictionary<string, string> _storage = new();
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public void Clear() => _storage.Clear();
    public string[] Keys() => _storage.Keys.ToArray();
    public bool ContainsKey(string key) => _storage.ContainsKey(key);
    public bool TryGetItem(string key, out string result) => 
        _storage.TryGetValue(key, out result!);

    public bool TryGetItem<T>(string key, out T result)
    {
        if (TryGetItem(key, out var json))
        {
            var deserialized = JsonSerializer.Deserialize<T>(json, _jsonOptions);
            if (deserialized != null)
            {
                result = deserialized;
                return true;
            }
        }
        
        result = default!;
        return false;
    }

    public void SetItem(string key, string content) => _storage[key] = content;
    public void SetItem<T>(string key, T content) => SetItem(
        key, content is string str 
            ? str 
            : JsonSerializer.Serialize(content, _jsonOptions)
    );

    public ValueTask ClearAsync(CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        Clear();
        return ValueTask.CompletedTask;
    }

    public ValueTask ClearAsync(string key, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        _storage.TryRemove(key, out _);
        return ValueTask.CompletedTask;
    }

    public ValueTask<List<string>> KeysAsync(CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        return ValueTask.FromResult(_storage.Keys.ToList());
    }

    public ValueTask<bool> ContainsKeyAsync(string key, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        return ValueTask.FromResult(ContainsKey(key));
    }

    public ValueTask<string?> GetItemAsync(string key, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        var success = TryGetItem(key, out var result);
        return ValueTask.FromResult(success ? result : null);
    }

    public ValueTask<T?> GetItemAsync<T>(string key, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        var success = TryGetItem<T>(key, out var result);
        return ValueTask.FromResult(success ? result : default(T));
    }

    public ValueTask SetItemAsync(string key, string content, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        SetItem(key, content);
        return ValueTask.CompletedTask;
    }

    public ValueTask SetItemAsync<T>(string key, T content, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        SetItem(key, content);
        return ValueTask.CompletedTask;
    }
}
