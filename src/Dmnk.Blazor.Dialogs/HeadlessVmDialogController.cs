using System.Drawing;
using Dmnk.Blazor.Dialogs.Api;
using Dmnk.Blazor.Dialogs.DefaultDialogs;
using Dmnk.Icons.Core;

namespace Dmnk.Blazor.Dialogs;

/// <summary>
/// Intended for unit tests: ViewModels are opened without an actual view.
/// </summary>
public class HeadlessVmDialogController : IVmDialogController
{
    private readonly List<(IVmDialogViewModel ViewModel, DateTime Opened)> _instances = [];

    /// <summary>
    /// Get the last opened instance of a viewmodel type. This is useful for unit tests to verify
    /// that a dialog was opened with the correct viewmodel and parameters.
    /// </summary>
    public T GetLastOpenedOfType<T>() =>
        (T) _instances
            .Where(i => i.ViewModel.GetType() == typeof(T))
            .OrderBy(i => i.Opened)
            .First().ViewModel;

    /// <summary> <inheritdoc/> </summary>
    public Task<VmDialogReference> Show<T>(
        VmDialogParameters parameters, T viewModel
    ) where T : IVmDialogViewModel
    {
        var toAdd = (viewModel, DateTime.Now);
        _instances.Add(toAdd);
        var reference = new VmDialogReference(async () =>
        {
            _instances.Remove(toAdd);
            // ReSharper disable once MethodHasAsyncOverload
            viewModel.OnDismiss();
            await viewModel.OnDismissAsync();
        });
#pragma warning disable CS0618 // Type or member is obsolete
        viewModel.Dialog = reference;
#pragma warning restore CS0618 // Type or member is obsolete
        // ReSharper disable once MethodHasAsyncOverload
        return Task.FromResult(reference);
    }

    /// <summary> <inheritdoc/> </summary>
    public Icon DefaultIconForIntent(MessageBoxType type)
    {
        return type switch
        {
            MessageBoxType.Info => MkIcon("Info"),
            MessageBoxType.Success => MkIcon("CheckmarkCircle"),
            MessageBoxType.Error => MkIcon("ErrorCircle"),
            MessageBoxType.Warning => MkIcon("Warning"),
            MessageBoxType.Confirmation => MkIcon("Question"),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
        
        Icon MkIcon(string name) => new(new PngIconDefinition(name), new Size(0, 0));
    }
}