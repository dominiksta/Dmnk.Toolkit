using Dmnk.Blazor.Dialogs.DefaultDialogs;
using Dmnk.Icons.Core;

namespace Dmnk.Blazor.Dialogs.Api;

/// <summary>
/// The entry point for application code to show dialogs.
/// </summary>
public interface IVmDialogController
{
    /// <summary>
    /// Show the dialog for a given viewmodel.
    /// <p>
    /// Before calling this, the component should be registered with its
    /// corresponding viewmodel using e.g.
    /// <see cref="BlazorVmDialogController.Register{TComponent, TViewModel}"/>.
    /// </p>
    /// </summary>
    public Task<VmDialogReference> Show<T>(
        VmDialogParameters parameters, T viewModel
    ) where T : IVmDialogViewModel;
    
    /// <summary>
    /// Return the default icon for a given message box type. This can be used by the <see
    /// cref="MessageBoxViewModel"/> and other similar dialogs to get the appropriate icon for the
    /// type of message box they are showing.
    /// </summary>
    public Icon DefaultIconForIntent(MessageBoxType type);
}