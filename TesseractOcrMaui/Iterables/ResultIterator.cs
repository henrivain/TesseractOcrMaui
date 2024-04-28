#if !IOS

using System.Collections;
using System.Diagnostics;
using System.Net.Http.Headers;
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
        : this(new TessEngineHandle(engine), level) { }

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
        EngineHandle = handle;
        Level = level;

        IntPtr iterPtr = TesseractApi.GetResultIterator(EngineHandle);
        if (iterPtr == IntPtr.Zero)
        {
            throw new ResultIteratorException("Cannot initialize new ResultIterator. " +
                "Make sure TessEngine image is set and Recognize() is called");
        }
        IsAtBeginning = true;
        Handle = new(this, iterPtr);
    }

    /// <summary>
    /// [This ctor should only be used with copied <see cref="ResultIterator"/>] <br/>
    /// New Iterator to iterate over recognizion as different size pages like TextLines, Symbols or Paragraphs.
    /// Notice that this class is <see cref="IDisposable"/>.
    /// </summary>
    /// <param name="iteratorPtr">Pointer to copied <see cref="ResultIterator"/> referrence.</param>
    /// <param name="engineHandle">
    /// <see cref="TessEngine.Handle"/> that <see cref="ResultIterator"/> depends on. 
    /// <see cref="TessEngine"/> instance must exist as long as created <see cref="ResultIterator"/>
    /// </param>
    /// <param name="level">
    /// <see cref="PageIteratorLevel"/> that determines page size to be read like TextLine, Symbol or Paragraph.
    /// </param>
    /// <param name="isAtBeginning">Stores value that is used to determine weather copied iterator is at -1 index.</param>
    /// <exception cref="NullPointerException">
    /// If <paramref name="iteratorPtr"/> is <see cref="IntPtr.Zero"/> or 
    /// <paramref name="engineHandle"/>.Handle is <see cref="IntPtr.Zero"/>
    /// </exception>
    private protected ResultIterator(
        IntPtr iteratorPtr, TessEngineHandle engineHandle,
        PageIteratorLevel level, bool isAtBeginning)
    {
        // This ctor is mainly for ResultIterator copy action
        NullPointerException.ThrowIfNull(iteratorPtr);
        NullPointerException.ThrowIfNull(engineHandle.Handle);

        EngineHandle = engineHandle;
        Level = level;
        Handle = new HandleRef(this, iteratorPtr);
        IsAtBeginning = isAtBeginning;
    }

    /// <summary>
    ///  Evaluated with <see cref="GetCurrent"/>. <see cref="MoveNext()"/> resets to <see langword="null"/>.
    /// </summary>
    TextSpan? _current;

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
    internal TessEngineHandle EngineHandle { get; }

    /// <inheritdoc/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TextSpan Current => _current ?? GetCurrent();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    object IEnumerator.Current => Current;

    /// <summary>
    /// Because iterators start at index -1, 
    /// the state before calling <see cref="MoveNext()"/> for the first time is stored here.
    /// </summary>
    public bool IsAtBeginning { get; private set; } = true;

    /// <summary>
    /// Advances the enumerator to the next element of the collection.
    /// </summary>
    /// <returns>
    /// <see langword="true" /> if the enumerator was successfully advanced to the next element, 
    /// <see langword="false" /> if the enumerator has passed the end of the collection.
    /// </returns>
    public bool MoveNext() => MoveNext(Level);

    /// <summary>
    /// Advances the enumerator to the next element of the collection by given <see cref="PageIteratorLevel"/> <paramref name="level"/>.
    /// </summary>
    /// <param name="level"></param>
    /// <returns>
    /// <see langword="true" /> if the enumerator was successfully advanced to the next element, 
    /// <see langword="false" /> if the enumerator has passed the end of the collection.
    /// </returns>
    public bool MoveNext(PageIteratorLevel level)
    {
        _current = null;
        if (IsAtBeginning)
        {
            IsAtBeginning = false;
            return true;
        }
        return ResultIteratorApi.Next(Handle, level);
    }


    /// <summary>
    /// Not Supported. Throws <see cref="NotSupportedException"/>.
    /// </summary>
    /// <exception cref="NotSupportedException">Throws always, reset not supported</exception>
    public void Reset()
    {
        throw new NotSupportedException($"{nameof(Reset)} is not supported for {nameof(ResultIterator)}.");
    }

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
    /// Get language (Tessadata file name) in current iterator position.
    /// </summary>
    /// <returns>Used tessData file name without extension or <see langword="null"/> if failed.</returns>
    public string? GetCurrentRecognizedLanguage()
    {
        IntPtr langPtr = ResultIteratorApi.GetRecognizedLanguage(Handle);
        return Marshal.PtrToStringUTF8(langPtr);
    }

    /// <summary>
    /// Get copy of <see cref="ResultIterator"/> at current index.
    /// </summary>
    /// <returns>Copy of <see cref="ResultIterator"/> at current index if successful, otherwise false.</returns>
    /// <exception cref="NullPointerException">If <see cref="EngineHandle"/> is <see cref="IntPtr.Zero"/>.</exception>
    public ResultIterator? CopyToCurrentIndex()
    {
        IntPtr newIteratorPtr = ResultIteratorApi.Copy(Handle);
        if (newIteratorPtr == IntPtr.Zero)
        {
            return null;
        }
        return new(newIteratorPtr, EngineHandle, Level, IsAtBeginning);
    }

    /// <summary>
    /// Get Current ResultIterator as PageIterator. 
    /// Same native object reference is used.
    /// </summary>
    /// <returns><see cref="PageIterator"/> with the same native reference as current <see cref="ResultIterator"/>.</returns>
    public PageIterator AsPageIterator()
    {
        // TODO: exceptions list
        return new PageIterator(this);
    }




    /// <summary>
    /// Gets the element in the collection at the current position of the enumerator.
    /// </summary>
    /// <returns>The element in the collection at the current position of the enumerator.</returns>
    /// <exception cref="IndexOutOfRangeException">If <see cref="IsAtBeginning"/> is <see langword="true"/> meaning the index is -1.</exception>
    private TextSpan GetCurrent()
    {
        if (IsAtBeginning)
        {
            throw new IndexOutOfRangeException($"Cannot access index -1, call {nameof(MoveNext)}() first.");
        }

        IntPtr ptr = ResultIteratorApi.GetUTF8Text(Handle, Level);
        float confidence = ResultIteratorApi.GetConfidence(Handle, Level);
        string resultText = Marshal.PtrToStringUTF8(ptr) ?? string.Empty;

        TesseractApi.DeleteString(ptr);

        _current = new(resultText, confidence, Level);
        return _current.Value;
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