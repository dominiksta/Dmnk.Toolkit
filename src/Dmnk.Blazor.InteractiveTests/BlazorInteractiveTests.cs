using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Dmnk.Blazor.InteractiveTests;

public static class BlazorInteractiveTests
{
    /// <summary>
    /// Set this once globally before calling <see cref="ShowComponent"/>.
    /// Used to determine the location of the `staticwebassets.*.json` files.
    /// </summary>
    internal static DirectoryInfo? TestProjectDir { get; private set; }
    
    public static void SetTestProjectDir(DirectoryInfo testProjectDir) { TestProjectDir = testProjectDir; }

    public static void SetTestProjectDir(string testProjectDir)
    {
        var dir = new DirectoryInfo(testProjectDir);
        if (dir is not { Exists: true }) throw new DirectoryNotFoundException(testProjectDir);
        SetTestProjectDir(dir);
    }

    public static void SetTestProjectDir(Assembly testProjectAssembly)
    {
        FileInfo csprojFile = GetCsProjForAssembly(testProjectAssembly);
        SetTestProjectDir(csprojFile.Directory!);
    }

    internal static FileInfo GetCsProjForAssembly(Assembly testProjectAssembly)
    {
        var name = testProjectAssembly.GetName().Name;
        DirectoryInfo? location  = new FileInfo(testProjectAssembly.Location).Directory;
        FileInfo? csprojFile = null;
        
        while (location != null)
        {
            var found = location.EnumerateFiles("*.csproj").ToList();
            if (found.Count > 1) throw new InvalidOperationException(
                $"Multiple csproj files found at {location.FullName}");
            
            if (found.Count == 1)
            {
                csprojFile = found.First();
                break;
            }
            
            location = location.Parent;
        }
        
        if (csprojFile is null) throw new DirectoryNotFoundException(
            $".csproj not found for assembly {name}");
        
        return csprojFile;
    }
    
    /// <summary>
    /// Show the blazor component given by <typeparamref name="T"/> in a WinForms Dialog.
    /// 
    /// <p>
    /// You MUST set <see cref="TestProjectDir"/> before calling this.
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
}