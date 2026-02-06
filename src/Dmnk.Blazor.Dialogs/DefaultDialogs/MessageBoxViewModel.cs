using Dmnk.Blazor.Dialogs.Api;
using Dmnk.Blazor.Dialogs.Properties;
using Microsoft.AspNetCore.Components;

namespace Dmnk.Blazor.Dialogs.DefaultDialogs;

public partial class MessageBoxViewModel : DialogViewModelBase
{
    public virtual string ActionConfirmText { get; init; } = Resources.Actions_OK;
    public MessageBoxType Type { get; init; } = MessageBoxType.Info;
    public string? Message { get; init; }
    public MarkupString? MessageMarkup { get; init; }

    public object DisplayMessage()
    {
        if (Message != null) return Message;
        if (MessageMarkup != null) return MessageMarkup;
        throw new InvalidOperationException(
            $"One of {nameof(Message)} or {nameof(MessageMarkup)} must be set"
        );
    }
}