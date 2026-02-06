using System.Drawing;

namespace Dmnk.Icons.Core;

/// <summary>
/// Defines an icon to be rendered, including its definition, size, color, and accessible name.
/// </summary>
public sealed class Icon(
    IconDefinition definition, 
    Size size, 
    Color? color = null, 
    string? accessibleName = null
)
{
    /// <summary> See <see cref="IconDefinition"/> </summary>
    public IconDefinition Definition { get; } = definition;
    
    /// <summary>
    /// The size of the icon in px. User level zoom is obviously not considered.
    /// </summary>
    public Size Size { get; } = size;
    
    /// <summary>
    /// The color of the icon. If null, the default color of the icon will be used. This will of
    /// course also only work for some icons, e.g. svg icons that define a single path.
    /// </summary>
    public Color? Color { get; } = color;

    /// <summary>
    /// The accessible name of the icon, used for screen readers and other assistive technologies.
    /// If null, the technical name of the icon will be used as a fallback.
    /// </summary>
    public string? AccessibleName
    {
        get => field ?? Definition.TechnicalName;
    } = accessibleName;

    /// <summary>
    /// Creates a new icon with the same everything, but a different color.
    /// </summary>
    public Icon WithColor(Color color) => new(Definition, Size, color, AccessibleName);
    
    /// <summary>
    /// Creates a new icon with the same everything, but a different accessible name.
    /// </summary>
    public Icon WithAccessibleName(string accessibleName) => new(Definition, Size, Color, accessibleName);
}