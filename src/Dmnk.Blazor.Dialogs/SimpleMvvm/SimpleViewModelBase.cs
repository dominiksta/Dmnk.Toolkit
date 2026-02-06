using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Dmnk.Blazor.Dialogs.SimpleMvvm;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Internal basic MVVM implementation, just to not depend on e.g. the CommunityToolkit in the core
/// library. Not intended for public use, use a proper MVVM library.
/// </summary>
public class SimpleViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null!) => 
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    
    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(storage, value))
            return false;
        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}