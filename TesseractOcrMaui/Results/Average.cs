namespace TesseractOcrMaui.Results;

/// <summary>
/// Count dynamic average with given values.
/// </summary>
public struct Average
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

    /// <summary>
    /// Add numbers to average and recalculate.
    /// </summary>
    /// <param name="nums"></param>
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

    /// <summary>
    /// Add numbers to average and recalculate.
    /// </summary>
    /// <param name="nums"></param>
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

    /// <summary>
    /// Add numbers to average and recalculate.
    /// </summary>
    /// <param name="num"></param>
    public void ReCalculate(float num) => ReCalculate((double)num);

    /// <summary>
    /// Add numbers to average and recalculate.
    /// </summary>
    /// <param name="num"></param>
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
