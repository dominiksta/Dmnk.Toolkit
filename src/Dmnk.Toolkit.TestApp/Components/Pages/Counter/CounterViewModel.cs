using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Dmnk.Toolkit.TestApp.Components.Pages.Counter;

public partial class CounterViewModel : ObservableObject
{
    private bool IncrementEnabled() => Count < 10;

    [ObservableProperty]
    private int _count;

    [RelayCommand(CanExecute = nameof(IncrementEnabled))]
    private void IncrementCount() => Count++;

    [RelayCommand(CanExecute = nameof(IncrementEnabled))]
    private async Task AsyncIncrementCount()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        Count++;
    }
}
