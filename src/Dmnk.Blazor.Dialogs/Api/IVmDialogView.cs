using System.ComponentModel;

namespace Dmnk.Blazor.Dialogs.Api;

/// <summary>
/// Defines a view for a dialog without a specific ViewModel. This is used for dialogs that don't
/// need a ViewModel, or where the ViewModel is created internally by the view.
/// </summary>
public interface IVmDialogView
{
    /// <summary>
    /// A reference to the dialog, which can be used to close or dismiss it.
    /// </summary>
    public VmDialogReference Dialog { get; set; }
    /// <summary>
    /// Core parameters that apply to all dialogs.
    /// </summary>
    public VmDialogParameters Params { get; set; }
}

/// <summary>
/// Defines a view for a dialog with a specific ViewModel.
/// </summary>
/// <typeparam name="T">Type of the ViewModel</typeparam>
public interface IVmDialogViewFor<T> : IVmDialogView where T : INotifyPropertyChanged
{
    /// <summary> The ViewModel </summary>
    public T Vm { get; set; }
}