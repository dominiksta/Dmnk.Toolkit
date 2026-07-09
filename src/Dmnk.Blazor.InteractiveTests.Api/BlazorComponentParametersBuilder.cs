using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace Dmnk.Blazor.InteractiveTests.Api;

/// <summary>
/// This class helps you build up a dictionary of parameters for a given blazor component.
/// <p>
/// You shouldn't need to instantiate this directly. Instead, you are expected to use it in
/// BlazorInteractiveTestRunner's ShowComponent method(s).
/// </p>
/// </summary>
/// <typeparam name="TComponent">The type of the blazor component</typeparam>
public sealed class BlazorComponentParametersBuilder<TComponent> where TComponent : ComponentBase
{
    private readonly Dictionary<string, object?> _values = new();

    /// <summary>
    /// Add a parameter.
    /// </summary>
    /// <example>
    /// <code>
    /// BlazorInteractiveTestRunner.ShowComponent&lt;MyComponent&gt;(
    ///     params => params
    ///         .Add(component => component.MyParameter, 4)
    ///         .Add(component => component.MyOtherParameter, "hi"));
    /// </code>
    /// </example>
    /// <param name="selector">
    /// An expression selecting a [Parameter] property of the component.
    /// </param>
    /// <param name="value">
    /// The value that <paramref name="selector"/>(myComponent) should be set to
    /// </param>
    public BlazorComponentParametersBuilder<TComponent> Add<TValue>(
        Expression<Func<TComponent, TValue>> selector,
        TValue value)
    {
        var property = GetProperty(selector);
        ValidateParameterProperty(property);
        _values[property.Name] = value;
        return this;
    }

    /// <summary>
    /// Returns the current built up dictionary of parameters.
    /// 
    /// <p>
    /// You shouldn't need to call this yourself. It is really only public if you need it
    /// for some debugging / sanity-check purpose.
    /// </p>
    /// </summary>
    public Dictionary<string, object?> Build() => _values;
    
    private static PropertyInfo GetProperty<TValue>(
        Expression<Func<TComponent, TValue>> selector)
    {
        if (selector.Body is not MemberExpression member)
            throw new ArgumentException("Selector must be a property access.", nameof(selector));

        if (member.Member is not PropertyInfo property)
            throw new ArgumentException("Selector must target a property.", nameof(selector));

        return property;
    }
    
    private static void ValidateParameterProperty(PropertyInfo property)
    {
        if (property.DeclaringType is null ||
            !typeof(TComponent).IsAssignableFrom(property.DeclaringType))
        {
            throw new ArgumentException("Selector must target the component type.");
        }

        if (property.GetCustomAttribute<ParameterAttribute>() is null &&
            property.GetCustomAttribute<CascadingParameterAttribute>() is null)
        {
            throw new ArgumentException(
                $"Property '{property.Name}' is not a Blazor parameter.");
        }
    } 
}