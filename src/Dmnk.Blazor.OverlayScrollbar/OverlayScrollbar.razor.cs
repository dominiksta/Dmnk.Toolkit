namespace Dmnk.Blazor.OverlayScrollbar;

/// <summary>
/// Apply a fancier scrollbar to the child elements. This creates a container
/// element with <c>overflow: auto;</c> and <c>height: 100%; width: 100%;</c>.
/// You can overwrite those styles using the <see cref="Style"/> property.
/// <p>
/// The only thing you <em>can't</em> overwrite is that the container must
/// remain as <c>display: flex</c>.
/// </p>
/// <p>
/// Realistically, you also want to add the <c>data-overlayscrollbars-initialize</c>
/// attribute to your <c>html</c> and <c>body</c> tags. This will prevent a
/// flash of unstyled content for the scrollbars on page load.
/// </p>
/// <p>
/// This is implemented using the OverlayScrollbar library:
/// <see href="https://kingsora.github.io/OverlayScrollbars/"/>.
/// </p>
/// </summary>
public partial class OverlayScrollbar { }