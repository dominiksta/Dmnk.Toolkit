using CommunityToolkit.Mvvm.ComponentModel;
using Dmnk.Blazor.Dialogs.Api;

namespace Dmnk.Blazor.Dialogs.CommunityToolkit;

public class DialogViewModelBase : ObservableObject, IVmDialogViewModel
{
    public VmDialogReference Dialog { get; set; }
    
    public virtual void OnDismiss() {}
    public virtual Task OnDismissAsync() => Task.CompletedTask;
}