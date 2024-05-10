namespace TesseractOcrMaui.Exceptions;
/// <summary>
/// Exception thrown when Tesseract cannot be initialized.
/// </summary>
public class TesseractInitException : TesseractException
{
    /// <summary>
    /// New exception thrown when Tesseract cannot be initialized
    /// </summary>
    public TesseractInitException() : this(null) { }

    /// <summary>
    /// New exception with message, thrown when Tesseract cannot be initialized
    /// </summary>
    /// <param name="message"></param>
    public TesseractInitException(string? message) : this(message, null) { }

    /// <summary>
    /// New exception with message and inner exception, thrown when Tesseract cannot be initialized
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public TesseractInitException(string? message, Exception? innerException) 
        : base(message ?? "Initialization process failed.", innerException)
    {
    }
}
