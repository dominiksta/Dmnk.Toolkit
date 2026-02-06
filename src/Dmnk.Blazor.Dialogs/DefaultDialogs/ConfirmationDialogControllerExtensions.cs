using Dmnk.Blazor.Dialogs.Api;
using Dmnk.Blazor.Dialogs.Properties;
using Microsoft.AspNetCore.Components;

namespace Dmnk.Blazor.Dialogs.DefaultDialogs;

/// <summary>
/// Some small extension methods to make showing confirmation dialogs (see <see
/// cref="ConfirmationDialogViewModel"/>) easier.
/// </summary>
public static class ConfirmationDialogControllerExtensions
{
    private static async Task<bool> ShowConfirmation(
        this IVmDialogController dialog,
        string? title, MessageBoxType type, string? message,
        MarkupString? messageMarkup, string? confirmText, string? denyText
    )
    {
        var vm = new ConfirmationDialogViewModel()
        {
            Type = type,
            Message = message, MessageMarkup = messageMarkup,
            ActionConfirmText = confirmText ?? Resources.Generic_Yes,
            ActionDenyText = denyText ?? Resources.Generic_No
        };
        var dlg = await dialog.Show(
            new VmDialogParameters()
            {
                Title = title ?? VmDialogControllerExtensionsHelpers.TitleForIntent(type),
                Width = "min(98vw, 400px)",
                Icon = dialog.DefaultIconForIntent(type)
            },
            vm
        );
        await dlg.WaitClosed;
        return vm.Result;
    }

    /// <summary>
    /// Show a confirmation dialog with the given message and intent. The returned task will
    /// complete with true if the user confirmed, false if they denied (or dismissed).
    /// </summary>
    /// <param name="dialog">The dialog controller instance</param>
    /// <param name="message">The message/question shown to the user</param>
    /// <param name="type">See <see cref="MessageBoxType"/></param>
    /// <param name="title">
    /// The title in the dialog header. Default is set based on the <c>type</c>.
    /// </param>
    /// <param name="confirmText">
    /// The text on the confirm button. Defaults to "Yes".
    /// </param>
    /// <param name="denyText">
    /// The text on the deny button. Defaults to "No".
    /// </param>
    public static Task<bool> ShowConfirmation(
        this IVmDialogController dialog,
        string message,
        MessageBoxType type = MessageBoxType.Confirmation, 
        string? title = null, // default is based on intent
        string? confirmText = null,
        string? denyText = null
    ) => dialog.ShowConfirmation(title, type, message, null, confirmText, denyText);
    
    /// <summary>
    /// Show a confirmation dialog with the given message and intent. The returned task will
    /// complete with true if the user confirmed, false if they denied (or dismissed).
    /// <p>
    /// Like
    /// <see cref="ShowConfirmation(IVmDialogController,string,MessageBoxType,string?,string?,string?)"/>,
    /// but allows for markup in the message. As always, <b>beware of XSS vulnerabilities</b>.
    /// </p>
    /// </summary>
    /// <param name="dialog">The dialog controller instance</param>
    /// <param name="message">The message/question shown to the user</param>
    /// <param name="type">See <see cref="MessageBoxType"/></param>
    /// <param name="title">
    /// The title in the dialog header. Default is set based on the <c>type</c>.
    /// </param>
    /// <param name="confirmText">
    /// The text on the confirm button. Defaults to "Yes".
    /// </param>
    /// <param name="denyText">
    /// The text on the deny button. Defaults to "No".
    /// </param>
    public static Task<bool> ShowConfirmation(
        this IVmDialogController dialog,
        MarkupString message,
        MessageBoxType type = MessageBoxType.Confirmation, 
        string? title = null, // default is based on intent
        string? confirmText = null, // defaults to yes
        string? denyText = null // defaults to no
    ) => dialog.ShowConfirmation(title, type, null, message, confirmText, denyText);
}