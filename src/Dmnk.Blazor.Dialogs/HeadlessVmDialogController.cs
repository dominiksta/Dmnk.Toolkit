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

    public HeadlessVmDialogController()
    {
        Register(typeof(InputDialogViewModel<>));
        Register<MessageBoxViewModel>();
        Register<ConfirmationDialogViewModel>();
    }
    
    public T GetLastOpenedOfType<T>()
    {
        return (T) _instances
            .Where(i => i.ViewModel.GetType() == typeof(T))
            .OrderBy(i => i.Opened)
            .First().ViewModel;
    }

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
    
    public void Register<TViewModel>() where TViewModel : IVmDialogViewModel 
        => Register(typeof(TViewModel));

    public async Task<VmDialogReference> Show<T>(
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
        viewModel.Dialog = reference;
        // ReSharper disable once MethodHasAsyncOverload
        return reference;
    }

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
        
        Icon MkIcon(string name) => new Icon(new PngIconDefinition(name), new Size(0, 0));
    }
}