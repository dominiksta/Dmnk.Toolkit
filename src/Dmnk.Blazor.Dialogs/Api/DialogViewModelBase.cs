using CommunityToolkit.Mvvm.ComponentModel;

namespace Dmnk.Blazor.Dialogs.Api;

public abstract class DialogViewModelBase : ObservableObject, IVmDialogViewModel
{
    public VmDialogReference Dialog { get; set; } = null!;
    
    public virtual void OnDismiss() { }
    public virtual Task OnDismissAsync() => Task.CompletedTask;
}