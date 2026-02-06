using System.Windows.Input;

namespace Dmnk.Blazor.Dialogs.SimpleMvvm;

public class SimpleRelayCommand : ICommand
{
    public Action<object?> ExecuteFunction { get; }
    public Predicate<object?> CanExecutePredicate { get; }
    public event EventHandler CanExecuteChanged;
    private void UpdateCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    
    public SimpleRelayCommand(Action<object?> executeFunction) : this(executeFunction, _ => true) { }  
    public SimpleRelayCommand(Action<object?> executeFunction, Predicate<object?> canExecutePredicate)
    {
        ExecuteFunction = executeFunction;
        CanExecutePredicate = canExecutePredicate;
    }
    
    public virtual bool CanExecute(object? parameter) => (CanExecutePredicate?.Invoke(parameter) ?? true);
    
    public virtual async void Execute(object? parameter)
    {
        UpdateCanExecute();
        ExecuteFunction(parameter);
        UpdateCanExecute();
    }
}