using Dmnk.Blazor.Dialogs.Api;
using Dmnk.Blazor.Dialogs.DefaultDialogs;
using Dmnk.Blazor.Dialogs.Properties;
using Dmnk.Icons.Core;
using Microsoft.AspNetCore.Components;

namespace Dmnk.Blazor.Dialogs;

/// <summary>
/// Helper methods for creating default dialogs.
/// </summary>
public static class VmDialogControllerExtensions
{
    public static async Task<(bool Cancelled, T Result)> ShowInput<T>(
        this IVmDialogController dialog,
        string title, 
        T? defaultValue = default,
        Func<T?, string?>? validate = null,
        Icon? icon = null
    )
    {
        var vm = new InputDialogViewModel<T>()
        {
            Value = defaultValue, Validate = validate
        };
        var dlg = await dialog.Show(
            new VmDialogParameters()
            {
                Title = title, 
                Icon = icon
            },
            vm
        );
        await dlg.WaitClosed;
        return (dlg.Cancelled, vm.Value!);
    }

    private static string TitleForIntent(MessageBoxType type) => type switch
    {
        MessageBoxType.Success => Resources.MessageIntent_Success,
        MessageBoxType.Error => Resources.MessageIntent_Error,
        MessageBoxType.Warning => Resources.MessageIntent_Warning,
        MessageBoxType.Confirmation => Resources.MessageBoxIntent_Confirmation,
        _ => Resources.MessageIntent_Info,
    };

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
                Title = title ?? TitleForIntent(type),
                Width = "min(98vw, 400px)",
                Icon = dialog.DefaultIconForIntent(type)
            },
            vm
        );
        await dlg.WaitClosed;
        return vm.Result;
    }

    private static async Task ShowMessageBox(
        this IVmDialogController dialog,
        string? title, MessageBoxType type, string? message, 
        MarkupString? messageMarkup, string? confirmText
    )
    {
        var dlg = await dialog.Show(
            new VmDialogParameters()
            {
                Title = title ?? TitleForIntent(type),
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
    
    #region MessageBox Overloads

    public static Task ShowSuccess(
        this IVmDialogController dialog,
        MarkupString message,
        string? title = null, // default matches intent
        string? confirmText = null // default is "OK"
    ) => dialog.ShowMessageBox(title, MessageBoxType.Success, null, message, confirmText);
    
    public static Task ShowSuccess(
        this IVmDialogController dialog,
        string message,
        string? title = null, // default matches intent
        string? confirmText = null // default is "OK"
    ) => dialog.ShowMessageBox(title, MessageBoxType.Success, message, null, confirmText);
    
    public static Task ShowWarning(
        this IVmDialogController dialog,
        MarkupString message,
        string? title = null, // default matches intent
        string? confirmText = null // default is "OK"
    ) => dialog.ShowMessageBox(title, MessageBoxType.Warning, null, message, confirmText);
    
    public static Task ShowWarning(
        this IVmDialogController dialog,
        string message,
        string? title = null, // default matches intent
        string? confirmText = null // default is "OK"
    ) => dialog.ShowMessageBox(title, MessageBoxType.Warning, message, null, confirmText);
    
    public static Task ShowInfo(
        this IVmDialogController dialog,
        MarkupString message,
        string? title = null, // default matches intent
        string? confirmText = null // default is "OK"
    ) => dialog.ShowMessageBox(title, MessageBoxType.Info, null, message, confirmText);
    
    public static Task ShowInfo(
        this IVmDialogController dialog,
        string message,
        string? title = null, // default matches intent
        string? confirmText = null // default is "OK"
    ) => dialog.ShowMessageBox(title, MessageBoxType.Info, message, null, confirmText);
    
    public static Task ShowError(
        this IVmDialogController dialog,
        MarkupString message,
        string? title = null, // default matches intent
        string? confirmText = null // default is "OK"
    ) => dialog.ShowMessageBox(title, MessageBoxType.Error, null, message, confirmText);
    
    public static Task ShowError(
        this IVmDialogController dialog,
        string message,
        string? title = null, // default matches intent
        string? confirmText = null // default is "OK"
    ) => dialog.ShowMessageBox(title, MessageBoxType.Error, message, null, confirmText);
    
    public static Task ShowMessageBox(
        this IVmDialogController dialog,
        string message,
        MessageBoxType type = MessageBoxType.Info,
        string? title = null, // default matches intent
        string? confirmText = null // default is "OK"
    ) => dialog.ShowMessageBox(title, type, message, null, confirmText);
    
    public static Task ShowMessageBox(
        this IVmDialogController dialog,
        MarkupString message,
        MessageBoxType type = MessageBoxType.Info,
        string? title = null, // default matches intent
        string? confirmText = null // default is "OK"
    ) => dialog.ShowMessageBox(title, type, null, message, confirmText);
    
    #endregion

    #region ShowConfirmation Overloads
    
    public static Task<bool> ShowConfirmation(
        this IVmDialogController dialog,
        string message,
        MessageBoxType type = MessageBoxType.Confirmation, 
        string? title = null, // default is based on intent
        string? confirmText = null, // defaults to yes
        string? denyText = null // defaults to no
    ) => dialog.ShowConfirmation(title, type, message, null, confirmText, denyText);
    
    public static Task<bool> ShowConfirmation(
        this IVmDialogController dialog,
        MarkupString message,
        MessageBoxType type = MessageBoxType.Confirmation, 
        string? title = null, // default is based on intent
        string? confirmText = null, // defaults to yes
        string? denyText = null // defaults to no
    ) => dialog.ShowConfirmation(title, type, null, message, confirmText, denyText);
    
    #endregion
}