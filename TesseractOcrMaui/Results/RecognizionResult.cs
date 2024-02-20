namespace TesseractOcrMaui.Results;

/// <summary>
/// Result that is returned from ITesseract text tecognizion
/// </summary>
public readonly struct RecognizionResult
{
    /// <summary>
    /// new Result that is returned from ITesseract text tecognizion
    /// </summary>
    public RecognizionResult() { }

    /// <summary>
    /// Status of last recognision
    /// </summary>
    public required RecognizionStatus Status { get; init; }

    /// <summary>
    /// More information about recognizion status.
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// Optional exception that was thrown if failed.
    /// </summary>
    public Exception? Exception { get; init; }

    /// <summary>
    /// Recognized text from the image.
    /// </summary>
    public string? RecognisedText { get; init; }

    /// <summary>
    /// Confidence of text recognizion, negative means not set.
    /// </summary>
    public float Confidence { get; init; } = -1f;

    /// <summary>
    /// Recognizion state that is neither success nor failure.
    /// </summary>
    public static readonly RecognizionResult InProgress = new()
    {
        Status = RecognizionStatus.InProgressSuccess,
        Message = "Recognizion in progress, state is neither success nor failed"
    };
}
