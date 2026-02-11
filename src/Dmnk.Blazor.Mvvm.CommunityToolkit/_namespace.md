---
uid: Dmnk.Blazor.Mvvm.CommunityToolkit
---

![NuGet Version](https://img.shields.io/nuget/v/Dmnk.Blazor.Mvvm.CommunityToolkit?style=flat-square&color=blue&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FDmnk.Blazor.Mvvm.CommunityToolkit%2F)

A teeny-tiny (like 20 LOC) library that provides an extension methods on
<xref:CommunityToolkit.Mvvm.Input.IAsyncRelayCommand> to allow efficient binding in a blazor
component.

See <xref:Dmnk.Blazor.Mvvm.CommunityToolkit.CommandExtensions>, as well as
<xref:Dmnk.Blazor.Mvvm.CommandExtensions>.

## Usage

**_Imports.razor**:

```razor
@using Dmnk.Blazor.Mvvm.CommunityToolkit
```

**MyViewModel.cs**:

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public class MyViewModel : ObservableObject
{
    [RelayCommand]
    private async Task AsyncMethod()
    {
        // Do something
    }
    
    [RelayCommand]
    private void SyncMethod()
    {
        // Do something
    }
}
```

**MyComponent.razor**:

```razor
@inherits MvvmComponentBase<MyViewModel>

@* Resolves to Dmnk.Dmnk.Blazor.Mvvm.CommunityToolkit.CommandExtensions.Bind *@
<button @onclick="Vm.AsyncMethodCommand.Bind(this)">Click me</button>

@* Resolves to Dmnk.Dmnk.Blazor.Mvvm.CommandExtensions.Bind *@
<button @onclick="Vm.SyncMethodCommand.Bind(this)">Click me</button>
```