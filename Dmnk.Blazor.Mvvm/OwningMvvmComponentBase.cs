using System.ComponentModel;

namespace Dmnk.Blazor.Mvvm;

/// <summary>
/// <inheritdoc cref="AbstractMvvmComponentBase{T}"/>
/// <p>The ViewModel is set as a property that isn't a parameter(<see cref="Vm"/>).</p>
/// </summary>
/// <typeparam name="T">The ViewModel</typeparam>
public abstract class OwningMvvmComponentBase<T> : AbstractMvvmComponentBase<T>
    where T : INotifyPropertyChanged
{
    [Obsolete("Internal - do NOT modify.")] 
    protected override T? __ViewModel { get => _vm; set => _vm = value!; }
    
    private T _vm = default!;

    public T Vm
    {
        get => _vm;
        set
        {
            if (EqualityComparer<T>.Default.Equals(_vm, value)) return;
            SetViewModel(value);
            _vm = value;
        }
    }

    protected override void OnInitialized()
    {
        if (Vm == null) throw new ArgumentNullException(nameof(Vm));
        base.OnInitialized();
    }
}