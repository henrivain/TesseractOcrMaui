namespace TesseractOcrMaui.Imaging;

/// <summary>
/// Point in 2D space
/// </summary>
public readonly struct Point2D
{
    /// <summary>
    /// New point in 2D space
    /// </summary>
    /// <param name="x">Horizontal X-coordinate</param>
    /// <param name="y">Vertical Y-coordinate</param>
    /// <exception cref="ArgumentOutOfRangeException">If any coordinate is less than 0.</exception>
    public Point2D(int x, int y)
    {
        if (x < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(x), "X coordinate cannot be less than 0");
        }
        if (y < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(y), "Y coordinate cannot be less than 0");
        }

        X = x; 
        Y = y;   
    }

    /// <summary>
    /// Horizontal X coordinate.
    /// </summary>
    public int X { get; }


    /// <summary>
    /// Vertical Y-coordinate.
    /// </summary>
    public int Y { get; }


    /// <summary>
    /// Enables to deconstruct values like
    /// <code>
    /// var (x, y) = new Point2D(3, 2);
    /// </code>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }
};
