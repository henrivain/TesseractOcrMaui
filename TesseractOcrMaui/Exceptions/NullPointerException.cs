using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;


namespace TesseractOcrMaui.Exceptions;
/// <summary>
/// Exception thrown when pointer turns out to be IntPtr.Zero when it shouldnt't.
/// </summary>
public class NullPointerException : TesseractException
{
    /// <summary>
    /// New <see cref="NullPointerException"/> which is thrown when pointer turns out 
    /// to be <see cref="IntPtr.Zero"/> when it shouldn't.
    /// </summary>
    public NullPointerException() : this(null) { }

    /// <summary>
    /// New <see cref="NullPointerException"/> with message.
    /// <see cref="NullPointerException"/> is thrown when pointer turns out 
    /// to be <see cref="IntPtr.Zero"/> when it shouldn't.
    /// </summary>
    /// <param name="message"></param>
    public NullPointerException(string? message) : this(message, null) { }

    /// <summary>
    /// New <see cref="NullPointerException"/> with message and inner exception.
    /// <see cref="NullPointerException"/> is thrown when pointer turns out 
    /// to be  when it shouldn't.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public NullPointerException(string? message, Exception? innerException) : base(message, innerException) { }

    /// <summary>
    /// Throw <see cref="NullPointerException"/> if <paramref name="pointer"/> has value of <see cref="IntPtr.Zero"/>
    /// </summary>
    /// <param name="pointer">Pointer to be validated</param>
    /// <param name="pointerName">
    /// Parameter name, in most of the cases
    /// this value is set automatically by compiler and should not be touched</param>
    [StackTraceHidden]
    public static void ThrowIfNull(IntPtr pointer, [CallerArgumentExpression(nameof(pointer))] string? pointerName = null)
    {
        pointerName ??= "Arg_Param_Name";

        if (pointer == IntPtr.Zero)
        {
            throw new NullPointerException($"Value cannot be IntPtr.Zero. {pointerName}");
        }
    }


    /// <summary>
    /// Throw <see cref="NullPointerException"/> if <paramref name="handle"/>.Handle has value of <see cref="IntPtr.Zero"/>
    /// </summary>
    /// <param name="handle"></param>
    /// <param name="pointerName"></param>
    [StackTraceHidden]
    public static void ThrowIfNull(HandleRef handle, [CallerArgumentExpression(nameof(handle))] string? pointerName = null)
    {
        pointerName ??= "Arg_Param_Name";

        if (handle.Handle == IntPtr.Zero)
        {
            throw new NullPointerException($"Handle value cannot be IntPtr.Zero. {pointerName}");
        }
    }


}
