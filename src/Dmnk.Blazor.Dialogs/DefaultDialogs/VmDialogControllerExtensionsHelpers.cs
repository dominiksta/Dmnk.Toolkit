using Dmnk.Blazor.Dialogs.Properties;

namespace Dmnk.Blazor.Dialogs.DefaultDialogs;

internal static class VmDialogControllerExtensionsHelpers
{
    internal static string TitleForIntent(MessageBoxType type) => type switch
    {
        MessageBoxType.Success => Resources.MessageIntent_Success,
        MessageBoxType.Error => Resources.MessageIntent_Error,
        MessageBoxType.Warning => Resources.MessageIntent_Warning,
        MessageBoxType.Confirmation => Resources.MessageBoxIntent_Confirmation,
        _ => Resources.MessageIntent_Info,
    };
}