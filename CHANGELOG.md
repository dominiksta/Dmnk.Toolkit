# Changelog

## `Dmnk.Blazor.Mvvm`: `0.0.9` - *unreleased*

**Added:**

- `IServiceCollection.AddBlazorMvvm()`, `IServiceCollection.AddViewModelRegistration()`
  and `IServiceCollection.AddViewModelRegistrationOpenGeneric()` extension methods for
  less verbose DI setup.

## `Dmnk.Blazor.Dialogs.Fluent`: `0.0.4` - *unreleased*

**Fixed**:

- `DisposeAsync()` of `DialogControllerProvider` would error on page refresh / tab close
- `BlazorVmDialogController`/`IVmDialogController` were registered as 
  singletons, causing issues with shared state between tabs in Blazor server
- Dialogs with `AllowCancel = false` could still be closed by hitting escape twice 
  (from the internal `OnDocumentKeyDownEscape` method) and would then hang the event handler

## `Dmnk.Blazor.Mvvm`: `0.0.8` - 2026-05-26

**Fixed:**

- `OptionalMvvmComponentBase`: When `Vm` was passed as a parameter (e.g. from a parent component),
  `PropertyChanged` events were never subscribed to, causing the UI to never update reactively.

## `Dmnk.Icons.Blazor`: `0.0.2` - 2026-05-29

**Fixed:**

- Colors applied through WithColor for CustomIconDefinitions were not applied.
- Size was not applied.

## `Dmnk.Icons.Core`: `0.0.2` - 2026-05-29

**Added**:

- SystemDrawingColorExtensions.ToHexString

## `Dmnk.Icons.Blazor.Fluent`: `0.0.2` - 2026-05-29

**Fixed:**

- Icons had a hard-coded `background-color: var(--neutral-layer-1)`. This is now removed.
- Colors applied through WithColor for CustomIconDefinitions were not applied.
- Icon sizes were not applied beyond the initial fluent icon size.

## `Dmnk.Blazor.Dialogs`: `0.0.2` - 2026-05-29

**Changed:**

- ViewModel registration is now done through DI (see type ViewModelRegistration).
  This is with the same mechanism as in Dmnk.Blazor.Mvvm and replaces the previous Register calls
  on the DialogControllers.

## `Dmnk.Blazor.Dialogs.Fluent`: `0.0.3` - 2026-05-29

**Fixed:**

- Alignment of Icon and Text in dialog headers

## `Dmnk.Blazor.Mvvm`: `0.0.7` - 2026-05-20

**Changed:**

- ViewModel Registrations now are done through DI (see type ViewModelRegistration).

**Added:**

- ViewModel Registrations now support open generics through the new `RegisterOpenGeneric` method,
  as well as through the existing `ViewModelFor` source generator.

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