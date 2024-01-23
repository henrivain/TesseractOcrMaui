namespace TesseractOcrMaui.Tessdata;

/// <summary>
/// Extensions to convert TessDataState to success boolean
/// </summary>
public static class TessDataExtensions
{
    /// <summary>
    /// Check if state is success.
    /// </summary>
    /// <param name="state"></param>
    /// <returns>True if success, otherwise false.</returns>
    public static bool FinishedWithSuccess(this TessDataState state) 
        => state is TessDataState.AllValid or TessDataState.AtLeastOneValid;

    /// <summary>
    /// Check if state is not success.
    /// </summary>
    /// <param name="state"></param>
    /// <returns>True if not successful, otherwise false.</returns>
    public static bool NotSuccess(this TessDataState state) => state.FinishedWithSuccess() is false;
}
