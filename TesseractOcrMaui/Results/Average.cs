namespace TesseractOcrMaui.Results;

/// <summary>
/// Count dynamic average with given values.
/// </summary>
public struct Average : IAverage
{
    /// <summary>
    /// Count dynamic average with given values.
    /// </summary>
    public Average() { }

    /// <summary>
    /// Count dynamic average with given values.
    /// </summary>
    /// <param name="nums"></param>
    public Average(params float[] nums) => ReCalculate(nums);

    /// <summary>
    /// Count dynamic average with given values.
    /// </summary>
    /// <param name="nums"></param>
    public Average(params double[] nums) => ReCalculate(nums);

    /// <inheritdoc/>

    public void ReCalculate(params float[] nums)
    {
        if (nums is null || nums.LongLength is 0)
        {
            return;
        }
        foreach (float num in nums)
        {
            ReCalculate(num);
        }
    }

    /// <inheritdoc/>
    public void ReCalculate(params double[] nums)
    {
        if (nums is null ||nums.LongLength is 0)
        {
            return;
        }
        foreach (double num in nums)
        {
            ReCalculate(num);
        }
    }

    /// <inheritdoc/>
    public void ReCalculate(float num) => ReCalculate((double)num);

    /// <inheritdoc/>
    public void ReCalculate(double num)
    {
        checked
        {
            _count++;
            _sum += num;

            Value = _sum / _count;
        }
    }

    double _sum = 0;

    long _count = 0;



    /// <summary>
    /// Current average
    /// </summary>
    public double Value { get; private set; } = 0;
}
