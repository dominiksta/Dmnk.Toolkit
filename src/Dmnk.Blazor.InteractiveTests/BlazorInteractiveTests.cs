using System.Reflection;
using System.Runtime.Versioning;
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
///     public void Setup() => BlazorInteractiveTests.SetTestProjectDir(GetType().Assembly);
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
///     await BlazorInteractiveTests.ShowComponent&lt;Counter&gt;();
/// }
/// </code>
/// </p>
/// </example>
[SupportedOSPlatform("windows")]
public static class BlazorInteractiveTests
{
    /// <summary>
    /// Show the blazor component given by <typeparamref name="T"/> in a (WinForms) Dialog.
    /// 
    /// <p>
    /// You MUST set <see cref="TestProjectDir"/> using one of the SetTestProjectDir methods
    /// before calling this.
    /// </p>
    /// </summary>
    /// <param name="parameters">
    /// The set of parameters of the blazor component (properties annotated with [Parameter]).
    /// </param>
    /// <param name="services"></param>
    /// <typeparam name="T"></typeparam>
    public static async Task ShowComponent<T>(
        IReadOnlyDictionary<string, object>? parameters = null,
        IServiceCollection? services = null)
        where T : ComponentBase
    {
        if (TestProjectDir is null) throw new InvalidOperationException(
            $"You must set {nameof(TestProjectDir)} to the assembly of your test project." +
            "(This is used to determine the location of the `staticwebassets.*.json` files)");
        
        using var form = new BlazorInteractiveTestForm<T>(TestProjectDir, parameters, services);
        await form.ShowDialogAsync();
    }
    
    /// <summary>
    /// Set this once globally (using one of the setter methods)
    /// before calling <see cref="ShowComponent"/>.
    /// <p>
    /// Used to determine the location of the <c>staticwebassets.*.json</c> files, which are
    /// required for any BlazorWebView to function (and produced automatically on build by default,
    /// you shouldn't have to worry about that).
    /// </p>
    /// </summary>
    internal static DirectoryInfo? TestProjectDir { get; private set; }

    /// <summary>
    /// Set <see cref="TestProjectDir"/> based on a <c>DirectoryInfo</c>.
    /// This should be something like <c>new DirectoryInfo(&quot;../My.BlazorLibrary/&quot;)</c>.
    /// </summary>
    public static void SetTestProjectDir(DirectoryInfo testProjectDir)
    {
        if (testProjectDir is not { Exists: true })
            throw new DirectoryNotFoundException(testProjectDir.FullName);
        TestProjectDir = testProjectDir;
    }

    /// <summary>
    /// Set <see cref="TestProjectDir"/> based on a string. This should be something like
    /// <c>&quot;../My.BlazorLibrary/&quot;</c>.
    /// </summary>
    public static void SetTestProjectDir(string testProjectDir) 
        => SetTestProjectDir(new DirectoryInfo(testProjectDir));

    /// <summary>
    /// Set <see cref="TestProjectDir"/> based on an <c>Assembly</c>. Something like
    /// <c>typeof(MyTypeInMyTestProject).Assembly</c>.
    /// <p>
    /// This will find the project directory by walking up the filesystem until a
    /// <c>.csproj</c> file is found.
    /// </p>
    /// </summary>
    public static void SetTestProjectDir(Assembly testProjectAssembly)
    {
        FileInfo csprojFile = CsProjFileFinder.GetCsProjForAssembly(testProjectAssembly);
        SetTestProjectDir(csprojFile.Directory!);
    }
}