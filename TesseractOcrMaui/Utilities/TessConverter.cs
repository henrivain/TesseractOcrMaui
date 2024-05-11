using System.Globalization;

namespace TesseractOcrMaui.Utilities;

/// <summary>
/// Convert object to tesseract option value string.
/// </summary>
internal static class TessConverter
{
    /// <summary>
    /// Try convert value to tesseract option string.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Null if failed, otherwise string representing given value.</returns>
    public static string? TryToString(object? value)
    {
        return value switch
        {
            bool => ToString((bool)value),
            decimal => ToString((decimal)value),
            double => ToString((double)value),
            float => ToString((float)value),
            short => ToString((short)value),
            int => ToString((int)value),
            long => ToString((long)value),
            ushort => ToString((ushort)value),
            uint => ToString((uint)value),
            ulong => ToString((ulong)value),
            string => (string)value,
            _ => null
        };
    }

    public static string ToString(bool value)
    {
        if (!value)
        {
            return "FALSE";
        }

        return "TRUE";
    }

    private static NumberFormatInfo FormatInfo { get; } = CultureInfo.InvariantCulture.NumberFormat;

    public static string ToString(decimal value)
    {
        return value.ToString("R", FormatInfo);
    }

    public static string ToString(double value)
    {
        return value.ToString("R", FormatInfo);
    }

    public static string ToString(float value)
    {
        return value.ToString("R", FormatInfo);
    }

    public static string ToString(short value)
    {
        return value.ToString("D", FormatInfo);
    }

    public static string ToString(int value)
    {
        return value.ToString("D", FormatInfo);
    }

    public static string ToString(long value)
    {
        return value.ToString("D", FormatInfo);
    }

    public static string ToString(ushort value)
    {
        return value.ToString("D", FormatInfo);
    }

    public static string ToString(uint value)
    {
        return value.ToString("D", FormatInfo);
    }

    public static string ToString(ulong value)
    {
        return value.ToString("D", FormatInfo);
    }
}
