using Dmnk.Blazor.Dialogs.Properties;
using Dmnk.Blazor.Dialogs.SimpleMvvm;

namespace Dmnk.Blazor.Dialogs.DefaultDialogs;

/// <summary>
/// This dialog will ask the user for a single input value of type T, and optionally
/// <see cref="Validate"/> it.
/// </summary>
/// <typeparam name="T">
/// Must be one of int, string, double, DateTime.
/// </typeparam>
public class InputDialogViewModel<T> : SimpleDialogViewModelBase
{
    private readonly HashSet<Type> _validTypes =
        [typeof(int), typeof(string), typeof(double), typeof(DateTime)];
    
    /// <summary>
    /// See <see cref="InputDialogViewModel{T}"/>. Checks that the type parameter is valid.
    /// </summary>
    public InputDialogViewModel()
    {
        var t = typeof(T);
        if (!_validTypes.Contains(t)) throw new ArgumentException(
            $"Type parameter {t} is not supported. Must be one of int, string, double, DateTime.");
    }
    
    /// <summary>
    /// The text shown on the confirmation button. Defaults to "OK".
    /// </summary>
    public string ActionConfirmText { get; init; } = Resources.Actions_OK;
    /// <summary>
    /// The text shown on the cancel button. Defaults to "Cancel".
    /// </summary>
    public string ActionCancelText { get; init; } = Resources.Actions_Cancel;
    
    /// <summary>
    /// An optional validation function that takes the current input value and returns an error
    /// message if the value is invalid, or null if the value is valid. If this function is set, the
    /// dialog will only allow closing with the confirmation button if the input value is valid. The
    /// error message can be displayed in the view by binding to the <see cref="ValidationError"/>
    /// property.
    /// </summary>
    public Func<T?, string?>? Validate { get; init; }

    /// <summary>
    /// The actual data input by the user. See type parameter T.
    /// </summary>
    public T? Value
    {
        get;
        set
        {
            SetProperty(ref field, value);
            OnPropertyChanged(nameof(ValidationError));
        }
    }

    /// <summary>
    /// Returns the current validation error, if any. See <see cref="Validate"/>.
    /// </summary>
    public string? ValidationError => Validate?.Invoke(Value);

    /// <summary>
    /// Close the dialog, but only if <see cref="ValidationError"/> is null.
    /// </summary>
    public SimpleAsyncRelayCommand CloseCommand => new SimpleAsyncRelayCommand(
        async _ =>
        {
            if (ValidationError != null) return;
            await Dialog.Close();
        }, 
        _ => ValidationError == null
    );
}