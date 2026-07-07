using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Dmnk.Blazor.InteractiveTests.TestComponents;

public partial class FluentMvvmTestViewModel : ObservableObject
{
    [ObservableProperty] private string _name;
    
    [ObservableProperty] private int _count;

    [RelayCommand]
    private void IncrementCount() => _count++;
}