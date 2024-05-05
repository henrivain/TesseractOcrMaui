#if !IOS
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Collections;
using System.Diagnostics;
using TesseractOcrMaui.Imaging;
using TesseractOcrMaui.ImportApis;
using TesseractOcrMaui.PointerTypes;
using TesseractOcrMaui.Results;

namespace TesseractOcrMaui.Iterables;

/// <summary>
/// Iterator to iterate over text layout. Implements <see cref="IEnumerator{SpanInfo}"/> and <see cref="IDisposable"/>.
/// </summary>
public class PageIterator : ParentDependantDisposableObject, IEnumerator<SpanLayout>
{
    
    SpanLayout? _current;


    /// <summary>
    /// New <see cref="PageIterator"/> to iterate over image text layout like bounding boxes and paragraph layout.
    /// Implements <see cref="IEnumerator{SpanInfo}"/> and <see cref="IDisposable"/>. 
    /// </summary>
    /// <param name="iterator">
    /// ResultIterator instance that is casted into PageIterator. 
    /// <paramref name="iterator"/> must exist as long as created <see cref="PageIterator"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">If <paramref name="iterator"/> is <see langword="null"/>.</exception>
    /// <exception cref="NullPointerException">If <paramref name="iterator"/>.Handle is <see cref="IntPtr.Zero"/></exception>
    internal PageIterator(ResultIterator iterator) : base(iterator)
    {
        /* Iterator should be the dependency object here, because it is also disposed  
           if TessEngine is disposed. Both iterators are using the same pointer,
           because PageIterator is casted from ResultIterator here */

        _creationType = ResultIteratorType.ResultIteratorBased;

        ArgumentNullException.ThrowIfNull(iterator);
        NullPointerException.ThrowIfNull(iterator.Handle);

        // C++ casts pointer to different iterator type, same address is returned
        IntPtr ptr = ResultIteratorApi.GetPageIterator(iterator.Handle);

        // This should never throw, same pointer as iterator.Handle is returned
        NullPointerException.ThrowIfNull(ptr);

        Handle = new HandleRef(this, ptr);
        Level = iterator.Level;
    }

    /// <summary>
    /// New <see cref="PageIterator"/> to iterate over image text layout like bounding boxes and paragraph layout.
    /// Implements <see cref="IEnumerator{SpanInfo}"/> and <see cref="IDisposable"/>. 
    /// </summary>
    /// <param name="engine">
    /// TessEngine that the <see cref="PageIterator"/> instance is depending on.
    /// <paramref name="engine"/> must exist as long as created <see cref="PageIterator"/>.
    /// </param>
    /// <param name="level">Text block size to be used.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="engine"/> is <see langword="null"/>.</exception>
    /// <exception cref="NullPointerException"> If <paramref name="engine"/>.Handle is <see cref="IntPtr.Zero"/>.</exception>
    /// <exception cref="PageIteratorException">If image does not contain text.</exception>
    /// <exception cref="ImageNotSetException">If <see cref="TessEngine.SetImage(Pix)"/> is not called before init.</exception>
    internal PageIterator(TessEngine engine, PageIteratorLevel level = PageIteratorLevel.TextLine) : base(engine) 
    {
        _creationType = ResultIteratorType.EngineBased;

        ArgumentNullException.ThrowIfNull(engine);
        NullPointerException.ThrowIfNull(engine.Handle);
        if (engine.IsImageSet is false) 
        {
            throw new ImageNotSetException($"TessEngine.SetImage() not called, cannot get iterator.");
        }

        IntPtr ptr = TesseractApi.AnalyseLayoutToPageIterator(engine.Handle);
        if (ptr == IntPtr.Zero)
        {
            throw new PageIteratorException($"Analyzed image page is empty.");
        }
        Handle = new HandleRef(this, ptr);
        Level = level;
    }

