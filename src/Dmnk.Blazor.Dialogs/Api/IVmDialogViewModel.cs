using System.ComponentModel;

namespace Dmnk.Blazor.Dialogs.Api;

public interface IVmDialogViewModel : INotifyPropertyChanged
{
    public VmDialogReference Dialog
    {
        get; 
        [Obsolete("Do NOT modify outside of custom DialogController implementations.")]
        set;
    }
    
    public void OnDismiss();
    public Task OnDismissAsync();
}