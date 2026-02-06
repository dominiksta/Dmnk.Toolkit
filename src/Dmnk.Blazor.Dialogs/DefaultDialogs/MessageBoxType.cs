namespace Dmnk.Blazor.Dialogs.DefaultDialogs;

/// <summary>
/// The type of message box, which determines the icon and styling used by default.
/// </summary>
public enum MessageBoxType
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    Success, 
    Warning, 
    Error, 
    Info, 
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// A confirmation dialog, which is used for asking the user to confirm an action. It is similar
    /// to a warning, but with a different icon and styling. It is used by the
    /// <see cref="ConfirmationDialogViewModel"/> and the ShowConfirmation methods of
    /// <see cref="VmDialogControllerExtensionsHelpers"/>.
    /// </summary>
    Confirmation
}