using System.ComponentModel;
using Microsoft.Extensions.Logging;

namespace Dmnk.Blazor.Mvvm;

/// <summary> <inheritdoc/> </summary>
public class ViewModelRegistry(ILogger<ViewModelRegistry> log) : IViewModelRegistry
{
    private readonly Dictionary<Type, Type> _registry = new();

    /// <summary> <inheritdoc/> </summary>
    public void Register<TViewModel, TComponent>()
        where TComponent : MvvmComponentBase<TViewModel>
        where TViewModel : INotifyPropertyChanged
    {
        if (_registry.ContainsKey(typeof(TViewModel)))
        {
            log.LogError(
                "ViewModel {VmType} is already registered with view {ViewType}. " +
                "Overwriting with {NewViewType}",
                typeof(TViewModel).FullName, _registry[typeof(TViewModel)].FullName, 
                typeof(TComponent).FullName);
        }
        _registry[typeof(TViewModel)] = typeof(TComponent);
    }
    
    /// <summary> <inheritdoc/> </summary>
    public Type? GetViewForViewModel(Type viewModelType) => 
        _registry.GetValueOrDefault(viewModelType);
}