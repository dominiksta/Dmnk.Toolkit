---
uid: Dmnk.Blazor.Mvvm
---

This library provides some base types to inherit from in a Blazor application, to implement the MVVM
pattern. It makes no assumptions about the implementation of the ViewModel, only that it must
implement `INotifyPropertyChanged` and that commands implement `ICommand`.

## Example

```csharp
// You don't have to use the community toolkit, but we will use it in this example here.
// If you are using the community toolkit, you probably also want the 
// Dmnk.Blazor.Mvvm.CommunityToolkit package, which allows binding to async relay commands.
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public class MyViewModel : ObservableObject
{
    [ObservableProperty]
    private string _text;
    
    [RelayCommand]
    private void DoSomething()
    {
        Text = "Hello World!";
    }
}
```

```razor
@inherits MvvmComponentBase<MyViewModel>

<p>@Vm.Text</p>

<button @onclick="@Vm.DoSomethingCommand.Bind(this)">Click me</button>
```