    /// <summary>
    /// [This ctor is for <see cref="Copy"/> and <see cref="CopyToCurrentIndex"/> only] <para/>
    /// Iterator to iterate over image text layout like bounding boxes and paragraph layout.
    /// Implements <see cref="IEnumerator{SpanInfo}"/> and <see cref="IDisposable"/>. 
    /// </summary>
    /// <param name="copiedPtr">New <see cref="PageIterator"/> pointer from copy.</param>
    /// <param name="level"><see cref="PageIteratorLevel"/> from old iterator.</param>
    /// <param name="isAtBeginning">Value from <see cref="IsAtBeginning"/> when copying.</param>
    /// <param name="dependency">TessEngine instance that iterator is depending on.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="dependency"/> is <see langword="null"/>.</exception>
    /// <exception cref="NullPointerException">If <paramref name="copiedPtr"/> is <see cref="IntPtr.Zero"/>.</exception>
    private PageIterator(
        IntPtr copiedPtr, 
        PageIteratorLevel level, 
        bool isAtBeginning, 
        DisposableObject dependency) : base(dependency)
    {
        _creationType = ResultIteratorType.Copied;
        NullPointerException.ThrowIfNull(copiedPtr);
        IsAtBeginning = isAtBeginning; 
        Handle = new HandleRef(this, copiedPtr);
        Level = level;
    }



    /// <summary>
    /// Handle to native PageIterator.
    /// </summary>
    public HandleRef Handle { get; private set; }

    /// <summary>
    /// Determine weather iterator is at index -1 or not.
    /// </summary>
    public bool IsAtBeginning { get; private set; } = true;

    /// <summary>
    /// <see cref="PageIteratorLevel"/> that determines block size to be read in one step
    /// like TextLine, Symbol or Paragraph.
    /// </summary>
    public PageIteratorLevel Level { get; set; }

    /// <summary>
    /// Gets the element in the collection at the current position of the enumerator.
    /// </summary>
    /// <returns>
    /// The element in the collection at the current position of the enumerator.
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">If <see cref="MoveNext"/> is not yet called and iterator is at index -1.</exception>
    /// <exception cref="ObjectDisposedException">If object is disposed.</exception>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public SpanLayout Current => _current ?? GetCurrent();

    /// <summary>
    /// Gets the element in the collection at the current position of the enumerator.
    /// </summary>
    /// <returns>
    /// The element in the collection at the current position of the enumerator.
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">If <see cref="MoveNext"/> is not yet called and iterator is at index -1.</exception>
    /// <exception cref="ObjectDisposedException">If object is disposed.</exception>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    object IEnumerator.Current => Current;

    /// <summary>
    /// Advances the enumerator to the next element of the collection.
    /// </summary>
    /// <returns>
    /// <see langword="true" /> if the enumerator was successfully advanced to the next element, 
    /// <see langword="false" /> if the enumerator has passed the end of the collection.
    /// </returns>
    /// <exception cref="ObjectDisposedException">If already disposed.</exception>
    public bool MoveNext()
    {
        ThrowIfDisposed();
        if (IsAtBeginning)
        {
            IsAtBeginning = false;
            return true;
        }

        _current = null;
        return PageIteratorApi.Next(Handle, Level);
    }

    /// <summary>
    /// Reset iterator back to start.
    /// </summary>
    /// <exception cref="ObjectDisposedException">If object already disposed.</exception>
    public void Reset()
    {
        ThrowIfDisposed();
        IsAtBeginning = true;
        _current = null;
        PageIteratorApi.Begin(Handle);
    }

    /// <summary>
    /// Copy current iterator instance with no state.
    /// </summary>
    /// <returns>New PageIterator instance.</returns>
    /// <exception cref="NullPointerException">If current instance cannot be copied and copied pointer is <see cref="IntPtr.Zero"/>.</exception>
    /// <exception cref="ObjectDisposedException">If object is already disposed.</exception>
    public PageIterator Copy()
    {
        PageIterator iter = CopyToCurrentIndex();
        iter.Reset();
        return iter;
    }

    /// <summary>
    /// Copy current Iterator instance in current state.
    /// </summary>
    /// <returns>New PageIterator with same state, but different memory address.</returns>
    /// <exception cref="NullPointerException">If current instance cannot be copied and copied pointer is <see cref="IntPtr.Zero"/>.</exception>
    /// <exception cref="ObjectDisposedException">If object is already disposed.</exception>
    public PageIterator CopyToCurrentIndex()
    {
        ThrowIfDisposed();
        IntPtr copied = PageIteratorApi.Copy(Handle);

        DisposableObject dependencyObject = _dependencyObject;
        if (dependencyObject is ResultIterator iter)
        {
            // Get TessEngine from iterator, because new iterator is created and
            // the same iterator pointer is nomore used
            dependencyObject = iter.GetDependencyObject();
        }

        // ArgumentNullException: _dependencyObject always not null -> cannot throw
        return new(copied, Level, IsAtBeginning, dependencyObject);
    }

