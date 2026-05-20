using System.Drawing;
using Dmnk.Icons.Core;
using Microsoft.AspNetCore.Components;

namespace Dmnk.Icons.Blazor;

/// <summary>
/// Allows defining a custom blazor-only icon directly as a markup string.
/// This is useful especially for using Icon libraries that were designed for Blazor and don't have
/// a direct SVG representation, but can be rendered as a Blazor component or markup string.
/// </summary>
public abstract class CustomIconDefinition(string technicalName) : IconDefinition(technicalName)
{
    /// <summary>
    /// Return a <see cref="MarkupString"/> that represents this icon to render in the DOM.
    /// </summary>
    /// <param name="color">
    /// An optional color to apply. If null, the definition's own default color is used.
    /// </param>
    /// <param name="size">
    /// An optional size in pixels to render at. If null, the definition's own default size is used.
    /// </param>
    public abstract MarkupString ToMarkup(Color? color = null, int? size = null);
}