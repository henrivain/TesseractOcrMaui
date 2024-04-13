#if !IOS
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Collections;
using TesseractOcrMaui.ImportApis;

namespace TesseractOcrMaui.Iterables;


public class PageIterator : DisposableObject
{
    public PageIterator(ResultIterator iterator)
    {
        NullPointerException.ThrowIfNull(iterator.Handle);

        IntPtr ptr = ResultIteratorApi.GetPageIterator(iterator.Handle);

        NullPointerException.ThrowIfNull(ptr);
        Handle = new(this, ptr);
        Level = iterator.Level;
    }

    public PageIteratorLevel Level { get; set; }

    public HandleRef Handle { get; private set; }


    public Pix GetCurrent()
    {
        IntPtr pixPtr = PageIteratorApi.GetBinaryImage(Handle, Level);
        return new Pix(pixPtr);
    }


    public bool MoveNext() => PageIteratorApi.Next(Handle, Level);
    public void Reset() => PageIteratorApi.Begin(Handle);

    protected override void Dispose(bool disposing)
    {
        if (Handle.Handle != IntPtr.Zero)
        {
            PageIteratorApi.Delete(Handle);
        }
    }


}

#endif