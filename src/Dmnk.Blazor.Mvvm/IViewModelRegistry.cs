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
/// services.AddSingleton&lt;ViewModelRegistry&gt;(container => {
///     var registry = new ViewModelRegistry(
///         container.GetRequiredService&lt;ILogger&lt;ViewModelRegistry&gt;&gt;());
///     registry.Register&lt;MySpecificViewModel1, MyComponent1&gt;();
///     registry.Register&lt;MySpecificViewModel2, MyComponent2&gt;();
///     return registry;
/// });
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
    /// Registers a View type for a given ViewModel type.
    /// TComponent must be an <see cref="MvvmComponentBase{T}"/>, not with an optional or owned
    /// viewmodel, because we pass a non-null ViewModel instance to the component when rendering.
    /// Without that constraint, it would be possible to register a component that doesn't accept
    /// the ViewModel as a parameter.
    /// </summary>
    void Register<TViewModel, TComponent>()
        where TComponent : MvvmComponentBase<TViewModel>
        where TViewModel : INotifyPropertyChanged;

    /// <summary>
    /// Retrieves the registered View type for a given ViewModel type, or null if no registration
    /// exists.
    /// </summary>
    Type? GetViewForViewModel(Type viewModelType);
}