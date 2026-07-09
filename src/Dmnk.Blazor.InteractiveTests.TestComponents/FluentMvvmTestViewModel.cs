using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Dmnk.Blazor.InteractiveTests.TestComponents;

public partial class FluentMvvmTestViewModel : ObservableObject
{
    public required string Title { get; init; }
    
    [ObservableProperty] private string _name = string.Empty;
    
    [ObservableProperty] private int _count;

    [RelayCommand]
    private void IncrementCount() => Count++;
}