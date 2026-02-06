using System.Windows.Input;
using Dmnk.Blazor.Dialogs.Api;
using Dmnk.Blazor.Dialogs.Properties;
using Dmnk.Blazor.Dialogs.SimpleMvvm;

namespace Dmnk.Blazor.Dialogs.DefaultDialogs;

public partial class InputDialogViewModel<T> : SimpleDialogViewModelBase
{
    public string ActionConfirmText { get; init; } = Resources.Actions_OK;
    public string ActionCancelText { get; init; } = Resources.Actions_Cancel;
    
    public Func<T?, string?>? Validate { get; init; }

    public T? Value
    {
        get;
        set
        {
            SetProperty(ref field, value);
            OnPropertyChanged(nameof(ValidationError));
        }
    }

    public string? ValidationError => Validate?.Invoke(Value);

    private async Task Close()
    {
        if (ValidationError != null) return;
        await Dialog.Close();
    }
    
    public SimpleAsyncRelayCommand CloseCommand => new SimpleAsyncRelayCommand(
        async _ =>
        {
            if (ValidationError != null) return;
            await Dialog.Close();
        }, 
        _ => ValidationError == null
    );
}