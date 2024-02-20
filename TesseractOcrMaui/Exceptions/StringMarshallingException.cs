namespace TesseractOcrMaui.Exceptions;

/// <summary>
/// Exception thrown when library cannot marshal string returned from native library correctly.
/// </summary>
public class StringMarshallingException : TesseractException
{
    /// <summary>
    /// New Exception thrown when library cannot marshal string returned from native library correctly.
    /// </summary>
    public StringMarshallingException() : base() { }

    /// <summary>
    /// New Exception with message thrown when library cannot marshal string returned from native library correctly.
    /// </summary>
    /// <param name="message">Error reason.</param>
    public StringMarshallingException(string? message) : base(message) { }

    /// <summary>
    /// New Exception with message and inner exception. 
    /// Thrown when library cannot marshal string returned from native library correctly.
    /// </summary>
    /// <param name="message">Error reason.</param>
    /// <param name="innerException">Exception that caused this error.</param>
    public StringMarshallingException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

}
