using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Dmnk.Blazor.Mvvm.CommunityToolkit;

/// <summary>
/// See <see cref="Bind"/>
/// </summary>
public static class CommandExtensions
{
    /// <summary>
    /// Exactly like <see cref="Dmnk.Blazor.Mvvm.CommandExtensions.Bind"/>, but for <see
    /// cref="IAsyncRelayCommand"/> instead of <see cref="ICommand"/>.
    ///
    /// <p>
    /// Convert a parameterless <see cref="IAsyncRelayCommand"/> to a parameterless <see
    /// cref="Func{Task}"/>. The returned <see cref="Func{Task}"/> will check if the command can be
    /// executed before executing it.
    /// </p>
    /// 
    /// <p>
    /// Blazor components will typically not accept commands but lambdas. This method allows for a
    /// slightly nicer syntax, but more importantly, it avoids allocating new
    /// <see cref="Func{Task}"/> instances on every render.
    /// </p>
    ///
    /// <p>
    /// You probably want to put <c>@using Dmnk.Blazor.Mvvm.CommunityToolkit</c> in your
    /// _Imports.razor so that this method takes precedence over <see
    /// cref="Dmnk.Blazor.Mvvm.CommandExtensions.Bind"/>.
    /// </p>
    /// </summary>
    public static Func<Task> Bind<T>(
       this IAsyncRelayCommand cmd, AbstractMvvmComponentBase<T> component
    ) where T : INotifyPropertyChanged
    {
#pragma warning disable CS0618 // Type or member is obsolete - intended use-case
        if (component.__CommandBindingsAsync.TryGetValue(cmd, out var bind)) return bind;
#pragma warning restore CS0618 // Type or member is obsolete
        
        Func<Task> action = async () =>
        {
            if (!cmd.CanExecute(null)) return;
            await cmd.ExecuteAsync(null);
        };
        
#pragma warning disable CS0618 // Type or member is obsolete - intended use-case
        component.__CommandBindingsAsync[cmd] = action;
#pragma warning restore CS0618 // Type or member is obsolete
        
        return action;
    }
}