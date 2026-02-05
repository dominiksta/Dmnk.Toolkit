namespace Dmnk.Blazor.Storage;

public interface IStorageProvider
{
    public ValueTask ClearAsync(CancellationToken ct = default);
    public ValueTask ClearAsync(string key, CancellationToken ct = default);
    public ValueTask<List<string>> KeysAsync(CancellationToken ct = default);
    public ValueTask<bool> ContainsKeyAsync(string key, CancellationToken ct = default);
    public ValueTask<string?> GetItemAsync(string key, CancellationToken ct = default);
    public ValueTask<T?> GetItemAsync<T>(string key, CancellationToken ct = default);
    public ValueTask SetItemAsync(string key, string content, CancellationToken ct = default);
    public ValueTask SetItemAsync<T>(string key, T content, CancellationToken ct = default);
}

public interface IInProcessStorageProvider : IStorageProvider
{
    public void Clear();
    public string[] Keys();
    public bool ContainsKey(string key);
    public bool TryGetItem(string key, out string result);
    public bool TryGetItem<T>(string key, out T result);
    public void SetItem(string key, string content);
    public void SetItem<T>(string key, T content);
}