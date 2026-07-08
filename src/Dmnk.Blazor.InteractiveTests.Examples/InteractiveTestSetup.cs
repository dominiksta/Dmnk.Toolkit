namespace Dmnk.Blazor.InteractiveTests.Examples;

[SetUpFixture]
public class InteractiveTestSetup
{
    [OneTimeSetUp]
    public void Setup() => BlazorInteractiveTests.PathInfo =
        InteractiveTestsProjectPathInfo.FromAssemblyInSolutionDir(GetType().Assembly);
}