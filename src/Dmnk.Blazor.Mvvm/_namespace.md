---
uid: Dmnk.Blazor.Mvvm
---

![NuGet Version](https://img.shields.io/nuget/v/Dmnk.Blazor.Mvvm?style=flat-square&color=blue&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FDmnk.Blazor.Mvvm%2F)

This library provides some base types to inherit from in a Blazor application, to implement the MVVM
pattern. It makes no assumptions about the implementation of the ViewModel, only that it must
implement `INotifyPropertyChanged` and that commands implement `ICommand`.

ViewModels are registered with their Views via DI using `ViewModelRegistration`. The
`ViewModelRegistry` resolves both closed and open-generic registrations. Use the
`[ViewModelFor]` attribute together with the source generator (see
<xref:Dmnk.Blazor.Mvvm.SourceGen>) to have registrations emitted automatically.

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

## Registering ViewModels

```csharp
// Program.cs — register each View/ViewModel pair as a singleton:
services.AddViewModelRegistration<MyViewModel, MyView>();
// For open-generic types:
services.AddViewModelRegistrationOpenGeneric(typeof(MyViewModel<>), typeof(MyView<>));
services.AddBlazorMvvm();
```

Or use the `[ViewModelFor]` attribute and the source generator to emit these calls automatically —
see <xref:Dmnk.Blazor.Mvvm.SourceGen>.