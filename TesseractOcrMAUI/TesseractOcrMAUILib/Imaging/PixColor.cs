using System.Runtime.InteropServices;

namespace TesseractOcrMAUILib.Imaging;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct PixColor : IEquatable<PixColor>
{
    public PixColor(byte red, byte green, byte blue, byte alpha = 255)
    {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;
    }

    public byte Red { get; }
    public byte Green { get; }
    public byte Blue { get; }
    public byte Alpha { get; }

    public static PixColor FromRgba(uint value)
    {
        return new PixColor(
           (byte)(value >> 24 & 0xFF),
           (byte)(value >> 16 & 0xFF),
           (byte)(value >> 8 & 0xFF),
           (byte)(value & 0xFF));
    }

    public static PixColor FromRgb(uint value)
    {
        return new PixColor(
           (byte)(value >> 24 & 0xFF),
           (byte)(value >> 16 & 0xFF),
           (byte)(value >> 8 & 0xFF),
           0xFF);
    }

    public uint ToRGBA()
    {
        return (uint)(Red << 24 |
           Green << 16 |
           Blue << 8 |
           Alpha);
    }

#if NETFULL
        public static explicit operator System.Drawing.Color(PixColor color)
        {
            return System.Drawing.Color.FromArgb(color.alpha, color.red, color.green, color.blue);
        }

        public static explicit operator PixColor(System.Drawing.Color color)
        {
            return new PixColor(color.R, color.G, color.B, color.A);
        }
#endif


    public override bool Equals(object? obj)
    {
        return obj is PixColor color && Equals(color);
    }

    public bool Equals(PixColor other)
    {
        return Red == other.Red && Blue == other.Blue && Green == other.Green && Alpha == other.Alpha;
    }

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

    public static bool operator ==(PixColor lhs, PixColor rhs) => lhs.Equals(rhs);
    public static bool operator !=(PixColor lhs, PixColor rhs) => !(lhs == rhs);
    public override string ToString() => string.Format("Color(0x{0:X})", ToRGBA());

}
