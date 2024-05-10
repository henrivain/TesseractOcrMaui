namespace TesseractOcrMaui.Imaging;

/// <summary>
/// 2d bounding box inside image borders. All coordinates are calculated from Top-Right image corner.
/// </summary>
public readonly struct BoundingBox 
{
    /// <summary>
    /// Coordinate rectangle inside image borders. All coordinates are calculated from Top-Right image corner.
    /// </summary>
    /// <param name="x1">Horizontal left coordinate.</param>
    /// <param name="y1">Vertical upper coordinate.</param>
    /// <param name="x2">Horizontal right coordinate.</param>
    /// <param name="y2">Vertical lower coordinate.</param>
    public BoundingBox(int x1, int y1, int x2, int y2)
    {
        if (x1 < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(x1), "Coordinate must be non negative");
        }
        if (y1 < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(x1), "Coordinate must be non negative");
        }
        if (x1 > x2)
        {
            throw new ArgumentOutOfRangeException(nameof(x1),
                "Horizontal left coordinate cannot be more than horizontal right");
        }
        if (y1 > y2)
        {
            throw new ArgumentOutOfRangeException(nameof(x1),
                "Vertical upper coordinate cannot be more than horizontal lower");
        }
        X1 = x1;
        Y1 = y1;
        X2 = x2;
        Y2 = y2;
    }
    /// <summary>
    /// Horizontal left coordinate.
    /// </summary>
    public int X1 { get; }

    /// <summary>
    /// Horizontal right coordinate.
    /// </summary>
    public int X2 { get; } 

    /// <summary>
    /// Vertical upper coordinate.
    /// </summary>
    public int Y1 { get; } 

    /// <summary>
    /// Vertical lower coordinate
    /// </summary>
    public int Y2 { get; }

    /// <summary>
    /// Empty bounding box with all coordinates 0.
    /// </summary>
    public static readonly BoundingBox Empty = new(0, 0, 0, 0);
}
