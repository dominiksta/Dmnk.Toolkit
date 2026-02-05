using System.Drawing;
using Dmnk.Icons.Core;
using Fluent = Microsoft.FluentUI.AspNetCore.Components;

namespace Dmnk.Icons.Blazor.Fluent;

public static class FluentIconExtensions
{
    public static Icon ToIcon(this Fluent::Icon fluentIcon)
    {
        return new Icon(
            new FluentIconDefinition(fluentIcon), 
            new Size((int) fluentIcon.Size, (int) fluentIcon.Size)
        );
    }
}