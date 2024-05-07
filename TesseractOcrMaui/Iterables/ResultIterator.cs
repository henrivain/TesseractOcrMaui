#if !IOS

using System.Collections;
using System.Diagnostics;
using TesseractOcrMaui.ImportApis;
using TesseractOcrMaui.Results;

namespace TesseractOcrMaui.Iterables;

/// <summary>
/// Iterator to iterate over recognizion as different size pages like TextLines, Symbols or Paragraphs.
/// Notice that this class is <see cref="IDisposable"/>.
/// </summary>
public class ResultIterator : ParentDependantDisposableObject, IEnumerator<TextSpan>
{
    TextSpan? _current;


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
    /// <exception cref="NullPointerException">If <paramref name="engine"/>.Handle is <see cref="IntPtr.Zero"/>.</exception>
    /// <exception cref="TesseractInitException">Engine image not set or recognized.</exception>
    /// <exception cref="ResultIteratorException">Native asset is null, consider making bug report if you encouter.</exception>
    internal ResultIterator(TessEngine engine, PageIteratorLevel level = PageIteratorLevel.TextLine) : base(engine)
    {
        ArgumentNullException.ThrowIfNull(engine);
        NullPointerException.ThrowIfNull(engine.Handle);

        
        if (engine.IsImageSet is false)
        {
            throw new TesseractInitException($"TessEngine.SetImage() not called, cannot get iterator.");
        }
        if (engine.IsRecognized is false)
        {
            throw new TesseractInitException($"TessEngine.Recognize() not called, cannot get iterator");
        }

        EngineHandle = new TessEngineHandle(engine);
        IntPtr iterPtr = TesseractApi.GetResultIterator(EngineHandle);

        if (iterPtr == IntPtr.Zero)
        {
            throw new ResultIteratorException("Cannot create native iterator.");
        }
        IsAtBeginning = true;
        Level = level;
        Handle = new(this, iterPtr);
    }

