using Dmnk.Icons.Core;

namespace Dmnk.Toolkit.TestApp.Components.Pages.Icons;

/// <summary>
/// A simple custom SVG icon defined directly in code using <see cref="SvgIconDefinition"/>.
/// </summary>
public class StarIconDefinition() : SvgIconDefinition("star")
{
    public static readonly StarIconDefinition Instance = new();

    public override string Svg =>
        """<polygon points="12,2 15.09,8.26 22,9.27 17,14.14 18.18,21.02 12,17.77 5.82,21.02 7,14.14 2,9.27 8.91,8.26" stroke="currentColor" stroke-width="1" fill="inherit"/>""";
}
