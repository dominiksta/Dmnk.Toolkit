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
    ///
    /// <p>
    /// If you are not using trimming (enabled by default in Blazor WASM since .NET 6 with
    /// <c>dotnet publish</c>) or AOT compilation, you may prefer using
    /// <see cref="AutoViewModelRegistry.AutoRegister"/> for automatic registration.
    /// Otherwise, use this method to manually register all View/ViewModel pairs.
    /// </p>
    /// </summary>
    void Register<TViewModel, TComponent>(bool noWarn = false)
        where TComponent : MvvmComponentBase<TViewModel>
        where TViewModel : INotifyPropertyChanged;
    
    /// <summary>
    /// Like <see cref="Register{TViewModel, TComponent}"/>, but accepts Type parameters instead of
    /// generics.
    ///
    /// <p>
    /// Note that there are no runtime checks for the constraints that apply to the generic version
    /// of this method to allow usage with e.g. AOT compilation.
    /// </p>
    /// </summary>
    void RegisterDynamic(Type viewModelType, Type componentType, bool noWarn = false);

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