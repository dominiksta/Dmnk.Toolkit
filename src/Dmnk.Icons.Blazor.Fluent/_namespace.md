---
uid: Dmnk.Icons.Blazor.Fluent
---

![NuGet Version](https://img.shields.io/nuget/v/Dmnk.Icons.Blazor.Fluent?style=flat-square&color=blue&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FDmnk.Icons.Blazor.Fluent%2F)

Provide icon definitions using <xref:Dmnk.Icons.Core> for the Fluent design system, or more
specifically, the [Blazor Fluent UI components](https://www.fluentui-blazor.net/Icon). Since this
library is based off of a Blazor component library, it only works in Blazor applications. In theory,
it could be refactored using code-generation to pull icons from the [official main repo for Fluent
icons](https://github.com/microsoft/fluentui-system-icons/).

## Example

```csharp
using MsIcons = Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size24;
using Icon = Dmnk.Icons.Core.Icon;

public class MyClassUsingDmnkIcons 
{
    public required Icon Icon { get; init; }
}

internal static class Program
{
    static void Main(string[] args)
    {
        var myObj = new MyClassUsingDmnkIcons 
        {
            Icon = MsIcons.Warning().ToGenericIcon()
        };
    }
}
```