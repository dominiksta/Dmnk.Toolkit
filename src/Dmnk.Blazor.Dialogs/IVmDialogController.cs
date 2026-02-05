using Dmnk.Blazor.Dialogs.DefaultDialogs;
using Dmnk.Icons.Core;

namespace Dmnk.Blazor.Dialogs;

public interface IVmDialogController
{
    /// <summary>
    /// Show the dialog for a given viewmodel.
    /// <p>
    /// Before calling this, the component should be registered with its
    /// corresponding viewmodel.
    /// </p>
    /// </summary>
    public Task<VmDialogReference> Show<T>(
        VmDialogParameters parameters, T viewModel
    ) where T : IVmDialogViewModel;
    
    public Icon DefaultIconForIntent(MessageBoxType type);
}