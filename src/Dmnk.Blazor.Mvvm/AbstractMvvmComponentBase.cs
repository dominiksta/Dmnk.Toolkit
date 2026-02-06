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
