namespace Dmnk.Blazor.InteractiveTests.Examples;

[SetUpFixture]
public class InteractiveTestSetup
{
    [OneTimeSetUp]
    public void Setup() => BlazorInteractiveTestsRunner.PathInfo =
        InteractiveTestsProjectPathInfo.FromAssemblyInSolutionDir(GetType().Assembly);
}