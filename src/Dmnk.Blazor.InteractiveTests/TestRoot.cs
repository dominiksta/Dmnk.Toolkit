using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Dmnk.Blazor.InteractiveTests;

/// <summary>
/// The root component that is rendered in the test window. Allows wrapping the actual SUT
/// in a given TestBed component, which may do things like inject tags into the header using
/// <see cref="Microsoft.AspNetCore.Components.Web.HeadContent"/> or add things like dialog
/// providers.
///
/// <p>
/// Additionally, it also by default includes the scoped styles of the test project.
/// </p>
/// </summary>
internal sealed class TestRoot : ComponentBase
{
    // this component is implemented like this and without razor syntax only to avoid using
    // the razor sdk in this package. using the razor sdk would cause this package to also 
    // expose the _framework assets, which then causes an asset conflict and therefore an
    // exception to be thrown in the consuming interactive test package. we could implement 
    // this in a new package and have this one reference it instead, but that seems a little
    // awkward for a single file as well.
    
    [Parameter] public required string ConsumingProjectName { get; set; }
    
    [Parameter] public Type? TestBedType { get; set; }
    [Parameter] public Dictionary<string, object?>? TestBedParameters { get; set; }
    
    [Parameter] public required Type SutType { get; set; }
    [Parameter] public Dictionary<string, object?>? SutParameters { get; set; }

    private readonly Lazy<bool> _testBedHasChildContent;

    public TestRoot()
    {
        _testBedHasChildContent =
            new Lazy<bool>(() => TestBedType?.GetProperty("ChildContent") is not null);
    }
    
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);
        
        // note that according to the docs
        // https://learn.microsoft.com/en-us/aspnet/core/blazor/advanced-scenarios?view=aspnetcore-10.0#sequence-numbers-relate-to-code-line-numbers-and-not-execution-order
        // the sequence numbers should refer to the line of code and therefore be hardcoded
        // and not incremented automatically

        if (TestBedType is null) builder.AddContent(0, RenderHeadContent());
        
        if (TestBedType is not null)
        {
            builder.OpenComponent<DynamicComponent>(10);
            builder.AddAttribute(11, "Type", TestBedType);
            if (_testBedHasChildContent.Value)
            {
                TestBedParameters ??= new Dictionary<string, object?>();
                TestBedParameters["ChildContent"] = RenderSut();
            }
            builder.AddAttribute(12, "Parameters", TestBedParameters);
            builder.CloseComponent();
            
            if (!_testBedHasChildContent.Value) builder.AddContent(20, RenderSut());
        }
        else
        {
            builder.AddContent(30, RenderSut());
        }
    }

    private RenderFragment RenderSut() => builder =>
    {
        builder.OpenComponent<DynamicComponent>(0);
        builder.AddAttribute(1, "Type", SutType);
        builder.AddAttribute(2, "Parameters", SutParameters);
        builder.CloseComponent();
    };

    private RenderFragment RenderHeadContent() => builder =>
    {
        builder.OpenComponent<HeadContent>(0);
        builder.AddAttribute(1, "ChildContent", (RenderFragment)(head =>
        {
            head.OpenElement(0, "link");
            head.AddAttribute(1, "href", $"{ConsumingProjectName}.styles.css");
            head.AddAttribute(2, "rel", "stylesheet");
            head.CloseElement();
        }));
        builder.CloseComponent();
    };
}