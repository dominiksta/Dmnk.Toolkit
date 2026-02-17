---
uid: Dmnk.Blazor.Mvvm.SourceGen
---

(This is not a separate NuGet package, but built into in the main Dmnk.Blazor.Mvvm package)

Use source-generation to register ViewModels with their Views, such that they can be used with 
[<RegisteredViewFor>](https://dominiksta.github.io/Dmnk.Toolkit//api/Dmnk.Blazor.Mvvm.RegisteredViewFor-1.html).

## Usage

**MyViewModel.cs**:

```csharp
// NOTE: MyView must either be in the same namespace as MyViewModel, or you must specify the full
// namespace in the attribute. This is a limitation of using source-generation for razor components.
[ViewModelFor(typeof(MyView))]
public class MyViewModel : INotifyPropertyChanged { ... }
```

**MyView.razor**:

```razor
@inherits MvvmComponentBase<MyViewModel>
@* ... *@
```

**Program.cs**:

```csharp
// ...
ServiceProvider provider = services.BuildServiceProvider();

provider.GetRequiredService<IViewModelRegistry>().AutoRegisterFromSourceGen();
```

**MyDynamicView.razor**:

```razor
<RegisteredViewFor Vm="myViewModel" />
```