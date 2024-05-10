
namespace TesseractOcrMaui.Exceptions;

/// <summary>
/// Thrown by PageIterator.
/// </summary>
public class PageIteratorException : TesseractInitException
{
    /// <summary>
    /// New Exception thrown by PageIterator.
    /// </summary>
    public PageIteratorException() : this(null)
    {
    }

    /// <summary>
    /// New Exception thrown by PageIterator.
    /// </summary>
    /// <param name="message"></param>
    public PageIteratorException(string? message) : this(message, null)
    {
    }

    /// <summary>
    /// New Exception thrown by PageIterator.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public PageIteratorException(string? message, Exception? inner)
        : base(message ?? "Page iteration failed.", inner)
    {
    }
}
