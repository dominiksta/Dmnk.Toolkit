---
uid: Dmnk.Blazor.Dialogs.Fluent
---

![NuGet Version](https://img.shields.io/nuget/v/Dmnk.Blazor.Dialogs.Fluent?style=flat-square&color=blue&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FDmnk.Blazor.Dialogs.Fluent%2F)

Provides a fluent design implementation for <xref:Dmnk.Blazor.Dialogs>.

- See <xref:Dmnk.Blazor.Dialogs.Fluent.DefaultDialogs> for how to implement a view for a given
  ViewModel.
- Register the <xref:Dmnk.Blazor.Dialogs.Fluent.FluentVmDialogController> in DI, register relevant
  views and viewmodels with it and finally inject it as a
  <xref:Dmnk.Blazor.Dialogs.Api.IVmDialogController> to display dialogs.
