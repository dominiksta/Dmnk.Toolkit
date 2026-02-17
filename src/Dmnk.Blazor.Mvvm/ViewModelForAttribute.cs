using System;

namespace Dmnk.Blazor.Mvvm;

/// <summary>
/// Marks a ViewModel to be automatically registered with its corresponding view component.
/// The source generator will create registration code for this pairing.
/// See <see href="https://dominiksta.github.io/Dmnk.Toolkit/api/Dmnk.Blazor.Mvvm.SourceGen.html"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class ViewModelForAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ViewModelForAttribute"/> class.
    /// </summary>
    /// <param name="viewType">The type of the Blazor component (view) for this ViewModel.</param>
    public ViewModelForAttribute(Type viewType)
    {
        ViewType = viewType;
    }

    /// <summary>
    /// Gets the type of the Blazor component (view) for this ViewModel.
    /// </summary>
    public Type ViewType { get; }
}
