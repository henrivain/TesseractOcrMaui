// Code copied from https://github.com/charlesw/tesseract
namespace TesseractOcrMaui.Imaging;

/// <summary>
/// Structure representing Pix image color.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct PixColor : IEquatable<PixColor>
{
    /// <summary>
    /// New color for Pix image.
    /// </summary>
    /// <param name="red"></param>
    /// <param name="green"></param>
    /// <param name="blue"></param>
    /// <param name="alpha"></param>
    public PixColor(byte red, byte green, byte blue, byte alpha = 255)
    {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;
    }

    /// <summary>
    /// Red value for color. 0-255.
    /// </summary>
    public byte Red { get; }

    /// <summary>
    /// Green value for color. 0-255.
    /// </summary>
    public byte Green { get; }

    /// <summary>
    /// Blue value for color. 0-255.
    /// </summary>
    public byte Blue { get; }

    /// <summary>
    /// Alpha value for opacity. 0-255.
    /// </summary>
    public byte Alpha { get; }

    /// <summary>
    /// New PixColor from RGBA
    /// </summary>
    /// <param name="value"></param>
    /// <returns>PixColor representing given value.</returns>
    public static PixColor FromRgba(uint value)
    {
        return new PixColor(
           (byte)(value >> 24 & 0xFF),
           (byte)(value >> 16 & 0xFF),
           (byte)(value >> 8 & 0xFF),
           (byte)(value & 0xFF));
    }

    /// <summary>
    /// New PixColor from RGB
    /// </summary>
    /// <param name="value"></param>
    /// <returns>PixColor representing given value.</returns>
    public static PixColor FromRgb(uint value)
    {
        return new PixColor(
           (byte)(value >> 24 & 0xFF),
           (byte)(value >> 16 & 0xFF),
           (byte)(value >> 8 & 0xFF),
           0xFF);
    }

    /// <summary>
    /// Convert PixColor to RGBA.
    /// </summary>
    /// <returns>Uint representing RGBA color.</returns>
    public uint ToRGBA()
    {
        return (uint)(Red << 24 |
           Green << 16 |
           Blue << 8 |
           Alpha);
    }


    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is PixColor color && Equals(color);
    }

    /// <inheritdoc/>
    public bool Equals(PixColor other)
    {
        return Red == other.Red && 
            Blue  == other.Blue && 
            Green == other.Green && 
            Alpha == other.Alpha;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int hashCode = 0;
        unchecked
        {
            hashCode += 1000000007 * Red.GetHashCode();
            hashCode += 1000000009 * Blue.GetHashCode();
            hashCode += 1000000021 * Green.GetHashCode();
            hashCode += 1000000033 * Alpha.GetHashCode();
        }
        return hashCode;
    }

    /// <inheritdoc/>
    public static bool operator ==(PixColor left, PixColor right) => left.Equals(right);
    /// <inheritdoc/>
    public static bool operator !=(PixColor left, PixColor right) => (left == right) is false;
    /// <inheritdoc/>
    public override string ToString() => $"Color(0x{ToRGBA():X})";  // :X converts to hex

}
