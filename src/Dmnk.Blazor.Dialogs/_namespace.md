---
uid: Dmnk.Blazor.Dialogs
---

![NuGet Version](https://img.shields.io/nuget/v/Dmnk.Blazor.Dialogs?style=flat-square&color=blue&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FDmnk.Blazor.Dialogs%2F)

Provides a set of components and services to easily create and manage dialogs in Blazor applications
using the MVVM pattern. Based on <xref:Dmnk.Blazor.Mvvm> (and <xref:Dmnk.Icons.Core>).

## Example

**MyHostViewModel.cs**:

```csharp
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
public class MyInputDialogViewModel : DialogViewModelBase
{
    [ObservableProperty]
    private string _inputValue;
}
```

**Program.cs**:

```csharp
// you need a concrete implementation of IVmDialogController/BlazorVmDialogController
// FluentVmDialogController is implemented in the Dmnk.Blazor.Dialogs.Fluent package using FluentUI
var dialogController = new FluentVmDialogController();
dialogController.Register<MyInputDialogViewModel, MyInputDialog>();

// obviously you could also just pass it in the constructor or inject it however you want
builder.Services.AddSingleton<IVmDialogController>(dialogController);
```

**MyHostComponent.razor**:

```razor
@inherits MvvmComponentBase<MyHostViewModel>

<button @onclick="@Vm.ShowDialogCommand.Bind(this)">Show Dialog</button>
```

**MyInputDialog.razor**:

```razor
@inherits BlazorVmDialogViewFor<MyInputDialogViewModel>

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