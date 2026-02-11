---
uid: Dmnk.Icons.Blazor
---

![NuGet Version](https://img.shields.io/nuget/v/Dmnk.Icons.Blazor?style=flat-square&color=blue&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FDmnk.Icons.Blazor%2F)

Allow using icons defined using <xref:Dmnk.Icons.Core> in Blazor applications by generating
`MarkupString`s. See <xref:Dmnk.Icons.Blazor.BlazorIconExtensions>.

## Example

```csharp
public class MyIconDefinition : SvgIconDefinition("my-icon") 
{
    public override string Svg => """<path d="some svg path here"/>""";
}

public class MyCustomBlazorIcon : CustomIconDefinition("custom-icon") 
{
    public override MarkupString ToMarkup() => 
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

<div>@MyIcons.MyCustomBlazorIcon.Size48.ToMarkupString()</div>
```