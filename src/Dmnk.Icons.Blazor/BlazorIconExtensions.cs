using Dmnk.Icons.Core;
using Microsoft.AspNetCore.Components;

namespace Dmnk.Icons.Blazor;

public static class BlazorIconExtensions
{
    public static string DefaultColor { get; set; } = "var(--accent-fill-rest)";
    
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
            _ => new MarkupString("<div color="red">ICON_TYPE_ERROR</div>")
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
}