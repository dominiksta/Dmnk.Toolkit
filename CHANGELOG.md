# Changelog

## `Dmnk.Icons.Blazor`: `0.0.2` - [unreleased]

**Fixed:**

- Colors applied through WithColor for CustomIconDefinitions were not applied.
- Size was not applied.

## `Dmnk.Icons.Core`: `0.0.2` - [unreleased]

**Added**:

- SystemDrawingColorExtensions.ToHexString

## `Dmnk.Icons.Blazor.Fluent`: `0.0.2` - [unreleased]

**Fixed:**

- Icons had a hard-coded `background-color: var(--neutral-layer-1)`. This is now removed.
- Colors applied through WithColor for CustomIconDefinitions were not applied.
- Icon sizes were not applied beyond the initial fluent icon size.

## `Dmnk.Blazor.Mvvm`: `0.0.7` - [unreleased]

**Changed:**

- ViewModel Registrations now are done through DI (see type ViewModelRegistration).

## `Dmnk.Blazor.Mvvm`: `0.0.6` - 2026-03-06

**Fixed:**

- AbstractMvvmComponentBase now implements both IDisposable and IAsyncDisposable (as virtual methods).

## `[Multiple]`:  2026-02-19

- `Dmnk.Blazor.OverlayScrollbar`: 0.0.2
- `Dmnk.Blazor.Mvvm`: 0.0.5
- `Dmnk.Blazor.Dialogs.Fluent`: 0.0.2

**Fixed:**

- `Dmnk.Blazor.OverlayScrollbar`: The JS and CSS files were not properly added to the nuget package. 
  They did exist but not in the `staticwebassets` folder where they belong.
- `Dmnk.Blazor.Mvvm`: Views now respond to CanExecuteChanged events of commands that are defined as fields 
  instead of properties.
- `Dmnk.Blazor.Dialogs.Fluent`: Fixed JS file location

## `Dmnk.Blazor.Mvvm`: `0.0.4` - 2026-02-18

**Fixed:**

- `RegisteredViewFor` now properly updates the view when the ViewModel changes, e.g. using in a `@foreach`

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