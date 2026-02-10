using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Dmnk.Blazor.Mvvm.Tests;

public partial class TestViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ChangeTextOnlyAfterFirstAsyncChangeCommand))]
    private string _text = "Hello World!";
    
    public string NonObservableText = "Hello World";
    
    [RelayCommand]
    private void ChangeText()
    {
        _firstChangeDone = true;
        Text = "Text changed!";
    }
    
    [RelayCommand]
    private async Task ChangeTextWait()
    {
        await Task.Delay(100);
        Text = "Text changed async!";
        ChangeTextOnlyAfterFirstAsyncChangeCommand.NotifyCanExecuteChanged();
    }
    
    private bool _firstChangeDone = false;
    public bool ChangeTextOnlyAfterFirstAsyncChangeCommandCanExecute() => _firstChangeDone;
    
    [RelayCommand(CanExecute = nameof(ChangeTextOnlyAfterFirstAsyncChangeCommandCanExecute))]
    private void ChangeTextOnlyAfterFirstAsyncChange()
    {
        Text= "Text changed only after first change!";
    }
    
    [RelayCommand]
    private void ChangeNonObservableText()
    {
        NonObservableText = "Non observable text changed!";
    }
}