namespace Dmnk.Blazor.Focus;

/// <summary>
/// Prevents using the "Tab" key to escape the child components focus. This is
/// mainly useful for dialogs, which should trap the focus while they are
/// active.
///
/// <p>
/// For <em>accessibility</em>, you should <b>ALWAYS</b> provide a way to exit
/// the focus trap, e.g. with a keyboard handler on the escape key.
/// </p>
/// <p>
/// This implemetation is essentially a port of
/// <see href="https://github.com/MudBlazor/MudBlazor/blob/dev/src/MudBlazor/Components/FocusTrap/MudFocusTrap.razor">MudFocusTrap</see>.
/// </p>
/// </summary>
public partial class FocusTrap { }