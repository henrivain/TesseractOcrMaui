namespace MauiTesseractOcr;

/// <summary>
/// Base for IDisposable object.
/// </summary>
public abstract class DisposableObject : IDisposable
{
    /// <summary>
    /// Is object already disposed?
    /// </summary>
    public bool IsDisposed { get; private set; } = false;

    /// <summary>
    /// Event after object disposed.
    /// </summary>

    public event EventHandler<EventArgs>? Disposed;
    
    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        IsDisposed = true;
        GC.SuppressFinalize(this);
        Disposed?.Invoke(this, EventArgs.Empty);
    }

    
    /// <inheritdoc/>
    ~DisposableObject()
    {
        Dispose(false);
    }

    /// <summary>
    /// Check if object is disposed.
    /// </summary>
    /// <exception cref="ObjectDisposedException">If object is already disposed.</exception>
    protected virtual void VerifyNotDisposed()
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(ToString());
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <param name="disposing"></param>
    protected abstract void Dispose(bool disposing);
}