    /// <summary>
    /// [This ctor is only for <see cref="CopyToCurrentIndex"/>] <para/>
    /// New Iterator to iterate over recognizion as different size pages like TextLines, Symbols or Paragraphs.
    /// Notice that this class is <see cref="IDisposable"/>.
    /// </summary>
    /// <param name="newIterator">Pointer to copied <see cref="ResultIterator"/> referrence.</param>
    /// <param name="engineHandle">
    /// <see cref="TessEngine.Handle"/> that <see cref="ResultIterator"/> depends on. 
    /// <see cref="TessEngine"/> instance must exist as long as created <see cref="ResultIterator"/>
    /// </param>
    /// <param name="level">
    /// <see cref="PageIteratorLevel"/> that determines page size to be read like TextLine, Symbol or Paragraph.
    /// </param>
    /// <param name="isAtBeginning">Stores value that is used to determine weather copied iterator is at -1 index.</param>
    /// <param name="dependency">
    /// <see cref="TessEngine"/> that <see cref="ResultIterator"/> depends on. 
    /// <see cref="TessEngine"/> instance must exist as long as created <see cref="ResultIterator"/>
    /// </param>
    /// <exception cref="NullPointerException">
    /// If <paramref name="newIterator"/> or <paramref name="engineHandle"/>.Handle is <see cref="IntPtr.Zero"/> 
    /// </exception>
    private protected ResultIterator(IntPtr newIterator, TessEngineHandle engineHandle, 
        PageIteratorLevel level, bool isAtBeginning, TessEngine dependency) : base(dependency)
    {
        // This ctor is for ResultIterator copy action
        NullPointerException.ThrowIfNull(newIterator);
        NullPointerException.ThrowIfNull(engineHandle.Handle);

        EngineHandle = engineHandle;
        Level = level;
        Handle = new HandleRef(this, newIterator);
        IsAtBeginning = isAtBeginning;
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
    /// Because iterators start at index -1, 
    /// the state before calling <see cref="MoveNext()"/> for the first time is stored here.
    /// </summary>
    public bool IsAtBeginning { get; private set; } = true;

    /// <summary>
    /// Gets the element in the collection at the current position of the enumerator.
    /// </summary>
    /// <returns>
    /// The element in the collection at the current position of the enumerator.
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">If <see cref="MoveNext()"/> is not yet called and iterator is at index -1.</exception>
    /// <exception cref="ObjectDisposedException">If object is already disposed.</exception>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TextSpan Current => _current ?? GetCurrent();

    /// <summary>
    /// Gets the element in the collection at the current position of the enumerator.
    /// </summary>
    /// <returns>
    /// The element in the collection at the current position of the enumerator.
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">If <see cref="MoveNext()"/> is not yet called and iterator is at index -1.</exception>
    /// <exception cref="ObjectDisposedException">If object already is disposed.</exception>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    object IEnumerator.Current => Current;

    /// <summary>
    /// Handle to <see cref="TessEngine"/> that current <see cref="ResultIterator"/> depends on.
    /// <see cref="TessEngine"/> must exist as long as current <see cref="ResultIterator"/>.
    /// </summary>
    internal TessEngineHandle EngineHandle { get; }




    /// <summary>
    /// Advances the enumerator to the next element of the collection.
    /// </summary>
    /// <returns>
    /// <see langword="true" /> if the enumerator was successfully advanced to the next element, 
    /// <see langword="false" /> if the enumerator has passed the end of the collection.
    /// </returns>
    /// <exception cref="ObjectDisposedException">If already disposed.</exception>
    public bool MoveNext() => MoveNext(Level);

    /// <summary>
    /// Advances the enumerator to the next element of the collection by given <see cref="PageIteratorLevel"/> <paramref name="level"/>.
    /// </summary>
    /// <param name="level"></param>
    /// <returns>
    /// <see langword="true" /> if the enumerator was successfully advanced to the next element, 
    /// <see langword="false" /> if the enumerator has passed the end of the collection.
    /// </returns>
    /// <exception cref="ObjectDisposedException">If already disposed.</exception>
    public bool MoveNext(PageIteratorLevel level)
    {
        ThrowIfDisposed();

        _current = null;
        if (IsAtBeginning)
        {
            IsAtBeginning = false;
            return true;
        }
        return ResultIteratorApi.Next(Handle, level);
    }

    /// <summary>
    /// Get language (Tessadata file name) in current iterator position.
    /// </summary>
    /// <returns>Used tessData file name without extension or <see langword="null"/> if cannot be retrieved.</returns>
    public string? GetCurrentRecognizedLanguage()
    {
        ThrowIfDisposed();
        ThrowIfAtBeginning();

        IntPtr langPtr = ResultIteratorApi.GetRecognizedLanguage(Handle);
        return Marshal.PtrToStringUTF8(langPtr);
    }




    /// <summary>
    /// Get copy of <see cref="ResultIterator"/> at current index.
    /// </summary>
    /// <returns>Copy of <see cref="ResultIterator"/> at current index.</returns>
    /// <exception cref="NullPointerException">If <see cref="EngineHandle"/> or <see cref="Handle"/> is <see cref="IntPtr.Zero"/>.</exception>
    /// <exception cref="ObjectDisposedException">If object already disposed.</exception>
    /// <exception cref="TesseractInitException">If native copying failed.</exception>
    public ResultIterator CopyToCurrentIndex()
    {
        ThrowIfDisposed();
        NullPointerException.ThrowIfNull(Handle);

        IntPtr newIteratorPtr = ResultIteratorApi.Copy(Handle);
        if (newIteratorPtr == IntPtr.Zero)
        {
            throw new TesseractInitException("Copying ResultIterator failed.");
        }
        if (_dependencyObject is not TessEngine)
        {
            throw new InvalidOperationException($"Dependency object should always be of type {nameof(TessEngine)}. " +
                $"Consider making a bug report");
        }
        return new(newIteratorPtr, EngineHandle, Level, IsAtBeginning, (TessEngine)_dependencyObject);
    }

    /// <summary>
    /// Get Current ResultIterator as PageIterator. 
    /// Same native object reference is used.
    /// </summary>
    /// <returns><see cref="PageIterator"/> with the same native reference as current <see cref="ResultIterator"/>.</returns>
    /// <exception cref="NullPointerException">If <see cref="Handle"/> is <see cref="IntPtr.Zero"/></exception>
    public PageIterator AsPageIterator()
    {
        ThrowIfDisposed();
        return new PageIterator(this);
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
    public bool IsDependedEngine(TessEngine? engine)
    {
        if (engine is null)
        {
            return false;
        }
        return engine?.Handle.Handle == Handle.Handle;
    }




    /// <summary>
    /// Gets the element in the collection at the current position of the enumerator.
    /// </summary>
    /// <returns>The element in the collection at the current position of the enumerator.</returns>
    /// <exception cref="IndexOutOfRangeException">If <see cref="IsAtBeginning"/> is <see langword="true"/> meaning the index is -1.</exception>
    /// <exception cref="ObjectDisposedException">Object is already disposed.</exception>
    private TextSpan GetCurrent()
    {
        ThrowIfDisposed();
        ThrowIfAtBeginning();

        IntPtr ptr = ResultIteratorApi.GetUTF8Text(Handle, Level);
        float confidence = ResultIteratorApi.GetConfidence(Handle, Level);
        string resultText = Marshal.PtrToStringUTF8(ptr) ?? string.Empty;

        TesseractApi.DeleteString(ptr);

        _current = new(resultText, confidence, Level);
        return _current.Value;
    }
    
    /// <summary>
    /// Throws <see cref="ObjectDisposedException"/> if object is disposed, otherwise does nothing.
    /// </summary>
    /// <exception cref="ObjectDisposedException">If object is disposed.</exception>
    [StackTraceHidden]
    private void ThrowIfDisposed()
    {
        if (IsDisposed)
        {

            string message = DidParentDispose ? 
                $"Cannot access a disposed object. {nameof(TessEngine)} that {nameof(ResultIterator)} " +
                $"is depending on was disposed which caused {nameof(ResultIterator)} to also be disposed." 
                : "Cannot access a disposed object.";
            throw new ObjectDisposedException(nameof(ResultIterator), message);
        }
    }

    /// <summary>
    /// Throws <see cref="IndexOutOfRangeException"/> if <see cref="IsAtBeginning"/> 
    /// is <see langword="true"/>, otherwise does nothing.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">if <see cref="IsAtBeginning"/> is <see langword="true"/>.</exception>
    [StackTraceHidden]
    private void ThrowIfAtBeginning()
    {
        if (IsAtBeginning)
        {
            throw new IndexOutOfRangeException($"Cannot access index -1, call {nameof(MoveNext)}() first.");
        }
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