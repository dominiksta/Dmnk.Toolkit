---
uid: Dmnk.Blazor.InteractiveTests
---

![NuGet Version](https://img.shields.io/nuget/v/Dmnk.Blazor.InteractiveTests?style=flat-square&color=blue&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FDmnk.Blazor.InteractiveTests%2F)

This package provides a way for you to write NUnit/xUnit/MSTest/whatever "tests" that show a 
particular Blazor component in a window. We call these "tests" interactive tests (not sure if 
there is a commonly accepted term for this).

This is *not intended for actual automated testing*. Rather, it is a way for you to work
on and manually test a component without starting your whole application - which becomes
especially useful in a large application.

Regrettably, this is **windows-only**, because it relies on the WinForms implementation of
BlazorWebView. Implementations of a Blazor Hybrid style webview for other platforms seem
to *kind of sort of* exist as community packages. If demand arises, that may be worth looking
into. The API shape *should* allow this to become cross-platform without breaking
changes.

It is worth noting that while the functionality is windows-only, your tests should still be able
to run on a **Linux-based CI environment**. To do so, you should multi-target your tests to e.g.
`net10.0;net10.0-windows`. Actually running the interactive tests on the cross-platform build
will raise an exception. But since they should be `[Explicit]`, they won't interfere with CI.

## Example (Assuming NUnit)

In `My.BlazorLibrary.Tests\Setup.cs`:

```csharp
[SetUpFixture]
public class TestSetUp
{
    [OneTimeSetUp]
    public void Setup() => BlazorInteractiveTestRunner.PathInfo =
        InteractiveTestsProjectPathInfo.FromAssemblyInSolutionDir(GetType().Assembly);
}
```

In `My.BlazorLibrary.Tests\MyInteractiveTests.cs`:

```csharp
// MUST be explicit - it will not terminate on its own.
// MUST have this ApartmentState.
[Test, Explicit, Apartment(ApartmentState.STA)]
public async Task Show_Counter_Component()
{
    // This will open up a window with just your `Counter` component so you can play
    // with it without starting the whole app.
    await BlazorInteractiveTestRunner.ShowComponent<Counter>();
}
```

## More Complex Example (TestBed and Services)

The same code for `Setup.cs` in the prior example still applies.

Assuming you have a component like this (`MyFluentComponent`):

```razor
@using Microsoft.FluentUI.AspNetCore.Components
@inject IDialogService DialogService

<h3>@Title</h3>

<FluentCard>
    <FluentButton OnClick='@(() =>  DialogService.ShowInfoAsync("Hello"))'>
        Show Message Box
    </FluentButton>
</FluentCard>

@code {
    [Parameter, EditorRequired] public required string Title { get; set; }
}
```

You can write an interactive test for it like this:

```csharp
using Dmnk.Blazor.InteractiveTests.FluentUIHelpers;
using Dmnk.Blazor.InteractiveTests.TestComponents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;

[Test, Explicit, Apartment(ApartmentState.STA)]
public async Task Show_FluentMvvm_Component()
{
    var services = new ServiceCollection()
        .AddFluentUIComponents();
    
    await BlazorInteractiveTestRunner.ShowComponent<MyFluentComponent, FluentUITestBed>(
        configureComponent: parameters => parameters
            .Add(c => c.Title, "My Title"),
        services: services);
}
```

Note that the **`FluentUITestBed`** is defined as a component that simply exposes a `ChildContent`
parameter that will be populated with the SUT component (`MyFluentComponent`). You can build your
own testbeds. It would be advisable to read the source code of 
<xref:Dmnk.Blazor.InteractiveTests.FluentUIHelpers>.