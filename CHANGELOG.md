# Changelog

## `Dmnk.Blazor.Mvvm`: `0.0.3` - 2026-02-18

**Fixed:**

- `RegisteredViewFor` now works properly when passing a ViewModel cast to one of its base types, e.g. `IMyViewModel` or
  `INotifyPropertyChanged`

**Breaking Changes:**

- The source generator for `ViewModelFor` now generates a class *in the namespace of the project* that is using the
  `ViewModelFor` attribute instead of in `Dmnk.Blazor.Mvvm`. This fixes usage in multiple projects.

## `Dmnk.Blazor.Mvvm`: `0.0.2` - 2026-02-17

**Added:**

- Added ViewModelRegistry, RegisteredViewFor and ViewModelFor Source Generator

## `ALL`: `0.0.1` - 2026-02-10

- Initial release