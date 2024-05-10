namespace TesseractOcrMaui.Exceptions;

/// <summary>
/// Exception Thrown when image should be set but isn't.
/// </summary>
public class ImageNotSetException : TesseractException
{
    /// <summary>
    /// New <see cref="ImageNotSetException"/>.
    /// Thrown when image should be set but isn't.
    /// </summary>
    public ImageNotSetException() : this(null) { }

    /// <summary>
    /// New <see cref="ImageNotSetException"/> with message.
    /// Thrown when image should be set but isn't.
    /// </summary>
    /// <param name="message"></param>
    public ImageNotSetException(string? message) : this(message, null)
    {
    }

    /// <summary>
    /// New <see cref="ImageNotSetException"/> with message and inner exception.
    /// Thrown when image should be set but isn't.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public ImageNotSetException(string? message, Exception? inner) 
        : base(message ?? "Image value not set.", inner) 
    { 
    }
}
