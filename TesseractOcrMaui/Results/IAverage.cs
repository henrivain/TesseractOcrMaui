namespace TesseractOcrMaui.Results;

/// <summary>
/// Average calculated from given values.
/// </summary>
public interface IAverage
{
    /// <summary>
    /// Add number to average and recalculate.
    /// </summary>
    /// <param name="num"></param>
    void ReCalculate(float num);

    /// <summary>
    /// Add number to average and recalculate.
    /// </summary>
    /// <param name="num"></param>
    void ReCalculate(double num);

    /// <summary>
    /// Add numbers to average and recalculate.
    /// </summary>
    /// <param name="num"></param>
    void ReCalculate(params float[] num);

    /// <summary>
    /// Add numbers to average and recalculate.
    /// </summary>
    /// <param name="num"></param>
    void ReCalculate(params double[] num);

    /// <summary>
    /// Current calculated average.
    /// </summary>
    double Value { get; } 
}
