namespace Dmnk.Icons.Blazor;

public static class ColorExtensions
{
    public static string ToHexString(this System.Drawing.Color color) => 
        $"#{color.R:X2}{color.G:X2}{color.B:X2}";
}