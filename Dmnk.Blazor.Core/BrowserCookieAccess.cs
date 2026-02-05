using Microsoft.JSInterop;

namespace Dmnk.Blazor.Core;

public static class BrowserCookieAccess
{
    public static async ValueTask<string?> GetCookie(
        IJSRuntime js, string name, CancellationToken ct = default
    )
    {
        await EnsureInit(js);
        return await js.InvokeAsync<string?>("__blazorGetCookie", ct, name);  
    } 
    
    //language=js
    private const string Js = """
      window.__blazorGetCookie = (name) => {
          try {
              const found = document.cookie
                  .split(';')
                  .map(c => c.trim().split('='))
                  .filter(c => c[0] === name);
              if (found.length === 0) return null; 
              return found[0][1];
          } catch (e) {
            console.warn(`could not get cookie ${name}`, e);
            return null;
          }
      };
      """;

    private static readonly InlineJs.InitFunc EnsureInit = InlineJs.MkInitFunc(Js);
}