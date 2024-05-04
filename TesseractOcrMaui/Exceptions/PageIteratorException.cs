using TesseractOcrMaui.Iterables;

namespace TesseractOcrMaui.Exceptions;

/// <summary>
/// Thrown by <see cref="PageIterator"/>.
/// </summary>
public class PageIteratorException : TesseractInitException
{
    /// <summary>
    /// New Exception thrown by <see cref="PageIterator"/>.
    /// </summary>
    public PageIteratorException() : this(null)
    {
    }

    /// <summary>
    /// New Exception thrown by <see cref="PageIterator"/>.
    /// </summary>
    /// <param name="message"></param>
    public PageIteratorException(string? message) : this(message, null)
    {
    }

    /// <summary>
    /// New Exception thrown by <see cref="PageIterator"/>.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public PageIteratorException(string? message, Exception? inner)
        : base(message ?? "Page iteration failed.", inner)
    {
    }
}
