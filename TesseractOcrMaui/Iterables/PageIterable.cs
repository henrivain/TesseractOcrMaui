#if !IOS

using System.Collections;
using TesseractOcrMaui.Results;

namespace TesseractOcrMaui.Iterables;

/// <summary>
/// <see cref="IEnumerable{SpanInfo}"/> implementation for <see cref="PageIterable"/>. 
/// Iterate over Text layout.
/// </summary>
public class PageIterable : IEnumerable<SpanInfo>
{
    /// <summary>
    /// New <see cref="IEnumerable{SpanInfo}"/> implementation for <see cref="PageIterable"/>.
    /// Iterate over Text layout.
    /// </summary>
    /// <param name="iterator">Dependency <see cref="ResultIterator"/>, must exist as long as created <see cref="PageIterable"/>.</param>
    /// <param name="level"><see cref="PageIteratorLevel"/> that determines text block size.</param>
    /// <exception cref="NullPointerException">If <paramref name="iterator"/>.Handle is <see cref="IntPtr.Zero"/>.</exception>
    public PageIterable(ResultIterator iterator, PageIteratorLevel level = PageIteratorLevel.TextLine)
    {
        ArgumentNullException.ThrowIfNull(iterator);

        _iterator = iterator.AsPageIterator();
        _level = level;
    }

    /// <summary>
    /// New <see cref="IEnumerable{SpanInfo}"/> implementation for <see cref="PageIterable"/>.
    /// Iterate over Text layout.
    /// </summary>
    /// <param name="engine">Dependency <see cref="TessEngine"/>, must exist as long as created <see cref="PageIterable"/>.</param>
    /// <param name="level"><see cref="PageIteratorLevel"/> that determines text block size.</param>
    public PageIterable(TessEngine engine, PageIteratorLevel level = PageIteratorLevel.TextLine)
    {
        ArgumentNullException.ThrowIfNull(engine);

        _iterator = new(engine);
        _level = level;
    }

    /// <summary>
    /// New <see cref="IEnumerable{SpanInfo}"/> implementation for <see cref="PageIterable"/>.
    /// Iterate over Text layout.
    /// </summary>
    /// <param name="iterator">
    /// Dependency <see cref="PageIterator"/>, 
    /// must exist as long as created <see cref="PageIterable"/>.
    /// Copied every GetEnumerator call.
    /// </param>
    public PageIterable(PageIterator iterator)
    {
        ArgumentNullException.ThrowIfNull(iterator);

        _iterator = iterator;
        _level = iterator.Level;
    }

    readonly PageIterator _iterator;
    readonly PageIteratorLevel _level;

    /// <summary>
    /// An enumerator that can be used to iterate through the collection.
    /// </summary>
    /// <returns>Retuns an enumerator that iterates through the collection.</returns>
    /// <exception cref="NullPointerException">If iterator cannot be copied.</exception>
    /// <exception cref="ObjectDisposedException">If <see cref="_iterator"/> is disposed before or during iteration.</exception>
    public IEnumerator<SpanInfo> GetEnumerator()
    {
        // ArgumentNullException: _dependencyObject cannot be null -> cannot throw
        // Iterator is copied every time to not get it disposed after foreach
        using PageIterator iterator = _iterator.Copy();
        iterator.Level = _level;
        while (iterator.MoveNext())
        {
            // IndexOutOfRangeException: MoveNext() called -> cannot throw
            yield return iterator.Current;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

#endif