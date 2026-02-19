using Dmnk.Blazor.Dialogs.Api;
using Microsoft.Extensions.DependencyInjection;

namespace Dmnk.Blazor.Dialogs.Fluent;

/// <summary>
/// DI helpers. See <see cref="AddFluentMvvmDialogs"/>.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers relevant types in DI and returns an instance of <see
    /// cref="FluentVmDialogController"/> so you can register your dialogs.
    /// </summary>
    public static FluentVmDialogController AddFluentMvvmDialogs(this IServiceCollection services)
    {
        var dialogController = new FluentVmDialogController();
        services.AddSingleton<BlazorVmDialogController>(dialogController);
        services.AddSingleton<IVmDialogController>(dialogController);
        return dialogController;
    }
}