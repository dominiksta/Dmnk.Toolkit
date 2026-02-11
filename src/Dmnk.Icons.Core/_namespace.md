---
uid: Dmnk.Icons.Core
---

![NuGet Version](https://img.shields.io/nuget/v/Dmnk.Icons.Core?style=flat-square&color=blue&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FDmnk.Icons.Core%2F)

Completely cross-platform icon definitions. These definitions should work with minimal effort in all
common .NET UI frameworks. For now, only Blazor has explicit support, but since all icons must be
defined as either SVG or PNG, rendering should not pose any issues in other frameworks.

This library was created specifically to support the use in Blazor WASM, where using resource files
is not ideal since they compile all Icons directly into the assembly, increasing bundle size. With
code-first definitions, the compiler can strip the unused icons, keeping the bundle size small.
There seemed to be no existing library that provides this functionality beyond specific icon packs
from specific design systems, so this library was created to allow defining Icons in a generic way
(Blazor) ViewModels.
