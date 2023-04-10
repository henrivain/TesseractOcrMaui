using TesseractOcrMaui.Tessdata;

namespace TesseractOcrMaui.Results;

/// <summary>
/// Result from loaded traineddata files.
/// </summary>
public readonly struct DataLoadResult
{
    /// <summary>
    /// Load state, 
    /// </summary>
    public required TessDataState State { get; init; }

    /// <summary>
    /// Files that were invalid (Cannot be found from app packages).
    /// </summary>
    public string[]? InvalidFiles { get; init; }

    /// <summary>
    /// Short message about traineddata load status.
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// All errors that occurred during traineddata loading.
    /// </summary>
    public string[]? Errors { get; init; }
}
