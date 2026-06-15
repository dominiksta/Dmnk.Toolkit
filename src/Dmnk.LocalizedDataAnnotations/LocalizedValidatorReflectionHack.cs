using System;
using System.Linq;
using System.Reflection;
using System.Resources;
using Dmnk.LocalizedDataAnnotations.Properties;

namespace Dmnk.LocalizedDataAnnotations;

/// <summary>
/// See <see cref="Hack"/>
/// </summary>
public static class LocalizedValidatorReflectionHack
{
    /// <summary>
    /// Indicates whether the hack is currently applied.
    /// </summary>
    public static bool IsHacked { get; private set; }

    private static ResourceManager? _originalResourceManager;
    
    /// <summary>
    /// Uses some reflection hackery to make <c>System.ComponentModel.DataAnnotations</c> use
    /// a ResourceManager that is actually localized.
    ///
    /// <p>
    /// Calling the method multiple times will do nothing on subsequent calls.
    /// </p>
    ///
    /// <p>
    /// Credits to `JBSnorro` on StackOverflow: <see href="https://stackoverflow.com/a/57428328"/>
    /// </p>
    /// </summary>
    /// <param name="doThrow">
    /// By default, the method will throw if it fails to hack the ResourceManager in Debug builds,
    /// and will not throw in Release builds, instead just logging the failure to the console.
    /// </param>
    /// <param name="resourceManager">
    /// By default, the method will use the ResourceManager defined by this library's resources.
    /// These contain the old .NET Framework localizations. You may provide your own
    /// ResourceManager if you want to use your own localizations.
    /// <p>
    /// Note that the resource manager must provide the relevant keys. You may reference
    /// <see cref="MessageProvider.IDefaultValidationMessageProvider"/> for a list of
    /// necessary keys related to the actual validation and the resx files in the repo for
    /// a full list of all keys need to be defined for e.g. exceptions to work properly.
    /// </p>
    /// </param>
    public static void Hack(bool? doThrow = null, ResourceManager? resourceManager = null)
    {
        doThrow ??= ShouldThrowDefault();
        
        if (IsHacked) return;
        
        try { DoHack(resourceManager); }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to hack the ResourceManager: {ex}");
            if (doThrow.Value) throw;
        }
        
        IsHacked = true;
    }

    /// <summary>
    /// Revert the hack done by <see cref="Hack"/>.
    /// This will restore the original (unlocalized) ResourceManager.
    /// </summary>
    /// <param name="doThrow">
    /// By default, the method will throw if it fails to unhack the ResourceManager in Debug builds,
    /// and will not throw in Release builds, instead just logging the failure to the console.
    /// </param>
    public static void UnHack(bool? doThrow = null)
    {
        doThrow ??= ShouldThrowDefault();
        
        if (!IsHacked) return;
        
        try { DoUnHack(); }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to un-hack the ResourceManager: {ex}");
            if (doThrow.Value) throw;
        }
        
        IsHacked = false;
    }

    private static bool ShouldThrowDefault()
    {
        #if DEBUG
        return true;
        #else
        return false;
        #endif
    }

    private static void DoHack(ResourceManager? resourceManager)
    {
        EnsureAssemblyIsLoaded();

        FieldInfo resourceManagerFieldInfo = GetResourceManagerFieldInfo();
        resourceManager ??= I18nLocalizedDataAnnotations.ResourceManager;
        _originalResourceManager ??= (ResourceManager)resourceManagerFieldInfo.GetValue(null)!;
        resourceManagerFieldInfo.SetValue(null, resourceManager);
    }

    private static void DoUnHack()
    {
        if (_originalResourceManager == null) throw new InvalidOperationException(
            "Cannot un-hack ResourceManager because original ResourceManager is not stored.");
        
        FieldInfo resourceManagerFieldInfo = GetResourceManagerFieldInfo();
        resourceManagerFieldInfo.SetValue(null, _originalResourceManager);
    }
    
    private static FieldInfo GetResourceManagerFieldInfo()
    {
        var srAssembly = AppDomain.CurrentDomain
            .GetAssemblies()
            .First(assembly => assembly.FullName.StartsWith("System.ComponentModel.Annotations,"));
        var srType = srAssembly.GetType("System.SR");
        if (srType == null) throw new InvalidOperationException(
            "Could not find System.SR type in System.ComponentModel.Annotations assembly.");
        var field = srType.GetField("s_resourceManager", BindingFlags.Static | BindingFlags.NonPublic);
        if (field == null) throw new InvalidOperationException(
            "Could not find s_resourceManager field in System.SR type.");
        return field;
    }
    
    internal static ResourceManager GetCurrentResourceManager()
    {
        EnsureAssemblyIsLoaded();
        FieldInfo resourceManagerFieldInfo = GetResourceManagerFieldInfo();
        return (ResourceManager)resourceManagerFieldInfo.GetValue(null)!;
    }

    /// <summary>
    /// Force lazy initialization of System.SR.s_resourceManager, which only happens on the first
    /// call to GetResourceString. Without this, s_resourceManager is null, and we would store null
    /// as _originalResourceManager, making UnHack impossible.
    /// </summary>
    private static void EnsureAssemblyIsLoaded()
    {
        _ = new System.ComponentModel.DataAnnotations.RequiredAttribute().FormatErrorMessage("x");
    }
}