namespace TesseractOcrMAUILib.Imaging;
public readonly struct Rect : IEquatable<Rect>
{
    public static readonly Rect Empty;

    public int X1 { get; }

    public int Y1 { get; }

    public int X2 => X1 + Width;

    public int Y2 => Y1 + Height;

    public int Width { get; }

    public int Height { get; }

    public Rect(int x, int y, int width, int height)
    {
        X1 = x;
        Y1 = y;
        Width = width;
        Height = height;
    }

    public static Rect FromCoords(int x1, int y1, int x2, int y2)
    {
        return new Rect(x1, y1, x2 - x1, y2 - y1);
    }

    public override bool Equals(object? obj)
    {
        return obj is Rect rect && Equals(rect);
    }
    public bool Equals(Rect other)
    {
        if (X1 == other.X1 && Y1 == other.Y1 && Width == other.Width)
        {
            return Height == other.Height;
        }
        return false;
    }
    public override int GetHashCode()
    {
        return 0 + 1000000007 * X1.GetHashCode() + 1000000009 * Y1.GetHashCode() +
            1000000021 * Width.GetHashCode() + 1000000033 * Height.GetHashCode();
    }
    public static bool operator ==(Rect lhs, Rect rhs) => lhs.Equals(rhs);
    public static bool operator !=(Rect lhs, Rect rhs) => !(lhs == rhs);
    public override string ToString()
    {
        return $"[Rect X={X1}, Y={Y1}, Width={Width}, Height={Height}]";
    }

}