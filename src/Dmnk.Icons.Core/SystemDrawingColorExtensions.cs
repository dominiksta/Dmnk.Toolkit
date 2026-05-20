namespace Dmnk.Icons.Core;

/// <summary>
/// See <see cref="ToHexString"/>
/// </summary>
public static class SystemDrawingColorExtensions
{
    /// <summary>
    /// Converts a <see cref="System.Drawing.Color"/> to a hex string in the format
    /// "<c>#RRGGBB</c>", for example to be used in CSS styles.
    /// </summary>
    public static string ToHexString(this System.Drawing.Color color) => 
        $"#{color.R:X2}{color.G:X2}{color.B:X2}";
}