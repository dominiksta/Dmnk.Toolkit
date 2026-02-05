using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Dmnk.Blazor.Focus;

public static class ElementReferenceExtensions
{
    private const string JsClass = "window.__Dmnk_Blazor_Focus";
    
    /// <summary>
    /// Sometimes a simple <c>FocusAsync</c> is not enough to focus an element
    /// because there are multiple components fighting over the focus
    /// asynchronously. While that may be something that in theory should be
    /// fixed at its root, this is often rather difficult
    /// <p>
    /// This method fights the symptoms by simply re-focusing the element
    /// <c>n</c> times (with a delay of 10ms) until it is focused.
    /// </p>
    /// </summary>
    public static ValueTask ForceFocus(
        this ElementReference el,
        IJSRuntime js,
        int retries = 5,
        CancellationToken ct = default
    ) => js.InvokeVoidAsync($"{JsClass}.ForceFocus", ct, el, retries);

    /// <summary>
    /// Store the currently focused element (<c>document.activeElement</c>) as a
    /// js field of the given element. You can call <see cref="RestoreFocus"/>
    /// to then restore focus to that element later.
    /// </summary>
    public static ValueTask SaveFocus(
        this ElementReference el, IJSRuntime js, CancellationToken ct = default
    ) => js.InvokeVoidAsync($"{JsClass}.SaveFocus", ct, el.Id);

    /// <summary> <see cref="SaveFocus"/> </summary>
    public static ValueTask RestoreFocus(
        this ElementReference el, IJSRuntime js, CancellationToken ct = default
    ) => js.InvokeVoidAsync($"{JsClass}.RestoreFocus", ct, el.Id);

    /// <summary>
    /// Focuses the last "focusable" element child. This includes inputs,
    /// selects, buttons, inputs, etc. Elements with tabindex="-1" are filtered.
    /// See also <see cref="FocusFirstFocusableChild"/>.
    /// </summary>
    public static ValueTask FocusLastFocusableChild(
        this ElementReference el, IJSRuntime js, CancellationToken ct = default
    ) => js.InvokeVoidAsync($"{JsClass}.FocusLast", ct, el);

    /// <summary>
    /// Focuses the first "focusable" element child. This includes inputs,
    /// selects, buttons, inputs, etc. Elements with tabindex="-1" are filtered.
    /// See also <see cref="FocusFirstFocusableChild"/>.
    /// </summary>
    public static ValueTask FocusFirstFocusableChild(
        this ElementReference el, IJSRuntime js, CancellationToken ct = default
    ) => js.InvokeVoidAsync($"{JsClass}.FocusFirst", ct, el);
}