    /// <summary>
    /// Check if iterator is at the first element on the given <paramref name="level"/> sized block.
    /// <para/><paramref name="level"/> should in most cases be bigger block size that <see cref="Level"/>.
    /// </summary>
    /// <param name="level"><see cref="PageIteratorLevel"/> that </param>
    /// <returns>
    /// <see langword="true"/> if iterator is at the start of current <paramref name="level"/> 
    /// sized block, otherwise <see langword="false"/>.
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">If <see cref="MoveNext"/> is not yet called and iterator is at index -1.</exception>
    /// <exception cref="ObjectDisposedException">If object is disposed.</exception>
    public bool IsAtBeginningOf(PageIteratorLevel level)
    {
        ThrowIfDisposed();
        ThrowIfAtBeginning();

        return PageIteratorApi.IsAtBeginningOf(Handle, level);
    }

    /// <summary>
    /// Check if iterator is at the last element on the given <paramref name="level"/> sized block.
    /// <para/><paramref name="level"/> should in most cases be bigger block size that <see cref="Level"/>.
    /// </summary>
    /// <param name="level"></param>
    /// <returns><see langword="true"/> if iterator is at the last index of the current <paramref name="level"/> 
    /// sized block, otherwise <see langword="false"/>.</returns>
    /// <exception cref="IndexOutOfRangeException">If <see cref="MoveNext"/> is not yet called and iterator is at index -1.</exception>
    /// <exception cref="ObjectDisposedException">If object is disposed.</exception>
    public bool IsAtFinalElement(PageIteratorLevel level)
    {
        ThrowIfDisposed();
        ThrowIfAtBeginning();

        return PageIteratorApi.IsAtFinalElement(Handle, level, Level);
    }

    /// <summary>
    /// Get current text block layout. Text block size is determined by <see cref="Level"/>.
    /// </summary>
    /// <returns><see cref="ParagraphInfo"/>, information about iterator's current text block layout.</returns>
    /// <exception cref="IndexOutOfRangeException">If <see cref="MoveNext"/> is not yet called and iterator is at index -1.</exception>
    /// <exception cref="ObjectDisposedException">If object is disposed.</exception>
    public ParagraphInfo GetCurrentParagraphInfo()
    {
        ThrowIfDisposed();
        ThrowIfAtBeginning();

        PageIteratorApi.ParagraphInfo(Handle, out ParagraphJustification justification,
            out bool isListItem, out bool isCrown, out int firstLineIndent);

        return new ParagraphInfo
        {
            Justification = justification,
            IsListItem = isListItem,
            IsCrown = isCrown,
            FirstLineIndent = firstLineIndent
        };
    }

    /// <summary>
    /// Get current text block coordinates. Text block size is determined by <see cref="Level"/>.
    /// </summary>
    /// <returns><see cref="BoundingBox"/>, recognized text block coordinates in iterators current state.</returns>
    /// <exception cref="IndexOutOfRangeException">If <see cref="MoveNext"/> is not yet called and iterator is at index -1.</exception>
    /// <exception cref="ObjectDisposedException">If object is disposed.</exception>
    public BoundingBox GetCurrentBoundingBox()
    {
        ThrowIfDisposed();
        ThrowIfAtBeginning();

        if (PageIteratorApi.BoundingBox(Handle, Level,
            out int left, out int top, out int right, out int bottom) is false)
        {
#if DEBUG
            throw new TesseractException("Bounding box not found");
#endif
        }

        return new(left, top, right, bottom);
    }

