using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Dmnk.Blazor.Mvvm;

/// <summary>
/// Like <see cref="ViewModelRegistry"/>, but with an additional method <see cref="AutoRegister"/>
/// that automatically registers all ViewModels and their corresponding Views based on reflection.
///
/// <p>
/// You may prefer using the source generator - see <see cref="ViewModelForAttribute"/>.
/// </p>
/// </summary>
[RequiresUnreferencedCode(AutoRegisterTrimmingNotice)]
[RequiresDynamicCode(AutoRegisterTrimmingNotice)]
public class ReflectionAutoViewModelRegistry(ILogger<ReflectionAutoViewModelRegistry> log) 
    : ViewModelRegistry(log)
{
    private const string AutoRegisterTrimmingNotice = $"""
        Automatic registration of ViewModels and Views does not work with trimming or AOT
        compilation. Trimming is enabled by default in Blazor WebAssembly apps when using
        'dotnet publish'. If you want to use AutoRegister, you need to either:
        
        1. Disable trimming in your project (not recommended in WASM), or
        2. Use manual registration methods (Register<TViewModel, TComponent>) instead.
        """;
    
    /// <summary>
    /// Automatically registers all ViewModel types that implement INotifyPropertyChanged and their
    /// corresponding View types that inherit from MvvmComponentBase&lt;T&gt; based on reflection.
    /// </summary>
    [RequiresUnreferencedCode(AutoRegisterTrimmingNotice)]
    [RequiresDynamicCode(AutoRegisterTrimmingNotice)]
    public void AutoRegister()
    {
        var baseType = typeof(MvvmComponentBase<>);
        
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        
        foreach (var type in assemblies.SelectMany(a => a.GetTypes()))
        {
            if (type.IsAbstract || !type.IsClass)
                continue;

            var current = type.BaseType;
            while (current != null)
            {
                if (current.IsGenericType && current.GetGenericTypeDefinition() == baseType)
                {
                    var vmType = current.GetGenericArguments()[0];
                    RegisterDynamic(vmType, type);
                    break;
                }
                current = current.BaseType;
            }
        }
    }
}