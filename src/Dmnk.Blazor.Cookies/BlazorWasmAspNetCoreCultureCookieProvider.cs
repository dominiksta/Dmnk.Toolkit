using System.Globalization;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Dmnk.Blazor.Cookies;

/// <summary>
/// See <see cref="Configure"/>.
/// </summary>
public static class BlazorWasmAspNetCoreCultureCookieProvider
{
    /// <summary>
    /// Set blazor wasm culture to ASP.NET Core culture from cookie.
    ///
    /// Example (in Program.cs)
    /// <code>
    /// builder.Services...
    /// 
    /// const string defaultCulture = "de";
    /// 
    /// CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(defaultCulture);
    /// CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(defaultCulture);
    /// 
    /// var host = builder.Build();
    /// 
    /// await BlazorAspNetCoreCultureCookieProvider.Configure(host.Services, defaultCulture);
    /// 
    /// await host.RunAsync();
    /// </code>
    /// </summary>
    public static async Task Configure(
        IServiceProvider container, 
        string defaultCulture,
        string cookieName = ".AspNetCore.Culture",
        Action<Exception>? onError = null
    )
    {
        string? c, uic;
        try
        {
            var js = container.GetRequiredService<IJSRuntime>();
            var cookie = await BrowserCookieAccess.GetCookie(js, cookieName);
            (c, uic) = ParseAspNetCultureCookie(cookie);
        }
        catch (Exception ex)
        {
            onError?.Invoke(ex);
            (c, uic) = (null, null);
        }
        
        CultureInfo.DefaultThreadCurrentCulture = 
            CultureInfo.GetCultureInfo(c ?? defaultCulture);
        CultureInfo.DefaultThreadCurrentUICulture = 
            CultureInfo.GetCultureInfo(uic ?? defaultCulture);

        // format example: c=en-UK|uic=en-US
        (string? Culture, string? UiCulture) ParseAspNetCultureCookie(string? cookieValue)
        {
            if (cookieValue == null) return (null, null);
            string urlDecoded = HttpUtility.UrlDecode(cookieValue);
            var splitByPipe = urlDecoded.Split("|");
            
            // only one of c & uic is provided
            if (splitByPipe.Length == 1) return ParseSingle(splitByPipe[0]);

            // wrong format
            if (splitByPipe.Length > 2) return (null, null);

            var (c1, uic1) = ParseSingle(splitByPipe[0]);
            var (c2, uic2) = ParseSingle(splitByPipe[0]);

            return c1 != null ? (c1, uic2) : (c2, uic1);

            (string? Culture, string? UiCulture) ParseSingle(string single)
            {
                var kv = single.Split("=");
                if (kv.Length == 0) return (null, null);
                return kv[0].Contains("uic") ? (null, kv[1]) : (kv[1], null);
            }
        }
    }
}