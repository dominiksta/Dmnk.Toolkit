using System.Windows.Input;

namespace Dmnk.Blazor.Dialogs.SimpleMvvm;

public class SimpleAsyncRelayCommand : ICommand
{
    public Func<object?, Task> ExecuteFunction { get; }
    public Predicate<object?> CanExecutePredicate { get; }
    public event EventHandler? CanExecuteChanged;
    private void UpdateCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    public bool IsWorking { get; private set; }

    public SimpleAsyncRelayCommand(Func<object?, Task> executeFunction) : this(executeFunction, _ => true) { }  
    public SimpleAsyncRelayCommand(Func<object?, Task> executeFunction, Predicate<object?> canExecutePredicate)
    {
        ExecuteFunction = executeFunction;
        CanExecutePredicate = canExecutePredicate;
    }

    public bool CanExecute(object? parameter) => !IsWorking && (CanExecutePredicate?.Invoke(parameter) ?? true);
    
    public void Execute(object? parameter = null)
    {
        _ = ExecuteAsync(parameter);
    }
    
    public async Task ExecuteAsync(object? parameter = null)
    {
        IsWorking = true;
        UpdateCanExecute();

        await ExecuteFunction(parameter);

        IsWorking = false;
        UpdateCanExecute();
    }
}