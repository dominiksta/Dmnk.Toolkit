using System.Drawing;

namespace Dmnk.Icons.Core;

public sealed class Icon(
    IconDefinition definition, 
    Size size, 
    Color? color = null, 
    string? accessibleName = null
)
{
    public IconDefinition Definition { get; } = definition;
    public Size Size { get; } = size;
    public Color? Color { get; } = color;

    public string? AccessibleName
    {
        get => field ?? Definition.TechnicalName;
    } = accessibleName;

    public Icon WithColor(Color color) => new(Definition, Size, color, AccessibleName);
    public Icon WithAccessibleName(string accessibleName) => new(Definition, Size, Color, accessibleName);
}