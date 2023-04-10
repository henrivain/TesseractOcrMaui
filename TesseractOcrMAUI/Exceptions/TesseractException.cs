using System.Runtime.Serialization;

namespace TesseractOcrMaui.Exceptions;
/// <summary>
/// Exception thrown when Tesseract cannot complete given task.
/// </summary>
[Serializable]
public class TesseractException : Exception
{
    /// <summary>
    /// New empty TesseractException.
    /// </summary>
    public TesseractException() { }

    /// <summary>
    /// New TesseractException with message about error reason.
    /// </summary>
    /// <param name="message">Error reason.</param>
    public TesseractException(string? message) : base(message) { }

    /// <summary>
    /// New TesseractException with message and inner exception.
    /// </summary>
    /// <param name="message">Error reason.</param>
    /// <param name="innerException">Exception that caused this error.</param>
    public TesseractException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected TesseractException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}