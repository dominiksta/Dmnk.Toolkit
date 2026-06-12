using System.ComponentModel;

namespace Dmnk.Blazor.Mvvm;

/// <summary>
/// Registry for associating ViewModel types with their corresponding View types.
/// </summary>
/// <example>
/// ViewModels:
/// <code>
/// class MySpecificViewModel1 : MyAbstractViewModel { }
/// class MySpecificViewModel2 : MyAbstractViewModel { }
/// </code>
/// Views:
/// <code lang="razor">
/// @inherits MvvmComponentBase&lt;MySpecificViewModel1&gt;
/// &lt;h3&gt;View for MySpecificViewModel1&lt;/h3&gt;
/// </code>
/// DI Setup:
/// <code>
/// services.AddBlazorMvvm();
/// services.AddViewModelRegistration&lt;MySpecificViewModel1, MyComponent1&gt;();
/// services.AddViewModelRegistration&lt;MySpecificViewModel2, MyComponent2&gt;();
/// </code>
/// In Component:
/// <code lang="razor">
/// @inject IEnumerable&lt;MyAbstractViewModel&gt; ViewModels
/// 
/// @foreach (var vm in ViewModels)
/// {
///     &lt;RegisteredViewFor Vm="vm"/&gt;
/// }
/// </code>
/// </example>
public interface IViewModelRegistry
{
    /// <summary>
    /// Retrieves the registered View type for a given ViewModel type, or null if no registration
    /// exists.
    /// </summary>
    Type? GetViewForViewModel<TViewModel>(TViewModel viewModel) 
        where TViewModel : INotifyPropertyChanged;
    
    /// <summary>
    /// Like <see cref="GetViewForViewModel{TViewModel}(TViewModel)"/>, but accepts a Type parameter
    /// instead of an instance.
    /// </summary>
    Type? GetViewForViewModelDynamic(Type viewModelType);
}