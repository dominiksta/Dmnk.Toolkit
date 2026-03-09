using System.ComponentModel;

namespace Dmnk.Blazor.Mvvm;

/// <summary>
/// Represents a mapping of a ViewModel type to a View type.
/// Used by <see cref="ViewModelRegistry"/> to store the associations between ViewModels and Views.
/// <p>
/// You probably want to inject a number of these into DI, such that
/// a ViewModelRegistry that is also in DI can consume them to build its mapping.
/// </p>
/// </summary>
public record ViewModelRegistration
{
    /// <summary>
    /// The type of the ViewModel. Must implement <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public Type ViewModelType { get; private set; }
    /// <summary>
    /// The type of the View. Must inherit from <see cref="MvvmComponentBase{T}"/>
    /// where T is the ViewModelType.
    /// </summary>
    public Type ViewType { get; private set; }
    
    private ViewModelRegistration(Type viewModelType, Type viewType)
    {
        ViewModelType = viewModelType;
        ViewType = viewType;
    }
    
    /// <summary>
    /// Creates a new instance of <see cref="ViewModelRegistration"/>.
    /// </summary>
    public static ViewModelRegistration Create<TViewModel, TView>()
        where TViewModel : INotifyPropertyChanged 
        where TView : MvvmComponentBase<TViewModel> 
        => new(typeof(TViewModel), typeof(TView));
}