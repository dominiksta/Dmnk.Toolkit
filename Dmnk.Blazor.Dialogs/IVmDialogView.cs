using System.ComponentModel;

namespace Dmnk.Blazor.Dialogs;

public interface IVmDialogView
{
    public VmDialogReference Dialog { get; set; }
    public VmDialogParameters Params { get; set; }
}

public interface IVmDialogViewFor<T> : IVmDialogView where T : INotifyPropertyChanged
{
    /// <summary> The ViewModel </summary>
    public T Vm { get; set; }
}