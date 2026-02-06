using System.ComponentModel;
using Dmnk.Blazor.Mvvm;
using Microsoft.AspNetCore.Components;

namespace Dmnk.Blazor.Dialogs.Api;

/// <summary>
/// Concrete implementation of <see cref="IVmDialogView"/>:
/// <inheritdoc cref="IVmDialogView"/>
/// </summary>
public abstract class BlazorVmDialogView : ComponentBase, IVmDialogView
{
    /// <summary> <inheritdoc/> </summary>
    public abstract required VmDialogReference Dialog { get; set; }
    /// <summary> <inheritdoc/> </summary>
    public abstract required VmDialogParameters Params { get; set; }
}

/// <summary>
/// Concrete implementation of <see cref="IVmDialogViewFor{T}"/>:
/// <inheritdoc cref="IVmDialogViewFor{T}"/>
/// </summary>
public abstract class BlazorVmDialogViewFor<T> : MvvmComponentBase<T>, IVmDialogViewFor<T>
    where T : INotifyPropertyChanged
{
    /// <summary> <inheritdoc/> </summary>
    public abstract required VmDialogReference Dialog { get; set; }
    /// <summary> <inheritdoc/> </summary>
    public abstract required VmDialogParameters Params { get; set; }
}