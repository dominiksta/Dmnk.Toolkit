using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dmnk.Blazor.Dialogs.Api;
using Dmnk.Blazor.Dialogs.Properties;

namespace Dmnk.Blazor.Dialogs.DefaultDialogs;

public partial class InputDialogViewModel<T> : DialogViewModelBase
{
    public string ActionConfirmText { get; init; } = Resources.Actions_OK;
    public string ActionCancelText { get; init; } = Resources.Actions_Cancel;
    
    public Func<T?, string?>? Validate { get; init; }
    
    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(ValidationError))]
    private T? _value;

    public string? ValidationError => Validate?.Invoke(Value);

    [RelayCommand(CanExecute = nameof(CanClose))]
    public async Task Close()
    {
        if (ValidationError != null) return;
        await Dialog.Close();
    }

    public bool CanClose() => ValidationError == null;
}