using Dmnk.Blazor.Dialogs.Api;
using Dmnk.Blazor.Dialogs.Properties;
using Microsoft.AspNetCore.Components;

namespace Dmnk.Blazor.Dialogs.DefaultDialogs;

/// <summary>
/// Extension methods for <see cref="IVmDialogController"/> to show simple message boxes with a
/// single "OK" button, using the <see cref="MessageBoxViewModel"/>.
/// </summary>
public static class MessageBoxDialogControllerExtensions
{
    private static async Task ShowMessageBox(
        this IVmDialogController dialog,
        string? title, MessageBoxType type, string? message, 
        MarkupString? messageMarkup, string? confirmText
    )
    {
        var dlg = await dialog.Show(
            new VmDialogParameters()
            {
                Title = title ?? VmDialogControllerExtensionsHelpers.TitleForIntent(type),
                Width = "min(98vw, 400px)",
                Icon = dialog.DefaultIconForIntent(type)
            },
            new MessageBoxViewModel()
            {
                Type = type,
                Message = message, MessageMarkup = messageMarkup,
                ActionConfirmText = confirmText ?? Resources.Actions_OK
            }
        );
        await dlg.WaitClosed;
    }
    
    /// <summary>
    /// Show a simple messagebox with the given message and intent.
    /// </summary>
    /// <param name="dialog">The dialog controller</param>
    /// <param name="message">The message body</param>
    /// <param name="type">See <see cref="MessageBoxType"/></param>
    /// <param name="title">The title in the dialog header</param>
    /// <param name="confirmText">
    /// The text of the confirmation button. Defaults to "OK".
    /// </param>
    /// <returns></returns>
    public static Task ShowMessageBox(
        this IVmDialogController dialog,
        string message,
        MessageBoxType type = MessageBoxType.Info,
        string? title = null, // default matches intent
        string? confirmText = null // default is "OK"
    ) => dialog.ShowMessageBox(title, type, message, null, confirmText);
    
    /// <summary>
    /// Like
    /// <see cref="ShowMessageBox(IVmDialogController, string, MessageBoxType, string?, string?)"/>,
    /// but allows for markup in the message body. <b>Beware of XSS vulnerabilities</b>.
    /// </summary>
    public static Task ShowMessageBox(
        this IVmDialogController dialog,
        MarkupString message,
        MessageBoxType type = MessageBoxType.Info,
        string? title = null, // default matches intent
        string? confirmText = null // default is "OK"
    ) => dialog.ShowMessageBox(title, type, null, message, confirmText);
    
    /// <summary>
    /// Like
    /// <see cref="ShowMessageBox(IVmDialogController, string, MessageBoxType, string?, string?)"/>,
    /// but with the intent set to "Success" and the default title matching that intent.
    /// </summary>
    public static Task ShowSuccess(
        this IVmDialogController dialog,
        string message,
        string? title = null, // default matches intent
        string? confirmText = null // default is "OK"
    ) => dialog.ShowMessageBox(title, MessageBoxType.Success, message, null, confirmText);
    
    /// <summary>
    /// Like
    /// <see cref="ShowMessageBox(IVmDialogController, MarkupString, MessageBoxType, string?, string?)"/>,
    /// but with the intent set to "Success" and the default title matching that intent.
    /// <b>Beware of XSS vulnerabilities</b>.
    /// </summary>
    public static Task ShowSuccess(
        this IVmDialogController dialog,
        MarkupString message,
        string? title = null, // default matches intent
        string? confirmText = null // default is "OK"
    ) => dialog.ShowMessageBox(title, MessageBoxType.Success, null, message, confirmText);
    
    /// <summary>
    /// Like
    /// <see cref="ShowMessageBox(IVmDialogController, string, MessageBoxType, string?, string?)"/>,
    /// but with the intent set to "Warning" and the default title matching that intent.
    /// </summary>
    public static Task ShowWarning(
        this IVmDialogController dialog,
        string message,
        string? title = null, // default matches intent
        string? confirmText = null // default is "OK"
    ) => dialog.ShowMessageBox(title, MessageBoxType.Warning, message, null, confirmText);
    
    /// <summary>
    /// Like
    /// <see cref="ShowMessageBox(IVmDialogController, MarkupString, MessageBoxType, string?, string?)"/>,
    /// but with the intent set to "Warning" and the default title matching that intent.
    /// <b>Beware of XSS vulnerabilities</b>.
    /// </summary>
    public static Task ShowWarning(
        this IVmDialogController dialog,
        MarkupString message,
        string? title = null, // default matches intent
        string? confirmText = null // default is "OK"
    ) => dialog.ShowMessageBox(title, MessageBoxType.Warning, null, message, confirmText);
    
    /// <summary>
    /// Like
    /// <see cref="ShowMessageBox(IVmDialogController, MarkupString, MessageBoxType, string?, string?)"/>,
    /// but with the intent set to "Information" and the default title matching that intent.
    /// <b>Beware of XSS vulnerabilities</b>.
    /// </summary>
    public static Task ShowInfo(
        this IVmDialogController dialog,
        MarkupString message,
        string? title = null, // default matches intent
        string? confirmText = null // default is "OK"
    ) => dialog.ShowMessageBox(title, MessageBoxType.Info, null, message, confirmText);
    
    /// <summary>
    /// Like
    /// <see cref="ShowMessageBox(IVmDialogController, string, MessageBoxType, string?, string?)"/>,
    /// but with the intent set to "Information" and the default title matching that intent.
    /// </summary>
    public static Task ShowInfo(
        this IVmDialogController dialog,
        string message,
        string? title = null, // default matches intent
        string? confirmText = null // default is "OK"
    ) => dialog.ShowMessageBox(title, MessageBoxType.Info, message, null, confirmText);
    
    /// <summary>
    /// Like
    /// <see cref="ShowMessageBox(IVmDialogController, string, MessageBoxType, string?, string?)"/>,
    /// but with the intent set to "Error" and the default title matching that intent.
    /// </summary>
    public static Task ShowError(
        this IVmDialogController dialog,
        string message,
        string? title = null, // default matches intent
        string? confirmText = null // default is "OK"
    ) => dialog.ShowMessageBox(title, MessageBoxType.Error, message, null, confirmText);
    
    /// <summary>
    /// Like
    /// <see cref="ShowMessageBox(IVmDialogController, MarkupString, MessageBoxType, string?, string?)"/>,
    /// but with the intent set to "Error" and the default title matching that intent.
    /// <b>Beware of XSS vulnerabilities</b>.
    /// </summary>
    public static Task ShowError(
        this IVmDialogController dialog,
        MarkupString message,
        string? title = null, // default matches intent
        string? confirmText = null // default is "OK"
    ) => dialog.ShowMessageBox(title, MessageBoxType.Error, null, message, confirmText);
}