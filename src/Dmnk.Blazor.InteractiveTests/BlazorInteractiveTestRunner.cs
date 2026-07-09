using Dmnk.Blazor.InteractiveTests.Api;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Dmnk.Blazor.InteractiveTests;

/// <summary>
/// This class provides a way for you to write nunit/xunit/whatever "tests" that show a particular
/// blazor component in a window. We call these "tests" interactive tests (not sure if there is
/// a commonly accepted term for this).
///
/// <p>
/// This is <em>not intended for actual automated testing</em>. Rather, it is a way for you to work
/// on and manually test a component without starting your whole application - which becomes
/// especially useful in a large application.
/// </p>
///
/// <p>
/// Note that your components will be run in a <b>Blazor Hybrid</b> style webview. Running a blazor
/// component in webassembly-mode is not supported since that would require an actual project
/// instead of just a single loose component. And we would have to mock http calls, and debugging
/// would be more difficult, and... it's just not worth it. But do keep that in mind as there may
/// be things that will work in you interactive test but not in the final app if it's deployed
/// as WASM.
/// </p>
///
/// <p>
/// Regrettably, this is <b>windows-only</b>, because it relies on the WinForms implementation of
/// BlazorWebView. Implementations of a Blazor Hybrid style webview for other platforms seem
/// to *kind of sort of* exist as community packages. If demand arises, that may be worth looking
/// into. The API shape <em>should</em> allow this to become cross-platform without breaking
/// changes.
/// </p>
/// </summary>
/// 
/// <example>
/// <p>
/// (Assuming NUnit, details are probably slightly different for other testing frameworks.)
/// </p>
/// 
/// <p>
/// In <c>My.BlazorLibrary\Counter.razor</c>: <em>Well, a counter component</em>
/// </p>
///
/// <p>
/// In <c>My.BlazorLibrary.Tests\Setup.cs</c>:
/// <code>
/// [SetUpFixture]
/// public class TestSetUp
/// {
///     [OneTimeSetUp]
///     public void Setup() => BlazorInteractiveTestRunner.PathInfo =
///         InteractiveTestsProjectPathInfo.FromAssemblyInSolutionDir(GetType().Assembly);
/// }
/// </code>
/// </p>
/// 
/// <p>
/// In <c>My.BlazorLibrary.Tests\MyInteractiveTests.cs</c>:
/// <code>
/// // MUST be explicit - it will not terminate on its own.
/// // MUST have this ApartmentState.
/// [Test, Explicit, Apartment(ApartmentState.STA)]
/// public async Task Show_Counter_Component()
/// {
///     // This will open up a window with just your `Counter` component so you can play
///     // with it without starting the whole app.
///     await BlazorInteractiveTestRunner.ShowComponent&lt;Counter&gt;();
/// }
/// </code>
/// </p>
/// </example>
public static class BlazorInteractiveTestRunner
{
    /// <summary>
    /// Show the blazor component given by <typeparamref name="TComponent"/> in a (WinForms) Dialog.
    /// 
    /// <p>
    /// You MUST set <see cref="PathInfo"/> using one of the SetTestProjectDir methods
    /// before calling this.
    /// </p>
    /// </summary>
    ///
    /// <example>
    /// <code>
    /// BlazorInteractiveTestRunner.ShowComponent&lt;MyComponent&gt;(
    ///     parameters => parameters
    ///         .Add(component => component.MyParameter, 4)
    ///         .Add(component => component.MyOtherParameter, "hi"));
    /// </code>
    /// </example>
    /// 
    /// <param name="configureComponent">
    /// Build up a list of parameters for the component.
    /// </param>
    /// <param name="services">
    /// When a blazor component uses <c>@inject</c> or <c>[Inject]</c>, the service will be
    /// resolved using this collection.
    /// </param>
    /// <typeparam name="TComponent">The actual component type</typeparam>
    public static async Task ShowComponent<TComponent>(
        Action<BlazorComponentParametersBuilder<TComponent>> configureComponent,
        IServiceCollection? services = null)
        where TComponent : ComponentBase
    {
        var builder = new BlazorComponentParametersBuilder<TComponent>();
        configureComponent(builder);
        await ShowComponent<TComponent>(builder.Build(), services);
    }
    
