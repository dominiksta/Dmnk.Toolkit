using Microsoft.AspNetCore.Components;

namespace Dmnk.Blazor.InteractiveTests.Tests;

[TestFixture]
public class BlazorInteractiveTestRunnerTests
{
    [Test]
    public void ShowComponent_Throws_When_PathInfo_Is_Not_Set()
    {
        var previous = BlazorInteractiveTestRunner.PathInfo;
        BlazorInteractiveTestRunner.PathInfo = null;

        try
        {
            Assert.That(
                async () => await BlazorInteractiveTestRunner.ShowComponent<TestComponent>(),
                Throws.InstanceOf<InvalidOperationException>()
                    .With.Message.Contains(nameof(BlazorInteractiveTestRunner.PathInfo)));
        }
        finally
        {
            BlazorInteractiveTestRunner.PathInfo = previous;
        }
    }

    [Test]
    public void ShowComponent_Throws_PlatformNotSupported_On_NonWindows_Target()
    {
        var previous = BlazorInteractiveTestRunner.PathInfo;
        BlazorInteractiveTestRunner.PathInfo =
            InteractiveTestsProjectPathInfo.FromAssemblyInSolutionDir(GetType().Assembly);

        try
        {
            Assert.That(
                async () => await BlazorInteractiveTestRunner.ShowComponent<TestComponent>(),
                Throws.InstanceOf<PlatformNotSupportedException>()
                    .With.Message.Contains("Interactive tests require windows"));
        }
        finally
        {
            BlazorInteractiveTestRunner.PathInfo = previous;
        }
    }

    private sealed class TestComponent : ComponentBase;
}
