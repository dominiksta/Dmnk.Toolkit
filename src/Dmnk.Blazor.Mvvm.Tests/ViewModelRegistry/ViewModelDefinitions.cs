using CommunityToolkit.Mvvm.ComponentModel;

namespace Dmnk.Blazor.Mvvm.Tests.ViewModelRegistry;

public abstract partial class AbstractViewModel : ObservableObject
{
    public abstract string Name { get; }
}

public class SpecificViewModel1 : AbstractViewModel
{
    public override string Name => "SpecificViewModel1";
}

public class SpecificViewModel2 : AbstractViewModel
{
    public override string Name => "SpecificViewModel2";
}

public class UnregisteredViewModel : AbstractViewModel
{
    public override string Name => "UnregisteredViewModel";
    public override string ToString() => "Custom ToString for UnregisteredViewModel";
}