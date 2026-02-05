using Microsoft.JSInterop;

namespace Dmnk.Blazor.Cookies;

public static class BrowserCookieAccess
{
    public static async ValueTask<string?> GetCookie(
        IJSRuntime js, string name, CancellationToken ct = default
    )
    {
        return await js.InvokeAsync<string?>("__blazorGetCookie", ct, name);  
    } 
}