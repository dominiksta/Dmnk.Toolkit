using Dmnk.Icons.Core;
using Microsoft.AspNetCore.Components;

namespace Dmnk.Blazor.Dialogs.Api;

public class VmDialogParameters
{
    public string? Title { get; init; }
    public MarkupString? TitleMarkup { get; init; }
    public Icon? Icon { get; set; }
    /// <summary>
    /// Needs to be a valid value for the CSS <c>width</c> property.
    /// <p>
    /// This also means you can set computed values such as <c>min(100vw,
    /// 700px)</c> to set a maximum value.
    /// </p>
    /// </summary>
    public string Width { get; set; } = "min(98vw, 500px)";
    /// <summary> See <see cref="Width"/> </summary>
    public string Height { get; set; } = "auto";
    /// <summary>
    /// Whether to allow the user to dismiss the dialoge by clicking on a
    /// generic "X" close button or hitting escape. If this is set to false, the
    /// dialog can only be closed with the
    /// <see cref="IVmDialogView.Dialog"/> property of the viewmodel.
    /// </summary>
    public bool AllowCancel { get; init; } = true;
}