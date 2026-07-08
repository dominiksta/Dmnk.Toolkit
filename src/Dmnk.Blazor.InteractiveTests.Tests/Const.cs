namespace Dmnk.Blazor.InteractiveTests.Tests;

public static class Const
{
    public const string FakeSlnDir = "FakeSolution";
    
#if DEBUG
    public const bool Debug = true;
#else
    public const bool Debug = false;
#endif
}