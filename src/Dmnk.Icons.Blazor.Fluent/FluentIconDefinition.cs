using Microsoft.AspNetCore.Components;
using Fluent = Microsoft.FluentUI.AspNetCore.Components;

namespace Dmnk.Icons.Blazor.Fluent;

internal class FluentIconDefinition(
    Fluent::Icon fluentIcon
) : CustomIconDefinition(fluentIcon.Name)
{
    public override MarkupString ToMarkup()
    {
        var markup = fluentIcon.ToMarkup().Value;
        // The Fluent ToMarkup() hard-codes "background-color: var(--neutral-layer-1);" into the SVG
        // style attribute.
        markup = markup.Replace("background-color: var(--neutral-layer-1); ", string.Empty);
        return new MarkupString(markup);
    }
}