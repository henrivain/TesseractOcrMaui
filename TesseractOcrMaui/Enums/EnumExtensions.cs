namespace TesseractOcrMaui.Enums;

/// <summary>
/// Extensions to ease the use of enums and enum statuses..
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Check if status is success, notice that even if this is false, state might be still not failed. (See <see cref="SuccessOrInProgress(RecognizionStatus)"/>)
    /// </summary>
    /// <param name="status"></param>
    /// <returns>True if success, otherwise false.</returns>
    public static bool FinishedWithSuccess(this RecognizionStatus status) => status is RecognizionStatus.Success;

    /// <summary>
    /// Check if status is not success.
    /// </summary>
    /// <param name="status"></param>
    /// <returns>True if ussuccessful, otherwise false.</returns>
    public static bool NotSuccess(this RecognizionStatus status) => FinishedWithSuccess(status) is false
        && status is RecognizionStatus.InProgressSuccess is false;

    /// <summary>
    /// Check if status is "not failed".
    /// </summary>
    /// <param name="status"></param>
    /// <returns>True if ussuccessful, otherwise false.</returns>
    public static bool SuccessOrInProgress(this RecognizionStatus status) =>
        status is RecognizionStatus.Success or RecognizionStatus.InProgressSuccess;





    /// <summary>
    /// Go one PageIteratorLevel up from current.
    /// </summary>
    /// <param name="level"></param>
    /// <returns>PageIteratorLevel that is one level up from current (For example TextLine -> Paragraph) 
    /// or throws <see cref="InvalidOperationException"/> if at highest level <see cref="PageIteratorLevel.Block"/>.</returns>
    /// <exception cref="InvalidOperationException">If at highest level <see cref="PageIteratorLevel.Block"/>.</exception>
    public static PageIteratorLevel GoLevelUp(this PageIteratorLevel level)
    {
        return level switch
        {
            PageIteratorLevel.Symbol => PageIteratorLevel.Word,
            PageIteratorLevel.Word => PageIteratorLevel.TextLine,
            PageIteratorLevel.TextLine => PageIteratorLevel.Paragraph,
            PageIteratorLevel.Paragraph => PageIteratorLevel.Block,
            PageIteratorLevel.Block => throw new InvalidOperationException("Block is the highest level cannot go up."),
            _ => throw new NotImplementedException($"{nameof(PageIteratorLevel)} of {level} not implemented.")
        };
    }
}
