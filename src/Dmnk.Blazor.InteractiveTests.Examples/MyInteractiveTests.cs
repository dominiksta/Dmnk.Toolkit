using Dmnk.Blazor.Dialogs.Fluent;
using Dmnk.Blazor.InteractiveTests.TestComponents;
using Dmnk.Blazor.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Dmnk.Blazor.InteractiveTests.Examples;

[TestFixture]
public class MyInteractiveTests
{
    [Test, Explicit, Apartment(ApartmentState.STA)]
    public async Task Show_Counter_Component()
    {
        await BlazorInteractiveTests.ShowComponent<Counter>();
    }
    
    [Test, Explicit, Apartment(ApartmentState.STA)]
    public async Task Show_FluentMvvm_Component()
    {
        var services = new ServiceCollection()
            .AddFluentUIComponents()
            .AddBlazorMvvm()
            .AddFluentMvvmDialogs();
        
        await BlazorInteractiveTests.ShowComponent<FluentMvvmTestComponent>(
            parameters: null, services: services);
    }
}