    /// <summary>
    /// A more advanced version of the more type-safe expression based overload
    /// <see cref="ShowComponent{T}(Action{BlazorComponentParametersBuilder{T}},IServiceCollection?)"/>,
    /// which allows you to define the parameters as a raw dictionary.
    ///
    /// <p>
    /// You typically shouldn't need to use this.
    /// </p>
    /// </summary>
    /// 
    /// <example>
    /// <code>
    /// BlazorInteractiveTestRunner.ShowComponent&lt;MyComponent&gt;(
    ///     new Dictionary&lt;string, object?&gt;()
    ///     {
    ///         { "MyParameter", 4 },
    ///         { "MyOtherParameter", "hi" },
    ///     });
    /// </code>
    /// </example>
    public static async Task ShowComponent<T>(
        Dictionary<string, object?>? parameters = null,
        IServiceCollection? services = null)
        where T : ComponentBase =>
        await ShowComponent(typeof(T), parameters, null, null, services);

    /// <summary>
    /// Allows you to additionally provide a testbed component, which is excepted to have a
    /// <c>ChildContent</c> parameter that the SUT component will then be wrapped in.
    /// </summary>
    public static async Task ShowComponent<TComponent, TTestBed>(
        Dictionary<string, object?>? componentParameters = null,
        Dictionary<string, object?>? testBedParameters = null,
        IServiceCollection? services = null)
        where TComponent : ComponentBase => 
        await ShowComponent(
            typeof(TComponent), componentParameters, 
            typeof(TTestBed), testBedParameters, services);
    
    /// <summary>
    /// Allows you to additionally provide a testbed component, which is excepted to have a
    /// <c>ChildContent</c> parameter that the SUT component will then be wrapped in.
    /// </summary>
    public static async Task ShowComponent<TComponent, TTestBed>(
        Action<BlazorComponentParametersBuilder<TComponent>> configureComponent,
        Action<BlazorComponentParametersBuilder<TTestBed>>? configureTestBed = null,
        IServiceCollection? services = null)
        where TComponent : ComponentBase
        where TTestBed : ComponentBase
    {
        var sutParamsBuilder = new BlazorComponentParametersBuilder<TComponent>();
        configureComponent(sutParamsBuilder);
        var testBedParamsBuilder = new BlazorComponentParametersBuilder<TTestBed>();
        configureTestBed?.Invoke(testBedParamsBuilder);
        await ShowComponent<TComponent, TTestBed>(
            sutParamsBuilder.Build(), testBedParamsBuilder.Build(), services);
    }

    /// <summary>
    /// Set this once globally (using one of the setter methods)
    /// before calling any of the ShowComponent methods.
    /// <p>
    /// Used to determine the location of the <c>staticwebassets.*.json</c> files, which are
    /// required for any BlazorWebView to function (and produced automatically on build by default,
    /// you shouldn't have to worry about that).
    /// </p>
    /// </summary>
    /// <example>
    /// BlazorInteractiveTestRunner.PathInfo =
    ///   InteractiveTestsProjectPathInfo.FromAssemblyInSolutionDir(
    ///     typeof(MyTypeInTestProject).Assembly);
    /// </example>
    public static InteractiveTestsProjectPathInfo? PathInfo { get; set; }
    
    private static async Task ShowComponent(
        Type componentType,
        Dictionary<string, object?>? componentParameters = null,
        Type? testBedType = null,
        Dictionary<string, object?>? testBedParameters = null,
        IServiceCollection? services = null)
    {
        
        if (PathInfo is null) throw new InvalidOperationException(
            $"You must set {nameof(PathInfo)} to the assembly of your test project." +
            "(This is used to determine the location of the `staticwebassets.*.json` files)");
        
#if _WINDOWS
        using var form = new BlazorInteractiveTestForm(
            PathInfo, 
            componentType, componentParameters, 
            testBedType, testBedParameters,
            services);
        await form.ShowDialogAsync();
#else
        throw new PlatformNotSupportedException("""
            Interactive tests require windows. Please make sure your project is either targeted
            for netX.X-windows or multi-targeted for netX.X-windows;netX.X and netX.X-windows
            is selected as the current target in your IDE. If you see this error in CI, a test
            of yours is probably missing an [Explicit] attribute (or similar).
            """);
#endif
    }
}