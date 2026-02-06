using Dmnk.Blazor.Dialogs.Properties;
using Dmnk.Blazor.Dialogs.SimpleMvvm;
using Microsoft.AspNetCore.Components;

namespace Dmnk.Blazor.Dialogs.DefaultDialogs;

/// <summary>
/// ViewModel for a simple message box dialog, which displays a message and an "OK" button.
/// See the MessageBox functions in <see cref="VmDialogControllerExtensionsHelpers"/> for usage.
/// </summary>
public class MessageBoxViewModel : SimpleDialogViewModelBase
{
    /// <summary>
    /// The text to show on the confirmation button. Defaults to "OK".
    /// </summary>
    public virtual string ActionConfirmText { get; init; } = Resources.Actions_OK;
    
    /// <summary> See <see cref="MessageBoxType"/> </summary>
    public MessageBoxType Type { get; init; } = MessageBoxType.Info;
    
    /// <summary>
    /// The message body to display in the message box.
    /// <p>One of this property and <see cref="MessageMarkup"/> <b>must</b> be set.</p>
    /// </summary>
    public string? Message { get; init; }
    
    /// <summary>
    /// Like <see cref="Message"/>, but allows for markup.
    /// <b>Beware of XSS vulnerabilities</b>.
    /// <p>One of this property and <see cref="MessageMarkup"/> <b>must</b> be set.</p>
    /// </summary>
    public MarkupString? MessageMarkup { get; init; }

    /// <summary>
    /// Get either the plain text message or the markup message, depending on which one is set.
    /// </summary>
    public object DisplayMessage()
    {
        if (Message != null) return Message;
        if (MessageMarkup != null) return MessageMarkup;
        throw new InvalidOperationException(
            $"One of {nameof(Message)} or {nameof(MessageMarkup)} must be set"
        );
    }
}