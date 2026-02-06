using Dmnk.Icons.Core;
using Microsoft.AspNetCore.Components;

namespace Dmnk.Icons.Blazor;

/// <summary>
/// Helpers to convert <see cref="Icon"/>s to Blazor <see cref="MarkupString"/>s for rendering in
/// Blazor components.
/// </summary>
public static class BlazorIconExtensions
{
    /// <summary>
    /// Set this globally to change the default color used for icons that don't specify a color. The
    /// default is "var(--accent-fill-rest)", which is the default accent color in Fluent UI. You
    /// can set this to any valid CSS color value, e.g. "red", "#ff0000", "rgb(255, 0, 0)", etc.
    /// </summary>
    public static string DefaultColor { get; set; } = "var(--accent-fill-rest)";
    
    /// <summary>
    /// Convert a generic <see cref="Icon"/> to a Blazor <see cref="MarkupString"/> that can be
    /// rendered in a Blazor component. The conversion is done based on the type of the icon
    /// definition, e.g. if it's an SVG icon, a <code>&lt;svg&gt;</code> element will be created, if
    /// it's a PNG icon, an <code>&lt;img&gt;</code> element will be created, etc. If the icon
    /// definition type is not supported, a red error message will be rendered instead (in release
    /// mode) or an exception will be thrown (in debug mode).
    /// </summary>
    public static MarkupString ToMarkup(this Icon icon)
    {
        return icon.Definition switch 
        {
            SvgIconDefinition svgIconDefinition => FromSvg(icon, svgIconDefinition),
            PngIconDefinition pngIconDefinition => FromPng(icon, pngIconDefinition),
            CustomIconDefinition customIconDefinition => customIconDefinition.ToMarkup(),
            #if DEBUG
            _ => throw new NotSupportedException(
                $"Icon definition of type {icon.GetType().FullName} is not supported in Blazor."
            )
            #else
            _ => new MarkupString("""<div color="red">ICON_TYPE_ERROR</div>""")
            #endif
        };
    }
    
    private static MarkupString FromPng(Icon icon, PngIconDefinition def)
    {
        var base64 = Convert.ToBase64String(def.Png);
        return new MarkupString(
            // language=html
            $"""
             <img 
                 src="data:image/png;base64,{base64}" 
                 width="{icon.Size.Width}" 
                 height="{icon.Size.Height}" 
                 alt="{icon.AccessibleName}"/>
             """
        );
    }
    
    private static MarkupString FromSvg(Icon icon, SvgIconDefinition def)
    {
      return new MarkupString(
          // language=svg
          $"""
           <svg viewBox="0 0 {icon.Size.Width} {icon.Size.Height}" 
               width="{icon.Size.Width}" 
               height="{icon.Size.Height}"
               fill="{icon.Color?.ToHexString() ?? DefaultColor}" 
               alt="{icon.AccessibleName}">
               {def.Svg}
           </svg>
           """
      );
    }

    private static string ToHexString(this System.Drawing.Color color) => 
        $"#{color.R:X2}{color.G:X2}{color.B:X2}";
}