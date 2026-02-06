namespace Dmnk.Blazor.Dialogs.Api;

public class VmDialogReference(Func<Task> onClose)
{
    private readonly TaskCompletionSource _close = new(1);
    
    /// <summary>
    /// Close and set <see cref="Cancelled"/> to true.
    /// OnDismissed(Async) will also be called before closing.
    /// </summary>
    public async Task Dismiss()
    {
        Cancelled = true;
        await Close();
    }
    
    /// <summary>
    /// Close without settings <see cref="Cancelled"/> to true.
    /// OnDismissed(Async) will also be called before closing.
    /// </summary>
    public async Task Close()
    {
        Closed = true;
        await onClose();
        _close.SetResult();
    }

    public Task WaitClosed => _close.Task;

    public bool Cancelled { get; private set; } = false;
    public bool Closed { get; private set; } = false;
}