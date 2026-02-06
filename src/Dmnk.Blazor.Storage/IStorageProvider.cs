namespace Dmnk.Blazor.Storage;

/// <summary>
/// Defines a (browser) storage api such as localStorage or sessionStorage. Fundamentally, this is
/// just a simple key-value store.
/// </summary>
public interface IStorageProvider
{
    /// <summary> Clear all keys from the storage. </summary>
    public ValueTask ClearAsync(CancellationToken ct = default);
    /// <summary>
    /// Clear a specific key from the storage.
    /// </summary>
    /// <exception cref="KeyNotFoundException">The given key does not exist</exception>
    public ValueTask ClearAsync(string key, CancellationToken ct = default);
    /// <summary>
    /// List all keys currently in the storage.
    /// </summary>
    public ValueTask<List<string>> KeysAsync(CancellationToken ct = default);
    /// <summary>
    /// Check if the storage contains a specific key.
    /// </summary>
    public ValueTask<bool> ContainsKeyAsync(string key, CancellationToken ct = default);
    /// <summary>
    /// Get the string value of a specific key from the storage.
    /// Returns null if the key does not exist.
    /// </summary>
    public ValueTask<string?> GetItemAsync(string key, CancellationToken ct = default);
    /// <summary>
    /// Get the value of a specific key from the storage, deserialized with System.Text.Json as T.
    /// </summary>
    public ValueTask<T?> GetItemAsync<T>(string key, CancellationToken ct = default);
    /// <summary>
    /// Set the string value of a specific key in the storage. If the key already exists, it will be
    /// overwritten.
    /// </summary>
    public ValueTask SetItemAsync(string key, string content, CancellationToken ct = default);
    /// <summary>
    /// Set the value of a specific key in the storage, serialized with System.Text.Json. If the key
    /// already exists, it will be overwritten.
    /// </summary>
    public ValueTask SetItemAsync<T>(string key, T content, CancellationToken ct = default);
}

/// <summary>
/// Like <see cref="IStorageProvider"/>, but allows for synchronous access to the storage. This is
/// only possible if the underlying storage provider supports it, such as when using the
/// IJSInProcessRuntime to access browser storage.
/// </summary>
public interface IInProcessStorageProvider : IStorageProvider
{
    /// <summary>
    /// Sync version of <see cref="IStorageProvider.ClearAsync(CancellationToken)"/>
    /// </summary>
    public void Clear();
    /// <summary>
    /// Sync version of <see cref="IStorageProvider.ClearAsync(string, CancellationToken)"/>
    /// </summary>
    public void Clear(string key);
    /// <summary>
    /// Sync version of <see cref="IStorageProvider.KeysAsync(CancellationToken)"/>
    /// </summary>
    public List<string> Keys();
    /// <summary>
    /// Sync version of <see cref="IStorageProvider.ContainsKeyAsync(string, CancellationToken)"/>
    /// </summary>
    public bool ContainsKey(string key);
    /// <summary>
    /// Get the string value of a specific key from the storage. Returns false if the key does not
    /// exist.
    /// </summary>
    public bool TryGetItem(string key, out string result);
    /// <summary>
    /// Get the value of a specific key from the storage, deserialized with System.Text.Json as T.
    /// Returns false if the key does not exist or deserialization fails.
    /// </summary>
    public bool TryGetItem<T>(string key, out T result);
    /// <summary>
    /// Sync version of <see cref="IStorageProvider.SetItemAsync(string, string, CancellationToken)"/>
    /// </summary>
    public void SetItem(string key, string content);
    /// <summary>
    /// Sync version of <see cref="IStorageProvider.SetItemAsync{T}(string, T, CancellationToken)"/>
    /// </summary>
    public void SetItem<T>(string key, T content);
}