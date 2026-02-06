namespace Dmnk.Blazor.Dialogs.Api;

/// <summary>
/// Holds a reference to an open dialog, allowing, among other things, to close or dismiss it from
/// the ViewModel.
/// </summary>
/// <param name="onClose"></param>
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

    /// <summary>
    /// A task that completes when the dialog is closed, either by calling <see cref="Close"/> or
    /// <see cref="Dismiss"/>.
    /// </summary>
    public Task WaitClosed => _close.Task;

    /// <summary>
    /// Whether the dialog was dismissed by calling <see cref="Dismiss"/>.
    /// </summary>
    public bool Cancelled { get; private set; } = false;
    
    /// <summary>
    /// Whether the dialog was closed by calling <see cref="Close"/> or <see cref="Dismiss"/>.
    /// </summary>
    public bool Closed { get; private set; } = false;
}