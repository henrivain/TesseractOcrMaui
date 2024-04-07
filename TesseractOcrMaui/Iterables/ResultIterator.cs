#if !IOS

using System.Collections;
using TesseractOcrMaui.ImportApis;
using TesseractOcrMaui.Results;

namespace TesseractOcrMaui.Iterables;

/// <summary>
/// Iterator to iterate over recognizion as different size pages like TextLines, Symbols or Paragraphs.
/// Notice that this class is <see cref="IDisposable"/>.
/// </summary>
public class ResultIterator : DisposableObject, IEnumerator<TextSpan>
{
    /// <summary>
    /// New Iterator to iterate over recognizion as different size pages like TextLines, Symbols or Paragraphs.
    /// Notice that this class is <see cref="IDisposable"/>.
    /// </summary>
    /// 
    /// <param name="engine">
    /// <see cref="TessEngine"/> that <see cref="ResultIterator"/> depends on. 
    /// <see cref="TessEngine"/> instance must exist as long as created <see cref="ResultIterator"/>
    /// </param>
    /// <param name="level">
    /// <see cref="PageIteratorLevel"/> that determines page size to be read like TextLine, Symbol or Paragraph.
    /// </param>
    /// 
    /// <exception cref="ArgumentNullException">If engine is null</exception>
    /// <exception cref="NullPointerException">If engine handle is <see cref="IntPtr.Zero"/></exception>
    /// <exception cref="ResultIteratorException">
    /// If native iterator cannot be initialized. Make sure <see cref="TessEngine.SetImage(Pix)"/> and 
    /// <see cref="TessEngine.Recognize(HandleRef?)"/> are called.
    /// </exception>
    public ResultIterator(TessEngine engine, PageIteratorLevel level = PageIteratorLevel.TextLine)
    {
        ArgumentNullException.ThrowIfNull(engine);
        NullPointerException.ThrowIfNull(engine.Handle);
        EngineHandle = engine.Handle;
        Level = level;

        // Object cannot be disposed at this point
        ResetPointer();
    }

    /// <summary>
    /// New Iterator to iterate over recognizion as different size pages like TextLines, Symbols or Paragraphs.
    /// Notice that this class is <see cref="IDisposable"/>.
    /// </summary>
    /// 
    /// <param name="handle">
    /// <see cref="TessEngine.Handle"/> that <see cref="ResultIterator"/> depends on. 
    /// <see cref="TessEngine"/> instance must exist as long as created <see cref="ResultIterator"/>
    /// </param>
    /// <param name="level">
    /// <see cref="PageIteratorLevel"/> that determines page size to be read like TextLine, Symbol or Paragraph.
    /// </param>
    /// 
    /// <exception cref="NullPointerException">If <paramref name="handle"/> is <see cref="IntPtr.Zero"/>.</exception>
    /// <exception cref="ResultIteratorException">
    /// If native iterator cannot be initialized. Make sure <see cref="TessEngine.SetImage(Pix)"/> and 
    /// <see cref="TessEngine.Recognize(HandleRef?)"/> are called.
    /// </exception>
    public ResultIterator(TessEngineHandle handle, PageIteratorLevel level = PageIteratorLevel.TextLine)
    {
        NullPointerException.ThrowIfNull(handle.Handle);
        EngineHandle = handle.Handle;
        Level = level;

        // Object cannot be disposed at this point
        ResetPointer();
    }


    /// <summary>
    /// Handle to native result iterator.
    /// </summary>
    public HandleRef Handle { get; private set; }

    /// <summary>
    /// <see cref="PageIteratorLevel"/> that determines page size to be read like TextLine, Symbol or Paragraph.
    /// </summary>
    public PageIteratorLevel Level { get; set; }

    /// <summary>
    /// Handle to <see cref="TessEngine"/> that current <see cref="ResultIterator"/> depends on.
    /// <see cref="TessEngine"/> must exist as long as current <see cref="ResultIterator"/>.
    /// </summary>
    private HandleRef EngineHandle { get; }

    /// <inheritdoc/>
    public TextSpan Current => GetCurrent();

    object IEnumerator.Current => Current;

    /// <summary>
    /// Because iterators start at index -1, 
    /// the state before calling <see cref="MoveNext"/> for the first time is stored here.
    /// </summary>
    public bool IsAtBeginning { get; private set; } = true;



    /// <summary>
    /// Advances the enumerator to the next element of the collection.
    /// </summary>
    /// <returns>
    /// <see langword="true" /> if the enumerator was successfully advanced to the next element, 
    /// <see langword="false" /> if the enumerator has passed the end of the collection.
    /// </returns>
    public bool MoveNext()
    {
        if (IsAtBeginning)
        {
            IsAtBeginning = false;
            return true;
        }
        return ResultIteratorApi.Next(Handle, Level);
    }


    /// <summary>
    /// Sets the enumerator to its initial position, which is before the first element in the collection.
    /// </summary>
    /// <exception cref="ObjectDisposedException">If current object is already disposed.</exception>
    /// <exception cref="ResultIteratorException">If new ResultIterator cannot be initialized.</exception>
    public void Reset() => ResetPointer();

    /// <summary>
    /// Check if <paramref name="engine"/> is same engine that <see cref="ResultIterator"/> instance depends on.
    /// </summary>
    /// <param name="engine"></param>
    /// <returns>
    /// True if <paramref name="engine"/> pointer matches <see cref="EngineHandle"/>, Otherwise false.</returns>
    public bool IsParentEngine(TessEngine? engine)
    {
        return engine?.Handle.Handle == Handle.Handle;
    }



    /// <summary>
    /// Gets the element in the collection at the current position of the enumerator.
    /// </summary>
    /// <param name="level"></param>
    /// <returns>The element in the collection at the current position of the enumerator.</returns>
    /// <exception cref="IndexOutOfRangeException">If <see cref="IsAtBeginning"/> is true, meaning the index is -1.</exception>
    public TextSpan GetCurrent(PageIteratorLevel? level = null)
    {
        if (IsAtBeginning)
        {
            throw new IndexOutOfRangeException($"Cannot access index -1, call {nameof(MoveNext)}() first.");
        }

        level ??= Level;

        IntPtr ptr = ResultIteratorApi.GetUTF8Text(Handle, level.Value);
        float confidence = ResultIteratorApi.GetConfidence(Handle, level.Value);

        return new TextSpan
        {
            Text = Marshal.PtrToStringUTF8(ptr) ?? string.Empty,
            Confidence = confidence,
            Level = Level,
        };

    }

    /// <summary>
    /// Delete old Handle and create new, so iteration can start from the beginning. 
    /// Does not dispose object.
    /// </summary>
    /// <exception cref="ObjectDisposedException">If current object is already disposed.</exception>
    /// <exception cref="ResultIteratorException">
    /// If new ResultIterator cannot be initialized. 
    /// Make sure TessEngine image is set and Recognize() is called.
    /// </exception>
    private void ResetPointer()
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
        Dispose(false);

        IntPtr iterPtr = TesseractApi.GetResultIterator(EngineHandle);
        if (iterPtr == IntPtr.Zero)
        {
            throw new ResultIteratorException("Cannot initialize new ResultIterator. " +
                "Make sure TessEngine image is set and Recognize() is called");
        }
        IsAtBeginning = true;
        Handle = new(this, iterPtr);
    }
    /// <inheritdoc/>
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