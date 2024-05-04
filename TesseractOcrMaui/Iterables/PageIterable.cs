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
    /// <param name="iterator"></param>
    /// <param name="level"></param>
    public PageIterable(ResultIterator iterator, PageIteratorLevel level = PageIteratorLevel.TextLine)
    {
        ArgumentNullException.ThrowIfNull(iterator);

        _iterator = iterator.AsPageIterator();
        _level = level;

        iterator.Disposed += (_, _) => _iterator.Dispose();
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