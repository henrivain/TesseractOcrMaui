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


}
