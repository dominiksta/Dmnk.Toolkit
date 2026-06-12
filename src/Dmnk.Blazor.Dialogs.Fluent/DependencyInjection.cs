using Dmnk.Blazor.Dialogs.Api;
using Dmnk.Blazor.Dialogs.DefaultDialogs;
using Dmnk.Blazor.Dialogs.Fluent.DefaultDialogs;
using Dmnk.Blazor.Mvvm;
using Microsoft.Extensions.DependencyInjection;

namespace Dmnk.Blazor.Dialogs.Fluent;

/// <summary>
/// DI helpers. See <see cref="AddFluentMvvmDialogs"/>.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers relevant types in DI.
    /// An <see cref="Dmnk.Blazor.Mvvm.IViewModelRegistry"/> must also be registered separately
    /// (e.g. via <c>AddMvvm()</c>) for the <c>DialogControllerProvider</c> to resolve dialog views.
    /// <p>
    /// This also registers <see cref="ViewModelRegistration"/> entries for the built-in dialog
    /// types (<see cref="MessageBoxViewModel"/>, <see cref="ConfirmationDialogViewModel"/>,
    /// <see cref="InputDialogViewModel{T}"/>), so they are automatically picked up by
    /// <see cref="ViewModelRegistry"/>.
    /// </p>
    /// </summary>
    public static IServiceCollection AddFluentMvvmDialogs(this IServiceCollection services)
    {
        services.AddScoped<BlazorVmDialogController, FluentVmDialogController>();
        services.AddScoped<IVmDialogController>(
            sp => sp.GetRequiredService<BlazorVmDialogController>());

        services.AddSingleton(ViewModelRegistration.Create<MessageBoxViewModel, MessageBoxView>());
        services.AddSingleton(ViewModelRegistration.Create<ConfirmationDialogViewModel, ConfirmationDialogView>());
        services.AddSingleton(ViewModelRegistration.CreateOpenGeneric(
            typeof(InputDialogViewModel<>), typeof(InputDialogView<>)));

        return services;
    }
}