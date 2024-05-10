namespace TesseractOcrMaui.Exceptions;

/// <summary>
/// Exception thrown by ResultIterator.
/// </summary>
public class ResultIteratorException : TesseractInitException
{
    /// <summary>
    /// New Exception thrown by ResultIterator.
    /// </summary>
    public ResultIteratorException() : this(null)
    {
    }

    /// <summary>
    /// New Exception thrown by ResultIterator.
    /// </summary>
    /// <param name="message"></param>
    public ResultIteratorException(string? message) : this(message, null)
    {
    }

    /// <summary>
    /// New Exception thrown by ResultIterator.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public ResultIteratorException(string? message, Exception? inner) 
        : base(message ?? "Result iteration failed.", inner) 
    { 
    }
}
