namespace TesseractOcrMAUILib.Results;
public readonly struct RecognizionResult
{
    public RecognizionResult() { }

    /// <summary>
    /// Status of last recognision
    /// </summary>
    public required RecognizionStatus Status { get; init; }
    
    /// <summary>
    /// Recognized text from the image.
    /// </summary>
    public string? RecognisedText { get; init; }

    /// <summary>
    /// Confidence of text recognizion, negative means not set.
    /// </summary>
    public float Confidence { get; init; } = -1f;

    /// <summary>
    /// More information about recognizion status.
    /// </summary>
    public string? Message { get; init; }
}
