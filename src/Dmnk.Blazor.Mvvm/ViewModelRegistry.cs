using System.ComponentModel;
using Microsoft.Extensions.Logging;

namespace Dmnk.Blazor.Mvvm;

/// <summary> <inheritdoc/> </summary>
public class ViewModelRegistry : IViewModelRegistry
{
    private readonly Dictionary<Type, Type> _registry = new();
    private readonly ILogger<ViewModelRegistry>? _log;

    /// <summary>
    /// See <see cref="IViewModelRegistry"/>
    /// </summary>
    public ViewModelRegistry(
        IEnumerable<ViewModelRegistration> registrations,
        ILogger<ViewModelRegistry>? log = null)
    {
        _log = log;
        foreach (var reg in registrations)
        {
            if (_registry.TryGetValue(reg.ViewModelType, out var value))
            {
                _log?.LogError(
                    "Duplicate registration for ViewModel type {ViewModelType}. " +
                    "Existing View type: {ExistingViewType}, " +
                    "New View type: {NewViewType}. " +
                    "Using existing registration.",
                    reg.ViewModelType, value, reg.ViewType);
                continue;
            }
            _log?.LogTrace(
                "Registering ViewModel type {ViewModelType} with View type {ViewType}", 
                reg.ViewModelType, reg.ViewType);
            _registry[reg.ViewModelType] = reg.ViewType;
        }
    }

    private Type? GetViewForViewModelLogFailure(Type viewModelType)
    {
        if (_registry.TryGetValue(viewModelType, out var viewType))
            return viewType;
        
        _log?.LogWarning("No view registered for ViewModel type {ViewModelType}", viewModelType);
        return null;
    }

    /// <summary> <inheritdoc/> </summary>
    public Type? GetViewForViewModel<TViewModel>(TViewModel viewModel) 
        where TViewModel : INotifyPropertyChanged => 
        GetViewForViewModelLogFailure(typeof(TViewModel));

    /// <summary> <inheritdoc/> </summary>
    public Type? GetViewForViewModelDynamic(Type viewModelType) => 
        GetViewForViewModelLogFailure(viewModelType);
}