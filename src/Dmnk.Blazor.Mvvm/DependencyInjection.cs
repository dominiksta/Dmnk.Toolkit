using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace Dmnk.Blazor.Mvvm;

/// <summary>
/// Extension methods for setting up Blazor MVVM services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds core Blazor MVVM services.
    /// </summary>
    public static IServiceCollection AddBlazorMvvm(this IServiceCollection services)
    {
        services.AddSingleton<IViewModelRegistry, ViewModelRegistry>();
        return services;
    }
    
    /// <summary>
    /// Adds a View to ViewModel registration (see <see cref="ViewModelRegistration"/>).
    ///
    /// <p>
    /// In most cases, you would likely want to use the source-generated registrations
    /// from the <see cref="ViewModelForAttribute"/> instead.
    /// </p>
    /// </summary>
    public static IServiceCollection AddViewModelRegistration<TViewModel, TView>(
        this IServiceCollection services)
        where TView : MvvmComponentBase<TViewModel>
        where TViewModel : INotifyPropertyChanged
    {
        var registration = ViewModelRegistration.Create<TViewModel, TView>();
        services.AddSingleton(registration);
        return services;
    }
    
    /// <summary>
    /// <p>
    /// Adds a <em>generic</em> View to ViewModel registration
    /// (see <see cref="ViewModelRegistration.CreateOpenGeneric"/>).
    /// </p>
    ///
    /// <p>
    /// Like <see cref="AddViewModelRegistration"/>, but for open generic type pairs
    /// (e.g. <c>typeof(MyViewModel&lt;&gt;)</c> and <c>typeof(MyView&lt;&gt;)</c>).
    /// </p>
    ///
    /// <p>
    /// In most cases, you would likely want to use the source-generated registrations
    /// from the <see cref="ViewModelForAttribute"/> instead.
    /// </p>
    /// </summary>
    public static IServiceCollection AddViewModelRegistrationOpenGeneric(
        this IServiceCollection services, Type viewModelType, Type viewType)
    {
        var registration = ViewModelRegistration.CreateOpenGeneric(viewModelType, viewType);
        services.AddSingleton(registration);
        return services;
    }
}