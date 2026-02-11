---
uid: Dmnk.Blazor.Mvvm.CommunityToolkit
---

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