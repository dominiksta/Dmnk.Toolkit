#if _WINDOWS
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;

namespace Dmnk.Blazor.InteractiveTests;

internal sealed class BlazorInteractiveTestForm<T> : Form where T : ComponentBase
{
    private readonly Size _clientSize;
    private readonly Size _minimumSize;
    private readonly ServiceProvider _serviceProvider;
    private readonly InteractiveTestBlazorWebView _blazorWebView;
    
    public BlazorInteractiveTestForm(
        InteractiveTestsProjectPathInfo pathInfo,
        Dictionary<string, object?>? parameters = null,
        IServiceCollection? services = null,
        Size? clientSize = null,
        Size? minimumSize = null)
    {
        _clientSize = clientSize ?? new Size(800, 600);
        _minimumSize = minimumSize ?? new Size(800, 600);
        services ??= new ServiceCollection();
        services.AddWindowsFormsBlazorWebView();
        _serviceProvider =  services.BuildServiceProvider();

        Initialize();

        _blazorWebView = new InteractiveTestBlazorWebView(pathInfo)
        {
            Dock = DockStyle.Fill,
            HostPage = "index.html",
            Services = _serviceProvider,
            StartPath = "/"
        };

        _blazorWebView.RootComponents.Add<T>(
            "#app", parameters ?? new Dictionary<string, object?>());
        _blazorWebView.RootComponents.Add<HeadOutlet>("#head-outlet");

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
        Text = $"{typeof(T).Name} [Interactive Test]";
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