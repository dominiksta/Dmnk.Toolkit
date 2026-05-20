---
uid: Dmnk.Blazor.Mvvm.SourceGen
---

(This is not a separate NuGet package, but built into in the main Dmnk.Blazor.Mvvm package)

Use source-generation to register ViewModels with their Views, such that they can be used with 
[<RegisteredViewFor>](https://dominiksta.github.io/Dmnk.Toolkit//api/Dmnk.Blazor.Mvvm.RegisteredViewFor-1.html).

Both closed and open-generic types are supported.

## Usage

**MyViewModel.cs**:

```csharp
// NOTE: MyView must either be in the same namespace as MyViewModel, or you must specify the full
// namespace in the attribute. This is a limitation of using source-generation for razor components.
[ViewModelFor(typeof(MyView))]
public class MyViewModel : INotifyPropertyChanged { ... }

// Open-generic types are also supported:
[ViewModelFor(typeof(MyGenericView<>))]
public class MyGenericViewModel<T> : INotifyPropertyChanged { ... }
```

**MyView.razor**:

```razor
@inherits MvvmComponentBase<MyViewModel>
@* ... *@
```

**Program.cs**:

```csharp
// Call the generated Register() method, then register the registry itself:
My.Namespace.Containing.MyViewModel.SourceGeneratedViewModelRegistrations.Register(services);
services.AddSingleton<IViewModelRegistry, ViewModelRegistry>();
```

**MyDynamicView.razor**:

```razor
<RegisteredViewFor Vm="myViewModel" />
```