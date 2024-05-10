namespace TesseractOcrMaui.PointerTypes;

/// <summary>
/// Wrapper for <see cref="Pix.Handle"/> to strongly type <see cref="HandleRef"/>
/// </summary>
public readonly struct PixHandle : IHandle
{
    /// <summary>
    /// New wrapper for <see cref="Pix.Handle"/> to strongly type <see cref="HandleRef"/>
    /// </summary>
    /// <param name="pix"></param>
    /// <exception cref="ArgumentNullException">If Pix is null</exception>
    internal PixHandle(Pix? pix)
    {
        ArgumentNullException.ThrowIfNull(pix);
        Handle = pix.Handle;
    }

    /// <summary>
    /// Handle to <see cref="Pix"/>.
    /// </summary>
    public HandleRef Handle { get; }

    internal nint Ptr => Handle.Handle;

    /// <summary>
    /// Gets <see cref="PixHandle"/> as <see cref="HandleRef"/>
    /// </summary>
    /// <param name="handle"></param>
    public static implicit operator HandleRef(PixHandle handle) => handle.Handle;

    /// <summary>
    /// Gets <see cref="PixHandle"/> as <see cref="IntPtr"/>
    /// </summary>
    /// <param name="handle"></param>
    public static implicit operator IntPtr(PixHandle handle) => handle.Handle.Handle;

    /// <summary>
    /// Gets string representation of <see cref="PixHandle"/>.
    /// </summary>
    /// <returns>string '<see cref="Pix"/> at [Hex Address]'. For example '<see cref="Pix"/> at 0x0000000'</returns>
    public override string ToString() => $"{nameof(Pix)} at {Ptr}";
}
