using System.ComponentModel;
using Microsoft.Extensions.Logging;

namespace Dmnk.Blazor.Mvvm;

/// <summary>
/// Blazor component that serves as a marker for associating a ViewModel type with a View type in the
/// <see cref="ViewModelRegistry"/>.
/// </summary>
public partial class RegisteredViewFor<TViewModel>(
    IViewModelRegistry? viewModelRegistry = null,
    ILogger<IViewModelRegistry>? logger = null)
    where TViewModel : INotifyPropertyChanged;