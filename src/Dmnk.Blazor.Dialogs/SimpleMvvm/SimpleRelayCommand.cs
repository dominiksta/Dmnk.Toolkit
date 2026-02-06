using System.Windows.Input;

namespace Dmnk.Blazor.Dialogs.SimpleMvvm;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Internal basic MVVM implementation, just to not depend on e.g. the CommunityToolkit in the core
/// library. Not intended for public use, use a proper MVVM library.
/// </summary>
public class SimpleRelayCommand(
    Action<object?> executeFunction,
    Predicate<object?> canExecutePredicate
) : ICommand
{
    public SimpleRelayCommand(Action<object?> executeFunction) 
        : this(executeFunction, _ => true) { }

    public event EventHandler? CanExecuteChanged;
    private void UpdateCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    public virtual bool CanExecute(object? parameter) => 
        canExecutePredicate?.Invoke(parameter) ?? true;
    
    public virtual void Execute(object? parameter)
    {
        UpdateCanExecute();
        executeFunction(parameter);
        UpdateCanExecute();
    }
}