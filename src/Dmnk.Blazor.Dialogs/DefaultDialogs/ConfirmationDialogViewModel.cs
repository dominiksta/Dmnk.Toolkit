using System.Windows.Input;
using Dmnk.Blazor.Dialogs.Api;
using Dmnk.Blazor.Dialogs.Properties;
using Dmnk.Blazor.Dialogs.SimpleMvvm;

namespace Dmnk.Blazor.Dialogs.DefaultDialogs;

public class ConfirmationDialogViewModel : MessageBoxViewModel
{
    public override string ActionConfirmText { get; init; } = Resources.Generic_Yes;
    public string ActionDenyText { get; init; } = Resources.Generic_No;

    public bool Result
    {
        get;
        private set => SetProperty(ref field, value);
    }
    
    public SimpleAsyncRelayCommand CloseCommand =>
        new(async parameter =>
        {
            if (parameter is not bool result) 
                throw new ArgumentException($"Parameter must be of type {typeof(bool)}", nameof(parameter));
            Result = result;
            await Dialog.Close();
        });
}