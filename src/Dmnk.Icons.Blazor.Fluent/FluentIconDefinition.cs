using Dmnk.Icons.Core;
using Microsoft.AspNetCore.Components;
using Fluent = Microsoft.FluentUI.AspNetCore.Components;

namespace Dmnk.Icons.Blazor.Fluent;

internal class FluentIconDefinition(
    Fluent::Icon fluentIcon
) : CustomIconDefinition(fluentIcon.Name)
{
    public override MarkupString ToMarkup(System.Drawing.Color? color = null, int? size = null)
    {
        var cssSize = size.HasValue ? $"{size.Value}px" : null;
        var markup = fluentIcon.ToMarkup(size: cssSize, color: color?.ToHexString()).Value;
        // The Fluent ToMarkup() hard-codes "background-color: var(--neutral-layer-1);" into the SVG
        // style attribute.
        markup = markup.Replace("background-color: var(--neutral-layer-1); ", string.Empty);
        return new MarkupString(markup);
    }
}