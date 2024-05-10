using System.Collections;
using TesseractOcrMaui.Imaging;
using TesseractOcrMaui.Results;
using static TesseractOcrMaui.Iterables.SyncIterator;

namespace TesseractOcrMaui.Iterables;
internal sealed class SyncIterator : DisposableObject, IEnumerator<SyncedIterators>, IEnumerator
{
    readonly ResultIterator _resultIterator;
    readonly PageIterator _pageIterator;

    /// <exception cref="NullPointerException">If engine Handle Intptr.Zero.</exception>
    /// <exception cref="ArgumentNullException">If engine null.</exception>
    /// <exception cref="TesseractInitException">Engine image not set or recognized.</exception>
    /// <exception cref="ResultIteratorException">Native asset null, make bug report with used data if thrown.</exception>
    /// <exception cref="ObjectDisposedException">If engine disposed durint iteration.</exception>
    internal SyncIterator(TessEngine engine, PageIteratorLevel level = PageIteratorLevel.TextLine)
        : this(new ResultIterator(engine, level))
    {
    }

    /// <exception cref="ObjectDisposedException">If engine disposed durint iteration.</exception>
    /// <exception cref="NullPointerException">If iter Handle is IntPtr.Zero.</exception>
    private SyncIterator(ResultIterator iter)
    {
        // Tracks and disposes both iterators.
        _resultIterator = iter;
        _pageIterator = iter.AsPageIterator();
        if (_resultIterator.IsAtBeginning is false)
        {
            // Skip C# state
            _pageIterator.MoveNext();
        }
    }



    public PageIteratorLevel Level => _resultIterator.Level;
    public bool IsAtBeginning => _resultIterator.IsAtBeginning;
    public SyncedIterators Current => new(_resultIterator, _pageIterator);
    object IEnumerator.Current => Current;

    public void SetIteratorLevel(PageIteratorLevel level)
    {
        _resultIterator.Level = level;
        _pageIterator.Level = level;
    }

    /// <exception cref="ObjectDisposedException"></exception>
    public bool MoveNext()
    {
        if (_pageIterator.IsAtBeginning)
        {
            _pageIterator.MoveNext();
        }
        return _resultIterator.MoveNext();
    }

    /// <exception cref="IndexOutOfRangeException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public SpanLayout GetSpanLayout() 
        => _pageIterator.Current;

    /// <exception cref="IndexOutOfRangeException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public TextSpan GetTextSpan() 
        => _resultIterator.Current;

    public string? GetRecognizedLanguage() 
        => _resultIterator.GetCurrentRecognizedLanguage();

    /// <exception cref="IndexOutOfRangeException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public bool IsAtBeginningOf(PageIteratorLevel level) 
        => _pageIterator.IsAtBeginningOf(level);

    /// <exception cref="IndexOutOfRangeException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public bool IsAtFinalElement(PageIteratorLevel level) 
        => _pageIterator.IsAtFinalElement(level);
    
    /// <exception cref="IndexOutOfRangeException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public ParagraphInfo GetParagraphInfo() 
        => _pageIterator.GetCurrentParagraphInfo();
    
    /// <exception cref="IndexOutOfRangeException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="PageIteratorException"></exception>
    public BoundingBox GetBoundingBox() 
        => _pageIterator.GetCurrentBoundingBox();




    /// <exception cref="NullPointerException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="TesseractInitException"></exception>
    public SyncIterator CopyAtCurrentIndex(PageIteratorLevel? level = null)
    {
        ResultIterator copied = _resultIterator.CopyToCurrentIndex();
        SyncIterator synced = new(copied);
        if (level is not null)
        {
            synced.SetIteratorLevel(level.Value);
        }
        return synced;
    }



    /// <summary>
    /// Not supported.
    /// </summary>
    /// <exception cref="NotSupportedException"></exception>
    public void Reset()
    {
        throw new NotSupportedException("Resetting not supported.");
    }

    protected override void Dispose(bool disposing)
    {
        _pageIterator.Dispose();
        _resultIterator.Dispose();
    }

    internal readonly record struct SyncedIterators(ResultIterator First, PageIterator Second);
}

