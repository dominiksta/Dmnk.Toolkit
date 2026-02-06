---
_layout: landing
---

[//]: # (This file is shown as the root of the documentation website)
[//]: # (Do not use GitHub flavored markdown syntax here, as it will be rendered by DocFX and not GitHub)

# Dmnk.Toolkit

Mostly small Blazor-related C# libraries. Documentation and Code are Work-In-Progress.

## Index

### MVVM Pattern for Blazor

- <xref:Dmnk.Blazor.Mvvm> - Base types to implement the MVVM pattern in Blazor applications.
- <xref:Dmnk.Blazor.Dialogs> - Base types to implement MVVM-based dialogs for Blazor applications, including support for
  nested dialogs and dialog results and some default dialogs like messageboxes.
- <xref:Dmnk.Blazor.Dialogs.Fluent> - Fluent UI themed dialog implementation for use with <xref:Dmnk.Blazor.Dialogs>.
- <xref:Dmnk.Blazor.Dialogs.CommunityToolkit> - Dead-simple integration package for using the 
  [MVVM Community Toolkit](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/) 
  with <xref:Dmnk.Blazor.Dialogs>.

### General Utilities

- <xref:Dmnk.Blazor.Suspense> - Blazor components that allow showing placeholder content while waiting some asynchronous
  operation, including lazy WASM module loading, to complete.
- <xref:Dmnk.Blazor.Focus> - Blazor components and extensions to manage keyboard focus
- <xref:Dmnk.Blazor.Cookies> - Utility methods to read generic cookies as well as ASP.NET CultureInfo cookies in Blazor
  WASM applications.

### Icons

- <xref:Dmnk.Icons.Core> - Core library to define icons as SVG paths or PNGs in code.
- <xref:Dmnk.Icons.Blazor> - Allows using icons defined using <xref:Dmnk.Icons.Core> in Blazor applications by
  generating `MarkupString`s.
- <xref:Dmnk.Icons.Blazor.Fluent> - A set of Fluent UI icons for use with <xref:Dmnk.Icons.Blazor>.
