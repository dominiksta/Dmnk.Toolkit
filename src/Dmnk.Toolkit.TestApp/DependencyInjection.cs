using Dmnk.Toolkit.TestApp.Components.Pages.DynamicItems;
using Dmnk.Toolkit.TestApp.Components.Pages.DynamicItems.DynamicItem1;
using Dmnk.Toolkit.TestApp.Components.Pages.DynamicItems.DynamicItem2;

namespace Dmnk.Toolkit.TestApp;

internal static class DependencyInjection
{
    public static void AddDynamicItemViewModels(this IServiceCollection services)
    {
        // scoped in this context means: one per circuit, meaning one per page-load
        services.AddScoped<IDynamicItemViewModel, DynamicItem1ViewModel>();
        // transient means a new instance every time one is requested
        services.AddTransient<IDynamicItemViewModel, DynamicItem2ViewModel>();
    }

    public static void AddViewModelRegistrations(this IServiceCollection services)
    {
        SourceGeneratedViewModelRegistrations.Register(services);
    }
}
