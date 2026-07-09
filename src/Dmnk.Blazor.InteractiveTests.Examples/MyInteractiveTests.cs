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
        // System.Diagnostics.Debugger.Launch();
        await BlazorInteractiveTestRunner.ShowComponent<Counter>();
    }
    
    [Test, Explicit, Apartment(ApartmentState.STA)]
    public async Task Show_Counter_Component_With_Parameters()
    {
        // System.Diagnostics.Debugger.Launch();
        await BlazorInteractiveTestRunner.ShowComponent<Counter>(
            parameters => parameters
                .Add(c => c.OptionalTitle, "My Optional Title"));
    }
    
    [Test, Explicit, Apartment(ApartmentState.STA)]
    public async Task Show_FluentMvvm_Component()
    {
        var services = new ServiceCollection()
            .AddFluentUIComponents()
            .AddBlazorMvvm()
            .AddFluentMvvmDialogs();
        
        await BlazorInteractiveTestRunner.ShowComponent<FluentMvvmTestComponent>(
            parameters: null, services: services);
    }

    /// <summary>
    /// This test project should still compile and run on linux. The interactive tests
    /// will simply fail if you haven't selected netX.X-windows in your IDE. And since
    /// they are explicit, they shouldn't interfere with (linux-based) CI.
    /// </summary>
    [Test]
    public void RegularTest() => Assert.Pass();
}