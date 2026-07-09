namespace Dmnk.Blazor.InteractiveTests.Examples;

[SetUpFixture]
public class InteractiveTestSetup
{
    [OneTimeSetUp]
    public void Setup() => BlazorInteractiveTestRunner.PathInfo =
        InteractiveTestsProjectPathInfo.FromAssemblyInSolutionDir(GetType().Assembly);
}