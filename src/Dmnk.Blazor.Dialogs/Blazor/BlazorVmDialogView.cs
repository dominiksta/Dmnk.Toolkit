using System.ComponentModel;
using Dmnk.Blazor.Mvvm;
using Microsoft.AspNetCore.Components;

namespace Dmnk.Blazor.Dialogs.Blazor;

public abstract class BlazorVmDialogView : ComponentBase, IVmDialogView
{
    public abstract required VmDialogReference Dialog { get; set; }
    public abstract required VmDialogParameters Params { get; set; }
}

public abstract class BlazorVmDialogViewFor<T> : MvvmComponentBase<T>, IVmDialogViewFor<T>
    where T : INotifyPropertyChanged
{
    public abstract required VmDialogReference Dialog { get; set; }
    public abstract required VmDialogParameters Params { get; set; }
}