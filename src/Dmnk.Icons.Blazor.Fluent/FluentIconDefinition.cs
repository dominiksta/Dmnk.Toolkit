using Microsoft.AspNetCore.Components;
using Fluent = Microsoft.FluentUI.AspNetCore.Components;

namespace Dmnk.Icons.Blazor.Fluent;

internal class FluentIconDefinition(
    Fluent::Icon fluentIcon
) : CustomIconDefinition(fluentIcon.Name)
{
    public override MarkupString ToMarkup() => fluentIcon.ToMarkup();
}