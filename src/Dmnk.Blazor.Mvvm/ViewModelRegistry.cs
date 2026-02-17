using System.ComponentModel;
using Microsoft.Extensions.Logging;

namespace Dmnk.Blazor.Mvvm;

/// <summary> <inheritdoc/> </summary>
public class ViewModelRegistry(ILogger<ViewModelRegistry>? log = null) : IViewModelRegistry
{
    private readonly Dictionary<Type, Type> _registry = new();

    /// <summary> <inheritdoc/> </summary>
    public void Register<TViewModel, TComponent>(bool noWarn = false)
        where TComponent : MvvmComponentBase<TViewModel>
        where TViewModel : INotifyPropertyChanged
    {
        RegisterDynamic(typeof(TViewModel), typeof(TComponent), noWarn);
    }

    /// <summary> <inheritdoc/> </summary>
    public void RegisterDynamic(Type viewModelType, Type componentType, bool noWarn = false)
    {
        if (!noWarn && _registry.TryGetValue(viewModelType, out var view))
        {
            log?.LogError(
                "ViewModel {VmType} is already registered with view {ViewType}. " +
                "Overwriting with {NewViewType}",
                viewModelType.FullName, view.FullName, 
                componentType.FullName);
        }
        
        _registry[viewModelType] = componentType;
    }

    /// <summary> <inheritdoc/> </summary>
    public Type? GetViewForViewModel<TViewModel>(TViewModel viewModel) 
        where TViewModel : INotifyPropertyChanged => 
        GetViewForViewModelDynamic(typeof(TViewModel));

    /// <summary> <inheritdoc/> </summary>
    public Type? GetViewForViewModelDynamic(Type viewModelType) => 
        _registry.GetValueOrDefault(viewModelType);
}