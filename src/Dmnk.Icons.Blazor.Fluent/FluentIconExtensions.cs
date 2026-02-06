using System.Drawing;
using Dmnk.Icons.Core;
using Fluent = Microsoft.FluentUI.AspNetCore.Components;

namespace Dmnk.Icons.Blazor.Fluent;

/// <summary>
/// Provides extension methods for converting Fluent UI icons to the generic <see cref="Icon"/>
/// type in <see cref="Dmnk.Icons.Core"/>.
/// </summary>
public static class FluentIconExtensions
{
    /// <summary>
    /// Converts a Fluent UI <see cref="Fluent::Icon"/> to a generic
    /// <see cref="Dmnk.Icons.Core.Icon"/>.
    /// </summary>
    public static Icon ToGenericIcon(this Fluent::Icon fluentIcon)
    {
        return new Icon(
            new FluentIconDefinition(fluentIcon), 
            new Size((int) fluentIcon.Size, (int) fluentIcon.Size)
        );
    }
}