    /// <summary>
    /// Binarized image strip Pix from recognized text.
    /// </summary>
    /// <returns>Binarized image strip Pix from recognized text</returns>
    /// <exception cref="IndexOutOfRangeException">If <see cref="MoveNext"/> is not yet called and iterator is at index -1.</exception>
    /// <exception cref="ObjectDisposedException">If object is disposed.</exception>
    /// <exception cref="NullPointerException">If pix cannot be created with given parameters</exception>
    public Pix GetCurrentSpanAsBinaryPix()
    {
        ThrowIfDisposed();
        ThrowIfAtBeginning();

        IntPtr pixPtr = PageIteratorApi.GetBinaryImage(Handle, Level);
        NullPointerException.ThrowIfNull(pixPtr);
        return new(pixPtr);
    }

    /// <summary>
    /// Get current recognized text strip as Pix. 
    /// To get non-binarized image with, provide handle to original 
    /// Pix that was provided to initialize TessEngine.
    /// </summary>
    /// <param name="padding">Pixels added around the text, so text is not touching image border.</param>
    /// <param name="pixHandle">Handle to pix that recognized is worked on.</param>
    /// <param name="textStart">Text Top-Right start coordinates after padding is added.</param>
    /// <returns>
    /// If handle to original Pix was provided Non-binarized pix image strip that includes recognized text block,
    /// <para/>Othewise binarized pix image strip of recognized text block
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">If <see cref="MoveNext"/> is not yet called and iterator is at index -1.</exception>
    /// <exception cref="ObjectDisposedException">If object is disposed.</exception>
    /// <exception cref="NullPointerException">If pix cannot be created with given parameters</exception>
    public Pix GetCurrentSpanAsPix(int padding, PixHandle pixHandle, out Point2D textStart)
    {
        ThrowIfDisposed();
        ThrowIfAtBeginning();

        // Return binary image if pix Handle is not valid
        IntPtr pixPtr = PageIteratorApi.GetImage(Handle, Level, padding,
            pixHandle.Handle, out int left, out int top);
        textStart = new(left, top);

        NullPointerException.ThrowIfNull(pixPtr);
        return new(pixPtr);
    }



    /// <summary>
    /// Creation type that affects how disposing is done.
    /// </summary>
    private protected readonly ResultIteratorType _creationType;

    /// <summary>
    /// Get current text layout from text block that's size is determined by <see cref="Level"/>.
    /// </summary>
    /// <returns><see cref="SpanLayout"/>That contains text block layout information.</returns>
    /// <exception cref="IndexOutOfRangeException">If <see cref="MoveNext"/> is not yet called and iterator is at index -1.</exception>
    /// <exception cref="ObjectDisposedException">If object is disposed.</exception>
    private SpanLayout GetCurrent()
    {
        return new SpanLayout
        {
            Info = GetCurrentParagraphInfo(),
            Box = GetCurrentBoundingBox()
        };
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
            string message = (DidParentDispose, _creationType) switch
            {
                (true, ResultIteratorType.EngineBased) or
                (true, ResultIteratorType.Copied) =>
                    $"Cannot access a disposed object. {nameof(TessEngine)} that {nameof(PageIterator)} " +
                    $"is depends on was disposed which caused {nameof(PageIterator)} to also be disposed.",

                (true, ResultIteratorType.ResultIteratorBased) =>
                    $"Cannot access a disposed object. {nameof(ResultIterator)} that {nameof(PageIterator)} " +
                    $"was casted from was disposed which caused {nameof(PageIterator)} to also be disposed.",

                _ => "Cannot access a disposed object.",
            };
            throw new ObjectDisposedException(nameof(PageIterator), message);
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

    protected override void Dispose(bool disposing)
    {
        if (Handle.Handle != IntPtr.Zero)
        {
            switch (_creationType)
            {
                case ResultIteratorType.ResultIteratorBased:
                    /* Do not delete handles, ResultIterator
                    * with same handle might still exist.
                    * ResultIterator handles disposal of
                    * Native resources                      
                    */
                    break;

                case ResultIteratorType.Copied:
                case ResultIteratorType.EngineBased:
                    PageIteratorApi.Delete(Handle);
                    break;

                default:
                    throw new NotImplementedException($"{nameof(ResultIteratorType)} of {_creationType} not implemented.");
            }
            Handle = new HandleRef(this, IntPtr.Zero);
        }
    }
}

#endif