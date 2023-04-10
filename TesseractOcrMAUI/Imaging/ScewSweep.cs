namespace TesseractOcrMaui.Imaging;

/// <summary>
/// Represents the parameters for a sweep search used by scew algorithms.
/// </summary>
public struct ScewSweep
{
    /// <summary>
    /// ScewSweep with default values.
    /// </summary>
    public static ScewSweep Default { get; } = new(DefaultReduction, DefaultRange, DefaultDelta);

    /// <summary>
    /// Default value for reduction. Sweep part; 4 is good
    /// </summary>
    public const int DefaultReduction = 4;

    /// <summary>
    /// Default value for range.
    /// </summary>
    public const float DefaultRange = 7.0F;

    /// <summary>
    /// Default value for delta.
    /// </summary>
    public const float DefaultDelta = 1.0F;

    /// <summary>
    /// New empty ScewSweep.
    /// </summary>
    public ScewSweep() { }

    /// <summary>
    /// new ScewSweep with given values.
    /// </summary>
    /// <param name="reduction"></param>
    /// <param name="range"></param>
    /// <param name="delta"></param>
    public ScewSweep(int reduction, float range, float delta)
    {
        Reduction = reduction;
        Range = range;
        Delta = delta;
    }

    /// <summary>
    /// ScewSweep reduction value. If not provided uses DefaultReduction 4.
    /// </summary>
    public int Reduction { get; init; } = DefaultReduction;

    /// <summary>
    /// ScewSweep range value. If not provided uses DefaultRange 7.0F.
    /// </summary>
    public float Range { get; init; } = DefaultRange;

    /// <summary>
    /// ScewSweep delta value. If not provided uses DefaultDelta 1.0F.
    /// </summary>
    public float Delta { get; init; } = DefaultDelta;

}
