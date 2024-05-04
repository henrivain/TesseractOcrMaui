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
    public ResultIterable(TessEngine engine) : this(engine, PageIteratorLevel.TextLine) { }

    /// <summary>
    /// New IEnumerable implementation of <see cref="ResultIterator"/>. Iterate over different text block sizes.
    /// </summary>
    /// <param name="engine">Engine that must exist as long as the iterator, not disposed automatically.</param>
    /// <param name="level">Text block size to be used.</param>
    public ResultIterable(TessEngine engine, PageIteratorLevel level)
    {
        ArgumentNullException.ThrowIfNull(engine);

        _engine = engine;
        _level = level;
    }


    private readonly PageIteratorLevel _level;
    private readonly TessEngine _engine;


    /// <summary>
    /// An enumerator that can be used to iterate through the collection.
    /// </summary>
    /// <returns>Retuns an enumerator that iterates through the collection.</returns>
    /// <exception cref="ObjectDisposedException">If <see cref="_engine"/> is disposed.</exception>
    /// <exception cref="NullPointerException">If <see cref="TessEngine.Handle"/> is <see cref="IntPtr.Zero"/>.</exception>
    /// <exception cref="ResultIteratorException">
    /// If native iterator cannot be initialized. Make sure <see cref="TessEngine.SetImage(Pix)"/> and 
    /// <see cref="TessEngine.Recognize(HandleRef?)"/> are called.
    /// </exception>
    public IEnumerator<TextSpan> GetEnumerator()
    {
        using ResultIterator iterator = new(_engine, _level);
        while (iterator.MoveNext())
        {
            yield return iterator.Current;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

#endif