using Dmnk.Blazor.Dialogs.Api;

namespace Dmnk.Blazor.Dialogs.SimpleMvvm;

public abstract class SimpleDialogViewModelBase : SimpleViewModelBase, IVmDialogViewModel
{
    public VmDialogReference Dialog { get => field; set => SetProperty(ref field, value); } = null!;
    
    public virtual void OnDismiss() { }
    public virtual Task OnDismissAsync() => Task.CompletedTask;
}