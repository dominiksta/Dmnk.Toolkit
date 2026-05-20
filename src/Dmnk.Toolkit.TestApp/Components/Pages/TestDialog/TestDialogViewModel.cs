using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dmnk.Blazor.Dialogs.Api;
using Dmnk.Blazor.Dialogs.CommunityToolkit;
using Dmnk.Blazor.Dialogs.DefaultDialogs;
using Dmnk.Blazor.Mvvm;

namespace Dmnk.Toolkit.TestApp.Components.Pages.TestDialog;

[ViewModelFor(typeof(TestDialogView))]
public partial class TestDialogViewModel(IVmDialogController dialogController) : DialogViewModelBase
{
    [ObservableProperty]
    private string _inputValue = string.Empty;

    [RelayCommand]
    private void ShowNestedMessageBox() => dialogController.ShowInfo("Nested MessageBox");
}
