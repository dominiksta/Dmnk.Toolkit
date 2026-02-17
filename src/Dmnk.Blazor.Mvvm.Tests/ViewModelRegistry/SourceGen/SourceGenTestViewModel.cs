using CommunityToolkit.Mvvm.ComponentModel;

namespace Dmnk.Blazor.Mvvm.Tests.ViewModelRegistry.SourceGen;

[ViewModelFor(typeof(SourceGenTestView))]
public partial class SourceGenTestViewModel : ObservableObject
{
    [ObservableProperty]
    private string _message = "Hello from Source Generator!";
}
