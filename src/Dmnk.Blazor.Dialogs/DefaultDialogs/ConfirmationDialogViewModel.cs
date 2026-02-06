using Dmnk.Blazor.Dialogs.Properties;
using Dmnk.Blazor.Dialogs.SimpleMvvm;

namespace Dmnk.Blazor.Dialogs.DefaultDialogs;

/// <summary>
/// Show a confirmation dialog with "Yes" and "No" buttons. The <see cref="Result"/> is a boolean
/// indicating whether the user confirmed or denied the prompt.
/// Prefer using <see cref="ConfirmationDialogControllerExtensions"/> to show this dialog.
/// </summary>
public class ConfirmationDialogViewModel : MessageBoxViewModel
{
    /// <summary>
    /// The text to show on the confirmation button. Defaults to "Yes".
    /// </summary>
    public override string ActionConfirmText { get; init; } = Resources.Generic_Yes;
    
    /// <summary>
    /// The text to show on the denial button. Defaults to "No".
    /// </summary>
    public string ActionDenyText { get; init; } = Resources.Generic_No;

    /// <summary>
    /// Whether the user confirmed (true) or denied (false) the prompt. Will be false
    /// if the dialog was dismissed.
    /// </summary>
    public bool Result
    {
        get;
        private set => SetProperty(ref field, value);
    }
    
    /// <summary>
    /// Command to close the dialog, with the command parameter being a boolean indicating whether
    /// the user confirmed (true) or denied (false) the prompt.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public SimpleAsyncRelayCommand CloseCommand =>
        new(async parameter =>
        {
            // ReSharper disable once LocalizableElement
            if (parameter is not bool result) throw new ArgumentException(
                $"Parameter must be of type {typeof(bool)}", nameof(parameter));
            Result = result;
            await Dialog.Close();
        });
}