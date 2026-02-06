using System.Drawing;
using Dmnk.Blazor.Dialogs.Api;
using Dmnk.Blazor.Dialogs.DefaultDialogs;
using Dmnk.Icons.Core;

namespace Dmnk.Blazor.Dialogs;

/// <summary>
/// Intended for unit tests: ViewModels are registered and opened without an
/// actual view.
/// </summary>
public class HeadlessVmDialogController : IVmDialogController
{
    private readonly HashSet<Type> _viewModels = new();
    private readonly List<(IVmDialogViewModel ViewModel, DateTime Opened)> _instances = [];

    /// <summary> See <see cref="HeadlessVmDialogController"/> </summary>
    public HeadlessVmDialogController()
    {
        Register(typeof(InputDialogViewModel<>));
        Register<MessageBoxViewModel>();
        Register<ConfirmationDialogViewModel>();
    }
    
    /// <summary>
    /// Get the last opened instance of a viewmodel type. This is useful for unit tests to verify
    /// that a dialog was opened with the correct viewmodel and parameters.
    /// </summary>
    public T GetLastOpenedOfType<T>() =>
        (T) _instances
            .Where(i => i.ViewModel.GetType() == typeof(T))
            .OrderBy(i => i.Opened)
            .First().ViewModel;

    /// <summary>
    /// Register a viewmodel type.
    /// Equivalent of <see cref="BlazorVmDialogController.Register(Type, Type)"/>.
    /// </summary>
    public void Register(Type viewModelType)
    {
        if (!viewModelType.IsAssignableTo(typeof(IVmDialogViewModel)))
            throw new InvalidOperationException(
                $"Only types assignable to {typeof(IVmDialogViewModel)} can be registered"
            );
        if (_viewModels.Contains(viewModelType)) 
            throw new InvalidOperationException($"{viewModelType} was already registered");
        _viewModels.Add(viewModelType);
    }
    
    /// <summary>
    /// Register a viewmodel type.
    /// Equivalent of <see cref="BlazorVmDialogController.Register{TComponent, TViewModel}"/>.
    /// </summary>
    public void Register<TViewModel>() where TViewModel : IVmDialogViewModel 
        => Register(typeof(TViewModel));

    /// <summary> <inheritdoc/> </summary>
    public Task<VmDialogReference> Show<T>(
        VmDialogParameters parameters, T viewModel
    ) where T : IVmDialogViewModel
    {
        var vmType = typeof(T).IsGenericType 
            ? typeof(T).GetGenericTypeDefinition() : typeof(T);
        if (!_viewModels.Contains(vmType))
            throw new InvalidOperationException(
                $"viewmodel of type {vmType} was not registered"
            );
        
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