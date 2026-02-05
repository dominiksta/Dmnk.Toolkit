namespace Dmnk.Blazor.Storage;

public class BrowserStorageItem<T>(string key, IStorageProvider storage)
{
    public ValueTask WriteAsync(T content, CancellationToken ct = default) 
        => storage.SetItemAsync<T>(key, content, ct);
    public ValueTask<T?> ReadAsync(CancellationToken ct = default) 
        => storage.GetItemAsync<T>(key, ct);
}

public class InProcessBrowserStorageItem<T>(string key, IInProcessStorageProvider storage)
    : BrowserStorageItem<T>(key, storage)
{
    private readonly string _key = key;
    
    public void Write(T content) => storage.SetItem<T>(_key, content);
    public T? Read() => storage.TryGetItem<T>(_key, out T ret) ? ret : default;
    public bool TryRead(out T result) => storage.TryGetItem<T>(_key, out result);
}