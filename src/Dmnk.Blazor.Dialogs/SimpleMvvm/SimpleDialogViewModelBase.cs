using Dmnk.Blazor.Dialogs.Api;

namespace Dmnk.Blazor.Dialogs.SimpleMvvm;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Internal basic MVVM implementation, just to not depend on e.g. the CommunityToolkit in the core
/// library. Not intended for public use, use a proper MVVM library.
/// </summary>
public abstract class SimpleDialogViewModelBase : SimpleViewModelBase, IVmDialogViewModel
{
    public VmDialogReference Dialog { get; set => SetProperty(ref field, value); } = null!;
    
    public virtual void OnDismiss() { }
    public virtual Task OnDismissAsync() => Task.CompletedTask;
}