namespace TesseractOcrMAUILib;
public abstract class DisposableObject : IDisposable
{
    public bool IsDisposed { get; private set; } = false;

    public event EventHandler<EventArgs>? Disposed;


    public void Dispose()
    {
        Dispose(true);
        IsDisposed = true;
        GC.SuppressFinalize(this);
        Disposed?.Invoke(this, EventArgs.Empty);
    }

    ~DisposableObject()
    {
        Dispose(false);
    }

    protected virtual void VerifyNotDisposed()
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(ToString());
        }
    }

    protected abstract void Dispose(bool disposing);
}
