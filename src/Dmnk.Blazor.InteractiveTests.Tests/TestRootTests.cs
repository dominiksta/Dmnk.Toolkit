using Bunit;
using Microsoft.AspNetCore.Components.Web;

namespace Dmnk.Blazor.InteractiveTests.Tests;

[TestFixture]
public class TestRootTests
{
    [Test]
    public void Renders_Sut_And_Default_Project_Stylesheet_When_No_TestBed_Is_Provided()
    {
        using var context = new BunitContext();
        SetupHeadOutletInterop(context);
        var headOutlet = context.Render<HeadOutlet>();

        var sut = context.Render<TestRoot>(parameters => parameters
            .Add(component => component.ConsumingProjectName, "My.TestProject")
            .Add(component => component.SutType, typeof(TestSutComponent))
            .Add(component => component.SutParameters, new Dictionary<string, object?>
            {
                [nameof(TestSutComponent.Title)] = "Hello from sut"
            }));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(sut.Markup, Does.Contain("Hello from sut"));
            Assert.That(headOutlet.Markup, Does.Contain("My.TestProject.styles.css"));
        }
    }

    [Test]
    public void Renders_TestBed_ChildContent_And_TestBed_HeadContent()
    {
        using var context = new BunitContext();
        SetupHeadOutletInterop(context);
        var headOutlet = context.Render<HeadOutlet>();

        var sut = context.Render<TestRoot>(parameters => parameters
            .Add(component => component.ConsumingProjectName, "My.TestProject")
            .Add(component => component.SutType, typeof(TestSutComponent))
            .Add(component => component.SutParameters, new Dictionary<string, object?>
            {
                [nameof(TestSutComponent.Title)] = "Wrapped sut"
            })
            .Add(component => component.TestBedType, typeof(TestChildContentTestBed))
            .Add(component => component.TestBedParameters, new Dictionary<string, object?>
            {
                [nameof(TestChildContentTestBed.Name)] = "Fluent bed"
            }));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(sut.Markup, Does.Contain("Fluent bed"));
            Assert.That(sut.Markup, Does.Contain("Wrapped sut"));
            Assert.That(headOutlet.Markup, Does.Contain("testbed.css"));
            Assert.That(headOutlet.Markup, Does.Not.Contain("My.TestProject.styles.css"));
        }
    }

    [Test]
    public void Renders_Sut_Beside_TestBed_When_TestBed_Has_No_ChildContent_Parameter()
    {
        using var context = new BunitContext();

        var sut = context.Render<TestRoot>(parameters => parameters
            .Add(component => component.ConsumingProjectName, "My.TestProject")
            .Add(component => component.SutType, typeof(TestSutComponent))
            .Add(component => component.SutParameters, new Dictionary<string, object?>
            {
                [nameof(TestSutComponent.Title)] = "Sibling sut"
            })
            .Add(component => component.TestBedType, typeof(TestNoChildContentTestBed))
            .Add(component => component.TestBedParameters, new Dictionary<string, object?>
            {
                [nameof(TestNoChildContentTestBed.Name)] = "Independent bed"
            }));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(sut.Markup, Does.Contain("Independent bed"));
            Assert.That(sut.Markup, Does.Contain("Sibling sut"));
        }
    }

    private static void SetupHeadOutletInterop(BunitContext context)
    {
        context.JSInterop.Setup<string>("Blazor._internal.PageTitle.getAndRemoveExistingTitle")
            .SetResult(string.Empty);
    }
}
