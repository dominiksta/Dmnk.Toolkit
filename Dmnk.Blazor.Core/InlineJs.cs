using Microsoft.JSInterop;

namespace Dmnk.Blazor.Core;

/// <summary>
/// Defines helpers for declaring javascript inline in c# instead of bundling resource files.
/// See <see cref="MkInitFunc"/>.
/// </summary>
public static class InlineJs
{
    public delegate ValueTask InitFunc(IJSRuntime runtime);
    
    /// <summary>
    /// Create function that will 'eval' a given string of js exactly once.
    /// </summary>
    public static InitFunc MkInitFunc(string js)
    {
        bool initialized = false;
        SemaphoreSlim semaphore = new(1);
        return async (runtime) =>
        {
            if (initialized) return;
            await semaphore.WaitAsync();
            if (initialized) return;
            try
            {
                await runtime.InvokeVoidAsync("eval", js);
                initialized = true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception while evaluating js: \n{js}", ex);
            }
            finally
            {
                semaphore.Release();
            }
        };
    }
}