namespace MauiTesseractOcr.Imaging;

/// <summary>
/// Represents the parameters for a sweep search used by scew algorithms.
/// </summary>
public struct ScewSweep
{
    public static ScewSweep Default { get; } = new(DefaultReduction, DefaultRange, DefaultDelta);


    public const int DefaultReduction = 4; // Sweep part; 4 is good
    public const float DefaultRange = 7.0F;
    public const float DefaultDelta = 1.0F;

    public ScewSweep() { }

    public ScewSweep(int reduction, float range, float delta)
    {
        Reduction = reduction;
        Range = range;
        Delta = delta;
    }

    public int Reduction { get; init; } = DefaultReduction;
    public float Range { get; init; } = DefaultRange;
    public float Delta { get; init; } = DefaultDelta;

}
