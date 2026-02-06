using Dmnk.Blazor.Dialogs.Api;
using Dmnk.Icons.Core;

namespace Dmnk.Blazor.Dialogs.DefaultDialogs;

/// <summary>
/// Helper methods to make showing input dialogs (see <see cref="InputDialogViewModel{T}"/>) easier.
/// </summary>
public static class InputDialogControllerExtensions
{
    /// <summary>
    /// Show an input dialog with the given title, default value and validation function. The dialog
    /// will return the value entered by the user.
    /// </summary>
    /// <param name="dialog">The dialog controller</param>
    /// <param name="title"> The title in the dialog header. </param>
    /// <param name="defaultValue"></param>
    /// <param name="validate"></param>
    /// <param name="icon"></param>
    /// <typeparam name="T">
    /// Must be one of int, string, double, DateTime.
    /// </typeparam>
    public static async Task<(bool Cancelled, T Result)> ShowInput<T>(
        this IVmDialogController dialog,
        string title, 
        T? defaultValue = default,
        Func<T?, string?>? validate = null,
        Icon? icon = null
    )
    {
        var vm = new InputDialogViewModel<T>()
        {
            Value = defaultValue, Validate = validate
        };
        var dlg = await dialog.Show(
            new VmDialogParameters()
            {
                Title = title, 
                Icon = icon
            },
            vm
        );
        await dlg.WaitClosed;
        return (dlg.Cancelled, vm.Value!);
    }

}