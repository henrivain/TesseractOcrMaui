#if !IOS

using TesseractOcrMaui.ImportApis;

namespace TesseractOcrMaui;
internal class ResultIterator : DisposableObject
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="engine"></param>
    /// <exception cref="NullPointerException">If engine handle is <see cref="IntPtr.Zero"/></exception>
    /// <exception cref="ArgumentNullException">If engine is null</exception>
    public ResultIterator(TessEngine engine)
    {
        ArgumentNullException.ThrowIfNull(engine);
        NullPointerException.ThrowIfNull(engine.Handle);

        IntPtr iterPtr = TesseractApi.GetResultIterator(engine.Handle);
        NullPointerException.ThrowIfNull(iterPtr);

        Handle = new(this, iterPtr);
    }

 

    public HandleRef Handle { get; private set; }




    


    

    public bool IsEngineParent(TessEngine engine) => engine.Handle.Handle == Handle.Handle;

    protected override void Dispose(bool disposing)
    {
        if (Handle.Handle != IntPtr.Zero) 
        {
            ResultIteratorApi.Delete(Handle);
            Handle = new HandleRef(this, IntPtr.Zero);
        }
    }
}

#endif