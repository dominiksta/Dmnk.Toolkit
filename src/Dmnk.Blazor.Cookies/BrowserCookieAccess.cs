using System.Net;
using Microsoft.JSInterop;

namespace Dmnk.Blazor.Cookies;

/// <summary>
/// Allows direct access to cookie values through js interop
/// </summary>
public static class BrowserCookieAccess
{
    /// <summary>
    /// Get the value (and only the value, not the expiration date etc.) of a cookie by its name.
    /// Returns null if the cookie is not found.
    /// </summary>
    public static async ValueTask<string?> GetCookieValue(
        IJSRuntime js, string name, CancellationToken ct = default
    )
    {
        var cookies = await js.InvokeAsync<string?>("document.cookie", ct, name);
        if (cookies == null) return null;
        var found = cookies
            .Split(';')
            .Select(c => c.Trim().Split('='))
            .Where(c => c.Length == 2 && c[0] == name).ToArray();
        if (found.Length == 0) return null;
        var value = found[0][1];
        return WebUtility.UrlDecode(value);
    }
}