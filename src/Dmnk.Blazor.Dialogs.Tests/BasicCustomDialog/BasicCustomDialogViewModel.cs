using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dmnk.Blazor.Dialogs.Api;

namespace Dmnk.Blazor.Dialogs.Tests.BasicCustomDialog;

public partial class BasicCustomDialogViewModel : DialogViewModelBase
{
    [ObservableProperty] private string _inputText = "";
    [ObservableProperty] private int _inputInt = 0;
    [ObservableProperty] private bool _setOnDismissedAsync = false;
    [ObservableProperty] private bool _setOnDismissed = false;

    [RelayCommand]
    public void Confirm() => Dialog.Close();

    public override Task OnDismissAsync()
    {
        SetOnDismissedAsync = true;
        return Task.CompletedTask;
    }

    public override void OnDismiss() => SetOnDismissed = true;
}