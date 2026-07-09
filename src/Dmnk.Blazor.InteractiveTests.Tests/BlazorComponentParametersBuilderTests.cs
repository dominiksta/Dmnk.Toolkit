using Dmnk.Blazor.InteractiveTests.Api;
using Microsoft.AspNetCore.Components;

namespace Dmnk.Blazor.InteractiveTests.Tests;

[TestFixture]
public class BlazorComponentParametersBuilderTests
{
    [Test]
    public void Add_Builds_Dictionary_For_Parameter_Property()
    {
        var builder = new BlazorComponentParametersBuilder<TestComponent>();

        var built = builder
            .Add(component => component.Title, "Hello")
            .Build();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(built, Has.Count.EqualTo(1));
            Assert.That(built, Contains.Key(nameof(TestComponent.Title)));
            Assert.That(built[nameof(TestComponent.Title)], Is.EqualTo("Hello"));
        }
    }

    [Test]
    public void Add_Allows_Chaining_And_Last_Value_Wins()
    {
        var builder = new BlazorComponentParametersBuilder<TestComponent>();

        var built = builder
            .Add(component => component.Title, "First")
            .Add(component => component.Count, 3)
            .Add(component => component.Title, "Second")
            .Build();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(built, Has.Count.EqualTo(2));
            Assert.That(built[nameof(TestComponent.Title)], Is.EqualTo("Second"));
            Assert.That(built[nameof(TestComponent.Count)], Is.EqualTo(3));
        }
    }

    /// <summary>
    /// This behavior is not necessarily set in stone.
    /// Maybe we should filter out cascading parameters?
    /// </summary>
    [Test]
    public void Add_Allows_Cascading_Parameter_Property()
    {
        var builder = new BlazorComponentParametersBuilder<TestComponent>();

        var built = builder
            .Add(component => component.Theme, "Dark")
            .Build();

        Assert.That(built[nameof(TestComponent.Theme)], Is.EqualTo("Dark"));
    }

    [Test]
    public void Add_Throws_For_Non_Parameter_Property()
    {
        var builder = new BlazorComponentParametersBuilder<TestComponent>();

        var act = () => builder.Add(component => component.NonParameter, "nope");

        Assert.That(act, Throws.ArgumentException.With.Message.Contains(
            $"Property '{nameof(TestComponent.NonParameter)}' is not a Blazor parameter."));
    }

    [Test]
    public void Add_Throws_For_Non_Property_Expression()
    {
        var builder = new BlazorComponentParametersBuilder<TestComponent>();

        var act = () => builder.Add(component => component.ToString(), "nope");

        Assert.That(act, Throws.ArgumentException.With.Message.Contains(
            "Selector must be a property access."));
    }

    private sealed class TestComponent : ComponentBase
    {
        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public int Count { get; set; }

        [CascadingParameter]
        public string? Theme { get; set; }

        public string? NonParameter { get; set; }
    }
}
