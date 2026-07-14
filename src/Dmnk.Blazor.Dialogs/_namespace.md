---
uid: Dmnk.Blazor.Dialogs
---

![NuGet Version](https://img.shields.io/nuget/v/Dmnk.Blazor.Dialogs?style=flat-square&color=blue&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FDmnk.Blazor.Dialogs%2F)

Provides a set of components and services to easily create and manage dialogs in Blazor applications
using the MVVM pattern. Based on <xref:Dmnk.Blazor.Mvvm> (and <xref:Dmnk.Icons.Core>).

## Example (Assuming `Dmnk.Blazor.Dialogs.Fluent` implementation)

**MyHostViewModel.cs**:

```csharp
[ViewModelFor(typeof(MyHostView))]
public class MyHostViewModel(IVmDialogController dialogController) : ObservableObject
{
    [RelayCommand]
    private async Task ShowDialog()
    {
        var vm = new MyInputDialogViewModel();
        var dlg = await dialogController.Show(
            new VmDialogParameters { Title = "Input Dialog" }, vm);
        await dlg.WaitClosed;
        if (dlg.Cancelled)
        {
            Console.WriteLine("Dialog was cancelled");
            return;
        }
        Console.WriteLine($"Dialog closed with input: {vm.InputValue}");
        dialogController.ShowSuccess($"You entered: {vm.InputValue}");
    }
}
```

**MyInputDialogViewModel.cs**:

```csharp
[ViewModelFor(typeof(MyInputDialog))]
public class MyInputDialogViewModel : DialogViewModelBase
{
    [ObservableProperty]
    private string _inputValue;
}
```

**Program.cs**:

```csharp
services.AddBlazorMvvm();
services.AddFluentMvvmDialogs(); // specific to Dmnk.Blazor.Dialogs.Fluent
// Dialog views are resolved via IViewModelRegistry — register each pair as a singleton:
services.AddViewModelRegistration<MyInputDialog, MyInputDialogViewModel>();
// or, preferably, with the source generator:
My.Namespace.SourceGeneratedViewModelRegistrations.Register(services);
```

**SomeRootComponentWithInteractivity.razor** (specific to Dmnk.Blazor.Dialogs.Fluent):
```razor
@using Dmnk.Blazor.Dialogs.Fluent
@* again, if you don't use the fluent implementation you will have to supply your own *@

<DialogControllerProvider/>
```

**MyHostComponent.razor**:

```razor
@inherits MvvmComponentBase<MyHostViewModel>

<button @onclick="@Vm.ShowDialogCommand.Bind(this)">Show Dialog</button>
```

**MyInputDialog.razor**:

```razor
@inherits BlazorVmDialogViewFor<MyInputDialogViewModel>

@* VmDialogBody and VmDialogFooter are specific to Dmnk.Blazor.Dialogs.Fluent *@

<VmDialogBody>
  <input @bind="Vm.InputValue" placeholder="Enter something..." />
</VmDialogBody>

<VmDialogFooter>
  <button @onclick="@Dialog.Close">Confirm</button>
  <button @onclick="@Dialog.Dismiss">Cancel</button>
</VmDialogFooter>

@code {
    [Parameter] public override required VmDialogReference Dialog { get; set; }
    [Parameter] public override required VmDialogParameters Params { get; set; }
}
```