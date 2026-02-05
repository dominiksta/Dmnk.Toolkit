using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dmnk.Blazor.Dialogs.Properties;

namespace Dmnk.Blazor.Dialogs.DefaultDialogs;

public partial class ConfirmationDialogViewModel : MessageBoxViewModel
{
    public override string ActionConfirmText { get; init; } = Resources.Generic_Yes;
    public string ActionDenyText { get; init; } = Resources.Generic_No;

    [ObservableProperty] private bool _result = false;

    [RelayCommand]
    public async Task Close(bool result)
    {
        Result = result;
        await Dialog.Close();
    }
}