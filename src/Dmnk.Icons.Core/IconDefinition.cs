using System.Drawing;

namespace Dmnk.Icons.Core;

/// <summary>
/// An icon definition, as in, the technical name and the actual icon data.
/// </summary>
public abstract class IconDefinition(string technicalName)
{
    /// <summary>
    /// The technical name of the icon. Not used for much, but can be useful for debugging or
    /// perhaps some runtime icon access logic. Note that <em>you</em> have to ensure that
    /// these are unique when adding new icons (if you care about that).
    /// </summary>
    public string TechnicalName { get; } = technicalName;
    
    /// <summary>
    /// Returns a 10x10 <see cref="Icon"/> of this definition. This may return a specific 10x10
    /// version of the icon or a stretched version, depending on the implementation.
    /// </summary>
    public virtual Icon Size10 => new(this, new Size(10, 10));
    /// <summary>
    /// Returns a 12x12 <see cref="Icon"/> of this definition. This may return a specific 12x12
    /// version of the icon or a stretched version, depending on the implementation.
    /// </summary>
    public virtual Icon Size12 => new(this, new Size(12, 12));
    /// <summary>
    /// Returns a 16x16 <see cref="Icon"/> of this definition. This may return a specific 16x16
    /// version of the icon or a stretched version, depending on the implementation.
    /// </summary>
    public virtual Icon Size16 => new(this, new Size(16, 16));
    /// <summary>
    /// Returns a 20x20 <see cref="Icon"/> of this definition. This may return a specific 20x20
    /// version of the icon or a stretched version, depending on the implementation.
    /// </summary>
    public virtual Icon Size20 => new(this, new Size(20, 20));
    /// <summary>
    /// Returns a 24x24 <see cref="Icon"/> of this definition. This may return a specific 24x24
    /// version of the icon or a stretched version, depending on the implementation.
    /// </summary>
    public virtual Icon Size24 => new(this, new Size(24, 24));
    /// <summary>
    /// Returns a 28x28 <see cref="Icon"/> of this definition. This may return a specific 28x28
    /// version of the icon or a stretched version, depending on the implementation.
    /// </summary>
    public virtual Icon Size28 => new(this, new Size(28, 28));
    /// <summary>
    /// Returns a 32x32 <see cref="Icon"/> of this definition. This may return a specific 32x32
    /// version of the icon or a stretched version, depending on the implementation.
    /// </summary>
    public virtual Icon Size32 => new(this, new Size(32, 32));
    /// <summary>
    /// Returns a 64x64 <see cref="Icon"/> of this definition. This may return a specific 64x64
    /// version of the icon or a stretched version, depending on the implementation.
    /// </summary>
    public virtual Icon Size48 => new(this, new Size(48, 48));
    
    /// <summary>
    /// Returns a custom size <see cref="Icon"/> of this definition. This may return a specific
    /// size version of the icon or a stretched version, depending on the implementation.
    /// </summary>
    public virtual Icon CustomSize(int width, int height) => new(this, new Size(width, height));
    /// <summary>
    /// Returns a custom size <see cref="Icon"/> of this definition. This may return a specific
    /// size version of the icon or a stretched version, depending on the implementation.
    /// </summary>
    public virtual Icon CustomSize(int size) => new(this, new Size(size, size));
    /// <summary>
    /// Returns a custom size <see cref="Icon"/> of this definition. This may return a specific
    /// size version of the icon or a stretched version, depending on the implementation.
    /// </summary>
    public virtual Icon CustomSize(Size size) => new(this, size);
}

/// <summary>
/// Defines an icon that is represented as a PNG. The PNG data is stored as a byte array, which can
/// be used to create an <see cref="Icon"/> of this definition. Note that the actual size of the PNG
/// data may not match the size of the <see cref="Icon"/> returned by the size properties, in which
/// case the PNG data will be stretched to fit the requested size.
/// </summary>
public class PngIconDefinition(string technicalName) : IconDefinition(technicalName)
{
    /// <summary> The PNG data. </summary>
    public virtual byte[] Png { get; } = [];
}

/// <summary>
/// Defines an icon that is represented as an SVG. The SVG data is stored as a string, which can be
/// used to create an <see cref="Icon"/> of this definition. Note that the actual size of the SVG
/// data may not match the size of the <see cref="Icon"/> returned by the size properties, in which
/// case the SVG data will be stretched to fit the requested size.
/// </summary>
public class SvgIconDefinition(string technicalName) : IconDefinition(technicalName)
{
    /// <summary>
    /// The SVG data. This should be a valid SVG string that can be rendered as an icon.
    /// The actual <c>&lt;svg&gt;</c> tag should be excluded from this string, as the rendering
    /// logic will wrap it in an <c>&lt;svg&gt;</c> tag with the appropriate size and color.
    /// </summary>
    public virtual string Svg { get; } = string.Empty;
}
