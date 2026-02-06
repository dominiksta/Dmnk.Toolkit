namespace Dmnk.Blazor.Storage;

/// <summary>
/// (Very) basic wrapper for a single item in an <see cref="IStorageProvider"/>, providing typed
/// read/write operations.
/// </summary>
public class BrowserStorageItem<T>(string key, IStorageProvider storage)
{
    /// <summary>
    /// Writes the given content to the storage, under the key specified in the constructor.
    /// </summary>
    public ValueTask WriteAsync(T content, CancellationToken ct = default) 
        => storage.SetItemAsync<T>(key, content, ct);
    
    /// <summary>
    /// Reads the content from the storage, using the key specified in the constructor.
    /// </summary>
    public ValueTask<T?> ReadAsync(CancellationToken ct = default) 
        => storage.GetItemAsync<T>(key, ct);
}

/// <summary>
/// In-process (aka synchronous) version of <see cref="BrowserStorageItem{T}"/>.
/// </summary>
public class InProcessBrowserStorageItem<T>(string key, IInProcessStorageProvider storage)
    : BrowserStorageItem<T>(key, storage)
{
    private readonly string _key = key;
    
    /// <summary>
    /// Sync version of <see cref="BrowserStorageItem{T}.WriteAsync(T, CancellationToken)"/>.
    /// </summary>
    public void Write(T content) => storage.SetItem<T>(_key, content);
    
    /// <summary>
    /// Sync version of <see cref="BrowserStorageItem{T}.ReadAsync(CancellationToken)"/>.
    /// </summary>
    public T? Read() => storage.TryGetItem<T>(_key, out T ret) ? ret : default;
    
    /// <summary>
    /// Analogous to <see cref="IDictionary{TKey,TValue}.TryGetValue"/>
    /// </summary>
    public bool TryRead(out T result) => storage.TryGetItem<T>(_key, out result);
}