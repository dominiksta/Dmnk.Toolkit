using Dmnk.Blazor.Dialogs.Api;
using Dmnk.Blazor.Dialogs.DefaultDialogs;
using Dmnk.Icons.Core;

namespace Dmnk.Blazor.Dialogs;

public abstract class BlazorVmDialogController : IVmDialogController
{
    /// <summary>
    /// Register a viewmodel with its corresponding component.
    /// </summary>
    public BlazorVmDialogController Register<TComponent, TViewModel>()
        where TComponent : IVmDialogView
        where TViewModel : IVmDialogViewModel
    {
        return Register(typeof(TComponent), typeof(TViewModel));
    }

    public BlazorVmDialogController Register(Type componentType, Type viewModelType)
    {
        if (!componentType.IsAssignableTo(typeof(IVmDialogView)))
            throw new InvalidOperationException(
                $"component must be assignable to {typeof(IVmDialogView)}"
            );
        if (!viewModelType.IsAssignableTo(typeof(IVmDialogViewModel)))
            throw new InvalidOperationException(
                $"viewmodel must be assignable to {typeof(IVmDialogViewModel)}"
            );

        if (componentType.IsGenericType && !viewModelType.IsGenericType ||
            viewModelType.IsGenericType && !componentType.IsGenericType)
            throw new InvalidOperationException(
                "either both or none of component and viewmodel type must be generic"
            );
        
        if ((componentType.IsGenericType && componentType.GetGenericArguments().Length != 1)
            || (viewModelType.IsGenericType && viewModelType.GetGenericArguments().Length != 1))
            throw new InvalidOperationException(
                "Generic components are only supported with one parameter, " +
                "which will also be passed to the viewmodel"
            );

        if (componentType.IsGenericType) 
            componentType = componentType.GetGenericTypeDefinition();
        if (viewModelType.IsGenericType)
            viewModelType = viewModelType.GetGenericTypeDefinition();
        
        if (!_vm2Component.TryAdd(viewModelType, componentType))
            throw new InvalidOperationException(
                $"Already registered viewmodel of type {viewModelType}"
            );
        return this;
    }

    private readonly Dictionary<Type, Type> _vm2Component = new();
    
    public event Action<(
        VmDialogParameters Parameters, Type Component, 
        IVmDialogViewModel ViewModel, VmDialogReference Reference
    )>? OnShow;

    public event Action<IVmDialogViewModel>? OnClose;
    
    public async Task<VmDialogReference> Show<T>(VmDialogParameters parameters, T viewModel) 
        where T : IVmDialogViewModel
    {
        Type vmType = typeof(T);
        bool isGeneric = vmType.IsGenericType;
        if (isGeneric) vmType = vmType.GetGenericTypeDefinition();
        
        if (!_vm2Component.TryGetValue(vmType, out Type? component))
            throw new InvalidOperationException(
                $"No component registered for viewmodel of type {typeof(T)}"
            );

        if (typeof(T).IsGenericType)
            component = component.MakeGenericType(typeof(T).GetGenericArguments()[0]);
        
        var reference = new VmDialogReference(async () =>
        {
            // ReSharper disable once MethodHasAsyncOverload
            viewModel.OnDismiss();
            await viewModel.OnDismissAsync();
            OnClose?.Invoke(viewModel);
        });
#pragma warning disable CS0618 // Type or member is obsolete
        viewModel.Dialog = reference;
#pragma warning restore CS0618 // Type or member is obsolete
        OnShow?.Invoke((parameters, component, viewModel, reference));
        // ReSharper disable once MethodHasAsyncOverload
        return reference;
    }

    public abstract Icon DefaultIconForIntent(MessageBoxType type);
}