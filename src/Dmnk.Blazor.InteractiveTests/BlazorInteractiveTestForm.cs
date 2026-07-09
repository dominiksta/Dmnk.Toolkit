#if _WINDOWS
using Dmnk.Blazor.InteractiveTests.Api;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;

namespace Dmnk.Blazor.InteractiveTests;

internal sealed class BlazorInteractiveTestForm : Form
{
    private readonly Size _clientSize;
    private readonly Size _minimumSize;
    private readonly ServiceProvider _serviceProvider;
    private readonly InteractiveTestBlazorWebView _blazorWebView;
    private readonly Type _sutType;
    
    public BlazorInteractiveTestForm(
        InteractiveTestsProjectPathInfo pathInfo,
        Type sutType,
        Dictionary<string, object?>? sutParameters = null,
        Type? testBedType = null,
        Dictionary<string, object?>? testBedParameters = null,
        IServiceCollection? services = null,
        Size? clientSize = null,
        Size? minimumSize = null)
    {
        _sutType = sutType;
        _clientSize = clientSize ?? new Size(800, 600);
        _minimumSize = minimumSize ?? new Size(800, 600);
        services ??= new ServiceCollection();
        services.AddWindowsFormsBlazorWebView();
        services.AddBlazorWebViewDeveloperTools();
        services.AddSingleton(new InteractiveTestContext()
        {
            TestProjectName =  pathInfo.TestProjectName,
        });
        _serviceProvider =  services.BuildServiceProvider();

        Initialize();

        _blazorWebView = new InteractiveTestBlazorWebView(pathInfo)
        {
            Dock = DockStyle.Fill,
            HostPage = "index.html",
            Services = _serviceProvider,
            StartPath = "/",
        };

        var testRootParams = new BlazorComponentParametersBuilder<TestRoot>()
            .Add(c => c.ConsumingProjectName, pathInfo.TestProjectName)
            .Add(c => c.SutType, sutType)
            .Add(c => c.SutParameters, sutParameters ?? new Dictionary<string, object?>());

        if (testBedType is not null) testRootParams
            .Add(c => c.TestBedType, testBedType)
            .Add(c => c.TestBedParameters, testBedParameters ?? new Dictionary<string, object?>());
        
        _blazorWebView.RootComponents.Add<TestRoot>("#app", testRootParams.Build());
        _blazorWebView.RootComponents.Add<HeadOutlet>("head::after");
        
        Controls.Add(_blazorWebView);
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        Activate();
    }

    private void Initialize()
    {
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = _clientSize;
        MinimumSize = _minimumSize;
        Text = $"{_sutType.Name} [Interactive Test]";
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _serviceProvider.Dispose();
            _blazorWebView.Dispose();
        }
    }
}
#endif