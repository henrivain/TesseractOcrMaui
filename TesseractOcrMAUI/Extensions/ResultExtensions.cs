using TesseractOcrMaui.Results;
using TesseractOcrMaui.Tessdata;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace TesseractOcrMaui.Extensions;

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

    /// <summary>
    /// Get paths to invalid files as strings.
    /// Every path is in new line.
    /// </summary>
    /// <param name="result"></param>
    /// <returns>Empty string if none invalid, otherwise paths in new lines</returns>
    internal static string GetInvalidFilesString(this DataLoadResult result)
    {
        if (result.InvalidFiles is null)
        {
            return string.Empty;
        }
        return string.Join(",\n", result.InvalidFiles);
    }

    /// <summary>
    /// Get number of invalid files.
    /// </summary>
    /// <param name="result"></param>
    /// <returns>0 if InvalidFiles is null, otherwise invalid file count.</returns>
    internal static int GetErrorCount(this DataLoadResult result)
    {
        return result.InvalidFiles?.Length ?? 0;
    }

    /// <summary>
    /// Get comma separated list of errors.
    /// </summary>
    /// <param name="result"></param>
    /// <returns>Empty string if Errors null, otherwise comma separated string of errors</returns>
    internal static string GetErrorsString(this DataLoadResult result)
    {
        if (result.Errors is null)
        {
            return string.Empty;
        }
        return string.Join(", ", result.Errors);
    }

    /// <summary>
    /// Log errors and invalid paths if not successfull or has any invalid files.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <param name="logger">Logger</param>
    internal static void LogLoadErrorsIfNotAllSuccess<T>(this DataLoadResult result, T logger) where T : ILogger
    {
        if (logger is null)
        {
            Debug.WriteLine($"Cannot log, no logger passed to " +
                $"'{nameof(ResultExtensions)}.{nameof(LogLoadErrorsIfNotAllSuccess)}'.");
            return;
        }
        if (result.NotSuccess() || result.InvalidFiles?.Length > 0) 
        {
            var statusStr = result.Success() ? "all" : "any";
            logger.LogWarning("Could not load {any/all} traineddata files, '{count}' files failed.",
                statusStr, result.GetErrorCount());
            logger.LogWarning("Here are invalid traineddata file paths: \n'{paths}'",
                result.GetInvalidFilesString());
            logger.LogWarning("Here are load errors for traineddata files: '{errors}'.",
                result.GetErrorsString());
        }
    }
}
