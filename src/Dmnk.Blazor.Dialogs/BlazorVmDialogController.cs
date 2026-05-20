using Dmnk.Blazor.Dialogs.Api;
using Dmnk.Blazor.Dialogs.DefaultDialogs;
using Dmnk.Icons.Core;

namespace Dmnk.Blazor.Dialogs;

/// <summary>
/// A base class for an implementation of <see cref="IVmDialogController"/>.
/// Dialog views are resolved via DI using <see cref="Dmnk.Blazor.Mvvm.IViewModelRegistry"/>.
/// </summary>
public abstract class BlazorVmDialogController : IVmDialogController
{
    /// <summary>
    /// Fired whenever a dialog is shown using <see cref="Show{T}"/>. You typically won't need to
    /// use this, unless you are implementing a new dialog controller or provider.
    /// </summary>
    public event Action<(
        VmDialogParameters Parameters,
        IVmDialogViewModel ViewModel, VmDialogReference Reference
    )>? OnShow;

    /// <summary>
    /// Fired whenever a dialog is closed, either by calling <see cref="VmDialogReference.Close"/>
    /// or <see cref="VmDialogReference.Dismiss"/>. You typically won't need to use this, unless you
    /// are implementing a new dialog controller or provider.
    /// </summary>
    public event Action<IVmDialogViewModel>? OnClose;
    
    /// <summary> <inheritdoc/> </summary>
    public Task<VmDialogReference> Show<T>(VmDialogParameters parameters, T viewModel) 
        where T : IVmDialogViewModel
    {
        var reference = new VmDialogReference(async () =>
        {
            // ReSharper disable once MethodHasAsyncOverload
            viewModel.OnDismiss();
            await viewModel.OnDismissAsync();
            OnClose?.Invoke(viewModel);
        });
#pragma warning disable CS0618 // Type or member is obsolete
        viewModel.Dialog = reference;
#pragma warning restore CS0618 // Type or member is obsolete
        OnShow?.Invoke((parameters, viewModel, reference));
        // ReSharper disable once MethodHasAsyncOverload
        return Task.FromResult(reference);
    }

    /// <summary> <inheritdoc/> </summary>
    public abstract Icon DefaultIconForIntent(MessageBoxType type);
}