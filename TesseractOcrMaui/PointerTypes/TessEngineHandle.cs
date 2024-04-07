namespace TesseractOcrMaui.Iterables;

/// <summary>
/// Wrapper for <see cref="TessEngine.Handle"/> to strongly type <see cref="HandleRef"/>
/// </summary>
public readonly struct TessEngineHandle 
{
    internal TessEngineHandle(TessEngine engine) => Handle = engine.Handle;
    internal HandleRef Handle { get; }
    internal nint Ptr => Handle.Handle;

    /// <summary>
    /// Gets <see cref="TessEngineHandle"/> as <see cref="HandleRef"/>
    /// </summary>
    /// <param name="handle"></param>
    public static implicit operator HandleRef(TessEngineHandle handle) => handle.Handle;

    /// <summary>
    /// Gets <see cref="TessEngineHandle"/> as <see cref="IntPtr"/>
    /// </summary>
    /// <param name="handle"></param>
    public static implicit operator IntPtr(TessEngineHandle handle) => handle.Handle.Handle;
}
