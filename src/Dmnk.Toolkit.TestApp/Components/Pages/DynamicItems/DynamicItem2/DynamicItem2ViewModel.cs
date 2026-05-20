using CommunityToolkit.Mvvm.ComponentModel;
using Dmnk.Blazor.Mvvm;
using Dmnk.Toolkit.TestApp.Components.Pages.DynamicItems;

namespace Dmnk.Toolkit.TestApp.Components.Pages.DynamicItems.DynamicItem2;

[ViewModelFor(typeof(DynamicItem2View))]
public partial class DynamicItem2ViewModel : DynamicItemViewModelBase
{
    public override string Name => "Dynamic Item 2";

    [ObservableProperty]
    private int _counter;
}
