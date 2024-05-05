#if !IOS

using System.Collections;
using TesseractOcrMaui.Results;

namespace TesseractOcrMaui.Iterables;

/// <summary>
/// IEnumerable implementation of <see cref="ResultIterator"/>. 
/// Iterate over different text block sizes.
/// </summary>
public class ResultIterable : IEnumerable<TextSpan>
{
    /// <summary>
    /// New IEnumerable implementation of <see cref="ResultIterator"/>. Iterate over different text block sizes.
    /// </summary>
    /// <param name="engine">Engine that must exist as long as the iterator, not disposed automatically.</param>
    /// <param name="level">Text block size to be used.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="engine"/> null.</exception>
    /// <exception cref="ObjectDisposedException">If <see cref="_engine"/> is disposed.</exception>
    /// <exception cref="NullPointerException">If <see cref="TessEngine.Handle"/> is <see cref="IntPtr.Zero"/>.</exception>
    /// <exception cref="ResultIteratorException">If native asset is null, consider making bug report if you encouter.</exception>
    /// <exception cref="TesseractInitException">
    /// If <see cref="TessEngine.SetImage(Pix)"/> or <see cref="TessEngine.Recognize(HandleRef?)"/> is not called.
    /// </exception>
    public ResultIterable(TessEngine engine, PageIteratorLevel level = PageIteratorLevel.TextLine)
    {
        ArgumentNullException.ThrowIfNull(engine);

        _engine = engine;
        _level = level;
    }


    readonly PageIteratorLevel _level;
    readonly TessEngine _engine;


    /// <summary>
    /// An enumerator that can be used to iterate through the collection.
    /// </summary>
    /// <returns>Retuns an enumerator that iterates through the collection.</returns>
    /// <exception cref="ObjectDisposedException">If <see cref="_engine"/> is disposed.</exception>
    /// <exception cref="NullPointerException">If <see cref="TessEngine.Handle"/> is <see cref="IntPtr.Zero"/>.</exception>
    /// <exception cref="ResultIteratorException">If native asset is null, consider making bug report if you encouter.</exception>
    /// <exception cref="TesseractInitException">
    /// If <see cref="TessEngine.SetImage(Pix)"/> or <see cref="TessEngine.Recognize(HandleRef?)"/> is not called.
    /// </exception>
    public IEnumerator<TextSpan> GetEnumerator()
    {
        // ArgumentNullException: _engine cannot be null -> cannot throw
        using ResultIterator iterator = new(_engine, _level);

        while (iterator.MoveNext())
        {
            // IndexOutOfBoundsException: MoveNext() called -> cannot throw
            yield return iterator.Current;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

#endif