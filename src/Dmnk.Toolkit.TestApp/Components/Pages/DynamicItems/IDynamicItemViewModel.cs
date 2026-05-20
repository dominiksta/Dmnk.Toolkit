using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Dmnk.Toolkit.TestApp.Components.Pages.DynamicItems;

public interface IDynamicItemViewModel : INotifyPropertyChanged
{
    public string Name { get; }
}

public abstract class DynamicItemViewModelBase : ObservableObject, IDynamicItemViewModel
{
    public abstract string Name { get; }
}
