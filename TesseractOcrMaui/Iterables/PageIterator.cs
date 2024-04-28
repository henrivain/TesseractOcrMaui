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
public class PageIterator : DisposableObject, IEnumerator<SpanInfo>
{
    internal PageIterator(ResultIterator iterator)
    {
        CreationType = ResultIteratorType.ResultIteratorBased;

        NullPointerException.ThrowIfNull(iterator.Handle);
        IntPtr ptr = ResultIteratorApi.GetPageIterator(iterator.Handle);

        // This should never throw,
        // same ptr is returned in ResultIteratorApi.GetPageIterator(handle)
        NullPointerException.ThrowIfNull(ptr);

        Handle = new HandleRef(this, ptr);
        Level = iterator.Level;
        iterator.Disposed += (_, _) => Dispose();
    }
    private PageIterator(IntPtr copiedPtr, PageIteratorLevel level)
    {
        CreationType = ResultIteratorType.Copied;
        NullPointerException.ThrowIfNull(copiedPtr);
        Handle = new HandleRef(this, copiedPtr);
        Level = level;
    }

    SpanInfo? _current;

    public HandleRef Handle { get; private set; }
    public bool IsAtBeginning { get; private set; } = true;
    public PageIteratorLevel Level { get; set; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public SpanInfo Current => _current ?? GetCurrent();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    object IEnumerator.Current => Current;


    public bool IsAtBeginningOf(PageIteratorLevel level)
    {
        return PageIteratorApi.IsAtBeginningOf(Handle, level);
    }
    public bool IsAtFinalElement(PageIteratorLevel level)
    {
        return PageIteratorApi.IsAtFinalElement(Handle, level);
    }
    public ParagraphInfo GetCurrentParagraphInfo()
    {
        if (IsAtBeginning)
        {
            throw new IndexOutOfRangeException($"Cannot access index -1, call {nameof(MoveNext)}() first.");
        }

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
    public BoundingBox GetCurrentBoundingBox()
    {
        if (IsAtBeginning)
        {
            throw new IndexOutOfRangeException($"Cannot access index -1, call {nameof(MoveNext)}() first.");
        }

        bool success = PageIteratorApi.BoundingBox(Handle, Level,
            out int left, out int top, out int right, out int bottom);

        return new(left, top, right, bottom);
    }


    public bool MoveNext()
    {
        if (IsAtBeginning)
        {
            IsAtBeginning = false;
            return true;
        }

        _current = null;
        return PageIteratorApi.Next(Handle, Level);
    }
    public void Reset()
    {
        IsAtBeginning = true;
        _current = null;
        PageIteratorApi.Begin(Handle);
    }
    public PageIterator Copy()
    {
        IntPtr copied = PageIteratorApi.Copy(Handle);
        return new(copied, Level);
    }



    /// <summary>
    /// Creation type that affects how disposing is done.
    /// </summary>
    internal ResultIteratorType CreationType { get; }
    internal Pix GetCurrentSpanAsBinaryPix()
    {
        if (IsAtBeginning)
        {
            throw new IndexOutOfRangeException($"Cannot access index -1, call {nameof(MoveNext)}() first.");
        }

        IntPtr pixPtr = PageIteratorApi.GetBinaryImage(Handle, Level);
        NullPointerException.ThrowIfNull(pixPtr);
        return new(pixPtr);
    }
    internal Pix GetCurrentSpanAsPix(int padding, PixHandle pixHandle, out Point2D textStart)
    {
        if (IsAtBeginning)
        {
            throw new IndexOutOfRangeException($"Cannot access index -1, call {nameof(MoveNext)}() first.");
        }

        // Return binary image if pix Handle is not valid
        IntPtr pixPtr = PageIteratorApi.GetImage(Handle, Level, padding,
            pixHandle.Handle, out int left, out int top);
        textStart = new(left, top);

        NullPointerException.ThrowIfNull(pixPtr);
        return new(pixPtr);
    }


    private SpanInfo GetCurrent()
    {
        return new SpanInfo
        {
            Info = GetCurrentParagraphInfo(),
            Box = GetCurrentBoundingBox()
        };
    }


    protected override void Dispose(bool disposing)
    {
        if (Handle.Handle != IntPtr.Zero)
        {
            switch (CreationType)
            {
                case ResultIteratorType.ResultIteratorBased:
                    break;

                case ResultIteratorType.Copied:
                case ResultIteratorType.EngineBased:
                    PageIteratorApi.Delete(Handle);
                    break;

                default:
                    throw new NotImplementedException($"{nameof(ResultIteratorType)} of {CreationType} not implemented.");
            }
            Handle = new HandleRef(this, IntPtr.Zero);
        }
    }
}

#endif