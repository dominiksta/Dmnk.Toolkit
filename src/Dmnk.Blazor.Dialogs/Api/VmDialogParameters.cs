using Dmnk.Icons.Core;
using Microsoft.AspNetCore.Components;

namespace Dmnk.Blazor.Dialogs.Api;

/// <summary>
/// Core parameters that apply to all dialogs.
/// </summary>
public class VmDialogParameters
{
    /// <summary>
    /// The title that will be shown in the dialog header.
    /// </summary>
    public string? Title { get; init; }
    /// <summary>
    /// USE ONLY WITH TRUSTED DATA. Like <see cref="Title"/>, but allows for arbitrary HTML markup.
    /// Using untrusted data here can lead to XSS vulnerabilities.
    /// </summary>
    public MarkupString? TitleMarkup { get; init; }
    /// <summary>
    /// Define the icon to be shown in the dialog header.
    /// </summary>
    public Icon? Icon { get; init; }
    /// <summary>
    /// Needs to be a valid value for the CSS <c>width</c> property.
    /// <p>
    /// This also means you can set computed values such as <c>min(100vw,
    /// 700px)</c> to set a maximum value.
    /// </p>
    /// </summary>
    public string Width { get; init; } = "min(98vw, 500px)";
    /// <summary> See <see cref="Width"/> </summary>
    public string Height { get; init; } = "auto";
    /// <summary>
    /// Whether to allow the user to dismiss the dialoge by clicking on a
    /// generic "X" close button or hitting escape. If this is set to false, the
    /// dialog can only be closed with the
    /// <see cref="IVmDialogView.Dialog"/> property of the viewmodel.
    /// </summary>
    public bool AllowCancel { get; init; } = true;
}