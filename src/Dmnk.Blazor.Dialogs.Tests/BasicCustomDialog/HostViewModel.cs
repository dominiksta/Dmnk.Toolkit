using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dmnk.Blazor.Dialogs.Api;

namespace Dmnk.Blazor.Dialogs.Tests.BasicCustomDialog;

public partial class HostViewModel(IVmDialogController vmDialog) : ObservableObject
{
    public const int InitialInputInt = 1337;
    public const string InitialInputStr = "1337";

    [ObservableProperty] private int _intDlgResult = 0;
    [ObservableProperty] private string _strDlgResult = "";
    
    [RelayCommand]
    public async Task ShowDialog()
    {
        var vm = new BasicCustomDialogViewModel()
        {
            InputInt = InitialInputInt,
            InputText = InitialInputStr
        };
        var dlg = await vmDialog.Show(new VmDialogParameters() { Title = "" }, vm);
        await dlg.WaitClosed;
        IntDlgResult = vm.InputInt;
        StrDlgResult = vm.InputText;
    }
}