using System.ComponentModel;
using System.Windows.Input;

namespace Dmnk.Blazor.Mvvm;

/// <summary>
/// See <see cref="Bind"/>
/// </summary>
public static class CommandExtensions
{
    /// <summary>
    /// Convert a parameterless <see cref="ICommand"/> to a parameterless <see
    /// cref="Action"/>. The returned <see cref="Action"/> will check if the command can be
    /// executed before executing it.
    /// <p>
    /// Blazor components will typically not accept commands but lambdas. This method allows for a
    /// slightly nicer syntax, but more importantly, it avoids allocating new
    /// <see cref="Action"/> instances on every render.
    /// </p>
    /// </summary>
    public static Action Bind<T>(
        this ICommand cmd, AbstractMvvmComponentBase<T> component
    ) where T : INotifyPropertyChanged
    {
        if (component.__CommandBindings.TryGetValue(cmd, out var action)) return action;
        
        action = () =>
        {
            if (!cmd.CanExecute(null)) return;
            cmd.Execute(null);
        };
        
        component.__CommandBindings[cmd] = action;
        return action;
    }
}