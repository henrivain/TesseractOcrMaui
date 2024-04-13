using TesseractOcrMaui.PointerTypes;

namespace TesseractOcrMaui.Iterables;

/// <summary>
/// Wrapper for <see cref="TessEngine.Handle"/> to strongly type <see cref="HandleRef"/>
/// </summary>
public readonly struct TessEngineHandle : IHandle
{
    /// <summary>
    /// New strongly typed <see cref="HandleRef"/> wrapper for <see cref="TessEngine.Handle"/>.
    /// </summary>
    /// <param name="engine"></param>
    /// <exception cref="ArgumentNullException">If <paramref name="engine"/> is <see langword="null"/>.</exception>
    internal TessEngineHandle(TessEngine? engine)
    {
        ArgumentNullException.ThrowIfNull(engine);
        Handle = engine.Handle;
    }

    /// <summary>
    /// Handle to referenced <see cref="TessEngine"/>.
    /// </summary>
    public HandleRef Handle { get; }
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

    /// <summary>
    /// Gets string representation of <see cref="TessEngineHandle"/>.
    /// </summary>
    /// <returns>string '<see cref="TessEngine"/> at [Hex Address]'. For example '<see cref="TessEngine"/> at 0x0000000'</returns>
    public override string ToString() => $"{nameof(TessEngine)} at {Ptr}";
}
