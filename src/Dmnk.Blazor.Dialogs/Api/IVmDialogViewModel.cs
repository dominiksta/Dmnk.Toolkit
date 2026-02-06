using System.ComponentModel;

namespace Dmnk.Blazor.Dialogs.Api;

/// <summary>
/// Defines an MVVM ViewModel for a dialog. Basically just a regular ViewModel, except it holds a
/// reference to the dialog and can react to dismissal by the user.
/// </summary>
public interface IVmDialogViewModel : INotifyPropertyChanged
{
    /// <summary> A reference to the dialog. </summary>
    public VmDialogReference Dialog
    {
        get; 
        [Obsolete("Do NOT modify outside of custom DialogController implementations.")]
        set;
    }
    
    /// <summary>
    /// Called when the dialog is dismissed by the user, e.g. by clicking outside the dialog or by
    /// clicking a cancel button. This is called before the dialog is actually closed, so you can
    /// e.g. still do some cleanup work.
    /// </summary>
    public void OnDismiss();
    
    /// <summary>
    /// Async version of <see cref="OnDismiss"/>
    /// </summary>
    public Task OnDismissAsync();
}