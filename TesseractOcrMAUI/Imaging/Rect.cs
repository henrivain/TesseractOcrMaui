namespace MauiTesseractOcr.Imaging;

/// <summary>
/// Recognizion region rectangle in image.
/// </summary>
public readonly struct Rect : IEquatable<Rect>
{
    /// <summary>
    /// First x-coordinate
    /// </summary>
    public int X1 { get; }

    /// <summary>
    /// Second x-coordinate
    /// </summary>
    public int X2 => X1 + Width;

    /// <summary>
    /// First y-coordinate
    /// </summary>
    public int Y1 { get; }

    /// <summary>
    /// Second y-coordinate
    /// </summary>
    public int Y2 => Y1 + Height;

    /// <summary>
    /// Width of the region.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Height of the region.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// New rectancle shaped region.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public Rect(int x, int y, int width, int height)
    {
        X1 = x;
        Y1 = y;
        Width = width;
        Height = height;
    }

    /// <summary>
    /// New Rect from coordinates
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="x2"></param>
    /// <param name="y2"></param>
    /// <returns>New rect corresponding to coordinates.</returns>
    public static Rect FromCoords(int x1, int y1, int x2, int y2) => new(x1, y1, x2 - x1, y2 - y1);

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Rect rect && Equals(rect);
    }

    /// <inheritdoc/>
    public bool Equals(Rect other)
    {
        return X1 == other.X1 &&
               Y1 == other.Y1 &&
               Width == other.Width &&
               Height == other.Height;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return 0 + 1000000007 * X1.GetHashCode() + 1000000009 * Y1.GetHashCode() +
            1000000021 * Width.GetHashCode() + 1000000033 * Height.GetHashCode();
    }

    /// <inheritdoc/>
    public static bool operator ==(Rect lhs, Rect rhs) => lhs.Equals(rhs);
    /// <inheritdoc/>
    public static bool operator !=(Rect lhs, Rect rhs) => !(lhs == rhs);

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[Rect X={X1}, Y={Y1}, Width={Width}, Height={Height}]";
    }

}