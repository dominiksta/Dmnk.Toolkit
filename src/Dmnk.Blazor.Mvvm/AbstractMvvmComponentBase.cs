using System.ComponentModel;
using System.Windows.Input;
using Microsoft.AspNetCore.Components;

#pragma warning disable CS0618 // Type or member is obsolete

namespace Dmnk.Blazor.Mvvm;

/// <summary>
/// Invokes StateHasChanged when ViewModel fires INotifyPropertyChanged *and*
/// when any property of type ICommand fires CanExecuteChanged.
/// </summary>
/// <typeparam name="T">The ViewModel</typeparam>
public abstract class AbstractMvvmComponentBase<T> : ComponentBase, IDisposable 
    where T : INotifyPropertyChanged
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once MemberCanBeProtected.Global
    internal abstract T? __ViewModel { get; set; }
    
    // ReSharper disable once InconsistentNaming
    internal readonly Dictionary<ICommand, Action> __CommandBindings = new();
    
    /// <summary>
    /// Allow registering asynchronous command bindings. Useful to integrate with MVVM libraries
    /// because regrettably, the standard ICommand does not support async execution, which means
    /// that MVVM libraries typically have their own types for async commands. Since we do not
    /// want to depend on any specific MVVM library here, this dictionary can be used to cache
    /// lambdas created from such async commands.
    /// </summary>
    [Obsolete(
        """
        Not actually obsolete, but do NOT use this directly in a component.
        Build some extension methods instead, like in Dmnk.Blazor.Mvvm.CommunityToolkit.
        """
    )]
    // ReSharper disable once InconsistentNaming
    public readonly Dictionary<object, Func<Task>> __CommandBindingsAsync = new();
    
    private List<ICommand> GetCommandCanExecuteChanged(T viewModel)
    {
        List<ICommand> ret = [];
        foreach (var prop in typeof(T).GetProperties())
        {
            if (prop.PropertyType != typeof(ICommand) 
                && !prop.PropertyType.IsAssignableTo(typeof(ICommand))) continue;
            ret.Add((ICommand) prop.GetValue(viewModel)!);
        }
        return ret;
    }

    private List<ICommand> _commands = [];

    internal void SetViewModel(T? viewModel)
    {
        if (EqualityComparer<T?>.Default.Equals(__ViewModel, viewModel)) return;
        
        foreach (var command in _commands)
            command.CanExecuteChanged -= OnCanExecuteChanged;

        _commands = viewModel == null 
            ? [] : GetCommandCanExecuteChanged(viewModel);
        
        foreach (var command in _commands)
            command.CanExecuteChanged += OnCanExecuteChanged;
        
        if (__ViewModel != null) __ViewModel.PropertyChanged -= OnPropertyChanged;
        __ViewModel = viewModel;
        if (__ViewModel != null) __ViewModel.PropertyChanged += OnPropertyChanged;
        InvokeAsync(StateHasChanged);
    }

    /// <summary> <inheritdoc/> </summary>
    protected override void OnInitialized()
    {
        if (__ViewModel == null) return;
        SetViewModel(__ViewModel);
    }

    private void OnCanExecuteChanged(object? sender, EventArgs args) 
        => InvokeAsync(StateHasChanged);

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs args) 
        => InvokeAsync(StateHasChanged);

    /// <summary> <inheritdoc/> </summary>
    public virtual void Dispose()
    {
        if (__ViewModel == null) return;
        SetViewModel(default);
        GC.SuppressFinalize(this);
    }
}
