---
uid: Dmnk.Icons.Blazor
---

![NuGet Version](https://img.shields.io/nuget/v/Dmnk.Icons.Blazor?style=flat-square&color=blue&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FDmnk.Icons.Blazor%2F)

Allow using icons defined using <xref:Dmnk.Icons.Core> in Blazor applications by generating
`MarkupString`s. See <xref:Dmnk.Icons.Blazor.BlazorIconExtensions>.

Color and size set on an `Icon` instance (via `Icon.WithColor()` / `Icon.Size`) are forwarded to
the rendered markup. For SVG icons the `viewBox` is taken from `SvgIconDefinition.ViewBox` (the
coordinate space of the path data), while `width`/`height` reflect the requested render size.

## Example

```csharp
public class MyIconDefinition : SvgIconDefinition("my-icon") 
{
    // ViewBox must match the coordinate space of your SVG path data.
    public override (int Width, int Height) ViewBox => (24, 24);
    public override string Svg => """<path d="some svg path here"/>""";
}

public class MyCustomBlazorIcon : CustomIconDefinition("custom-icon") 
{
    public override MarkupString ToMarkup(System.Drawing.Color? color = null, int? size = null) => 
        Some.Other.Blazor.Icon.Library.Icon1.AsMarkupString();
}

public static class MyIcons 
{
    // since these are getters, the compiler can strip out unused icons
    public static IconDefinition MyIcon => new MyIconDefinition();
    public static IconDefinition MyCustomBlazorIcon => new MyCustomBlazorIcon();
}
```

**Program.cs**:

```csharp
BlazorIconExtensions.DefaultColor = "black"; // or whatever css color value you want
```

```razor
@using Dmnk.Icons.Blazor

<div>@MyIcons.MyIcon.Size20.ToMarkupString()</div>

@* render at 48px in red *@
<div>@MyIcons.MyIcon.Size48.WithColor(System.Drawing.Color.Red).ToMarkupString()</div>

<div>@MyIcons.MyCustomBlazorIcon.Size48.ToMarkupString()</div>
```