using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Dmnk.Blazor.Mvvm;

/// <summary>
/// <inheritdoc cref="AbstractMvvmComponentBase{T}"/>
/// <p>The ViewModel is set as a required parameter(<see cref="Vm"/>).</p>
/// </summary>
/// <typeparam name="T">The ViewModel</typeparam>
public abstract class MvvmComponentBase<T> : AbstractMvvmComponentBase<T> 
    where T : INotifyPropertyChanged
{
    [Obsolete("Internal - do NOT modify.")] 
    protected override T? __ViewModel { get => _vm; set => _vm = value!; }
    
    private T _vm = default!;

    [Parameter, EditorRequired]
    [SuppressMessage(
        "Usage", "BL0007:Component parameters should be auto properties",
        Justification = """
                        We kind of need to unregister the event handler here. Also,
                        even if it would work, moving this code to OnParameterSet
                        could still cause the same accidental re-renders.
                        """
    )]
    public T Vm
    {
        get => _vm;
        set
        {
            if (EqualityComparer<T>.Default.Equals(_vm, value)) return;
            _vm = value;
            SetViewModel(_vm);
        }
    }
}
