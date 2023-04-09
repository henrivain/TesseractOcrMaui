using System.Runtime.Serialization;

namespace MauiTesseractOcr.Exceptions;
/// <summary>
/// Thrown when tesseract result text cannot be encoded to another encoding.
/// </summary>
[Serializable]
public class InvalidBytesException : TesseractException
{
    /// <summary>
    /// New exception that is thrown when tesseract result, empty.
    /// </summary>
    public InvalidBytesException()
    {
    }

    /// <summary>
    /// New exception that is thrown when tesseract result, with message.
    /// </summary>
    /// <param name="message"></param>
    public InvalidBytesException(string? message) : base(message)
    {
    }

    /// <summary>
    /// New exception that is thrown when tesseract result, with message and inner exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public InvalidBytesException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// New exception that is thrown when tesseract result
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected InvalidBytesException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}