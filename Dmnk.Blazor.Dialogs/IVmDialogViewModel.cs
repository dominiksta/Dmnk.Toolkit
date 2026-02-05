using System.ComponentModel;

namespace Dmnk.Blazor.Dialogs;

public interface IVmDialogViewModel : INotifyPropertyChanged
{
    protected internal VmDialogReference Dialog { get; internal set; }
    
    public void OnDismiss();
    public Task OnDismissAsync();
}