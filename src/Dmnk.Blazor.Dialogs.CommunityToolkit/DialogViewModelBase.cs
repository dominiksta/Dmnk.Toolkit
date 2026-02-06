using CommunityToolkit.Mvvm.ComponentModel;
using Dmnk.Blazor.Dialogs.Api;

namespace Dmnk.Blazor.Dialogs.CommunityToolkit;

/// <summary> <inheritdoc cref="IVmDialogViewModel"/> </summary>
public class DialogViewModelBase : ObservableObject, IVmDialogViewModel
{
    /// <summary> <inheritdoc/> </summary>
    public VmDialogReference Dialog
    {
        get; 
        [Obsolete("Do NOT modify outside of custom DialogController implementations.")]
        set;
    } = null!; // will be set by the DialogController
    
    /// <summary> <inheritdoc/> </summary>
    public virtual void OnDismiss() {}
    /// <summary> <inheritdoc/> </summary>
    public virtual Task OnDismissAsync() => Task.CompletedTask;
}