using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dmnk.Blazor.Dialogs.Api;
using Dmnk.Blazor.Dialogs.DefaultDialogs;

namespace Dmnk.Toolkit.TestApp.Components.Pages.TestDialog;

public partial class DialogHostViewModel(IVmDialogController dialogController) : ObservableObject
{
    [RelayCommand]
    private async Task ShowDialog()
    {
        var vm = new TestDialogViewModel(dialogController)
        {
            InputValue = "Initial Input Value"
        };
        var dlg = await dialogController.Show(
            new VmDialogParameters { Title = "Input Dialog" }, vm);

        await dlg.WaitClosed;

        if (dlg.Cancelled)
        {
            Console.WriteLine("Dialog was cancelled");
            return;
        }
        Console.WriteLine($"Dialog closed with input: {vm.InputValue}");

        await dialogController.ShowSuccess($"You entered: {vm.InputValue}");
    }
}
