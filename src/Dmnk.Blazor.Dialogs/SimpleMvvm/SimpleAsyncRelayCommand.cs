using System.Windows.Input;

namespace Dmnk.Blazor.Dialogs.SimpleMvvm;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Internal basic MVVM implementation, just to not depend on e.g. the CommunityToolkit in the core
/// library. Not intended for public use, use a proper MVVM library.
/// </summary>
public class SimpleAsyncRelayCommand(
    Func<object?, Task> executeFunction,
    Predicate<object?> canExecutePredicate
) : ICommand
{
    public event EventHandler? CanExecuteChanged;
    private void UpdateCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    
    public bool IsWorking { get; private set; }

    public SimpleAsyncRelayCommand(Func<object?, Task> executeFunction) 
        : this(executeFunction, _ => true) { }

    public bool CanExecute(object? parameter) => 
        !IsWorking && (canExecutePredicate?.Invoke(parameter) ?? true);
    
    public void Execute(object? parameter = null) => _ = ExecuteAsync(parameter);

    public async Task ExecuteAsync(object? parameter = null)
    {
        IsWorking = true;
        UpdateCanExecute();

        await executeFunction(parameter);

        IsWorking = false;
        UpdateCanExecute();
    }
}