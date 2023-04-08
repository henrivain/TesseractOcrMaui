using MauiTesseractOcr.Results;
using MauiTesseractOcr.Tessdata;

namespace MauiTesseractOcr.Extensions;

/// <summary>
/// Extensions to ease the use of enums and enum statuses..
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Check if status means success.
    /// </summary>
    /// <param name="status"></param>
    /// <returns>True if success, otherwise false.</returns>
    public static bool Success(this RecognizionStatus status) => status is RecognizionStatus.Success;

    /// <summary>
    /// Check if status is not success.
    /// </summary>
    /// <param name="status"></param>
    /// <returns>True if ussuccessful, otherwise false.</returns>
    public static bool NotSuccess(this RecognizionStatus status) => Success(status) is false;

    /// <summary>
    /// Check if result has success code.
    /// </summary>
    /// <param name="result"></param>
    /// <returns>True if success, otherwise false.</returns>
    public static bool Success(this RecognizionResult result) => result.Status.Success();

    /// <summary>
    /// Check if result was not success code.
    /// </summary>
    /// <param name="result"></param>
    /// <returns>True if ussuccessful, otherwise false.</returns>
    public static bool NotSuccess(this RecognizionResult result) => result.Status.NotSuccess();

    /// <summary>
    /// Check if state is success.
    /// </summary>
    /// <param name="state"></param>
    /// <returns>True if success, otherwise false.</returns>
    public static bool Success(this TessDataState state) => state is TessDataState.AllValid or TessDataState.AtLeastOneValid;

    /// <summary>
    /// Check if state is not success.
    /// </summary>
    /// <param name="state"></param>
    /// <returns>True if not successful, otherwise false.</returns>
    public static bool NotSuccess(this TessDataState state) => state.Success() is false;

    /// <summary>
    /// Check if result has success code.
    /// </summary>
    /// <param name="result"></param>
    /// <returns>True if success, otherwise false.</returns>
    public static bool Success(this DataLoadResult result) => result.State.Success();

    /// <summary>
    /// Check if result was not success code.
    /// </summary>
    /// <param name="result"></param>
    /// <returns>True if ussuccessful, otherwise false.</returns>
    public static bool NotSuccess(this DataLoadResult result) => result.State.NotSuccess();

}
