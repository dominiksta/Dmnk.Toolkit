using System.Drawing;

namespace Dmnk.Icons.Core;

public abstract class IconDefinition(string technicalName)
{
    public string TechnicalName { get; } = technicalName;
    
    public virtual Icon Size10 => new(this, new Size(10, 10));
    public virtual Icon Size12 => new(this, new Size(12, 12));
    public virtual Icon Size16 => new(this, new Size(16, 16));
    public virtual Icon Size20 => new(this, new Size(20, 20));
    public virtual Icon Size24 => new(this, new Size(24, 24));
    public virtual Icon Size28 => new(this, new Size(28, 28));
    public virtual Icon Size32 => new(this, new Size(32, 32));
    public virtual Icon Size48 => new(this, new Size(48, 48));
    
    public virtual Icon CustomSize(int width, int height) => new(this, new Size(width, height));
    public virtual Icon CustomSize(int size) => new(this, new Size(size, size));
    public virtual Icon CustomSize(Size size) => new(this, size);
}

public class PngIconDefinition(string technicalName) : IconDefinition(technicalName)
{
    public byte[] Png { get; } = [];
}

public class SvgIconDefinition(string technicalName) : IconDefinition(technicalName)
{
    public string Svg { get; set; } = string.Empty;
}
