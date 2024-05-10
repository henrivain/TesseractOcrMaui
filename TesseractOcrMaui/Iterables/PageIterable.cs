using System.Collections;
using TesseractOcrMaui.Results;

namespace TesseractOcrMaui.Iterables;

/// <summary>
/// <see cref="IEnumerable{SpanInfo}"/> implementation for <see cref="PageIterable"/>. 
/// Iterate over Text layout.
/// </summary>
public class PageIterable : IEnumerable<SpanLayout>
{
    /// <summary>
    /// New <see cref="IEnumerable{SpanInfo}"/> implementation for <see cref="PageIterable"/>.
    /// Iterate over Text layout.
    /// </summary>
    /// <param name="iterator">Dependency <see cref="ResultIterator"/>, must exist as long as created <see cref="PageIterable"/>.</param>
    /// <param name="level"><see cref="PageIteratorLevel"/> that determines text block size.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="iterator"/> is null.</exception>
    /// <exception cref="ObjectDisposedException">If <see cref="_iterator"/> is disposed before or during iteration.</exception>
    /// <exception cref="NullPointerException">
    /// If <paramref name="iterator"/>.Handle is <see cref="IntPtr.Zero"/> 
    /// or iterator cannot be copied during iteration.
    /// </exception>
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
    /// <exception cref="ObjectDisposedException">If <see cref="_iterator"/> is disposed before or during iteration.</exception>
    /// <exception cref="ArgumentNullException">If <paramref name="engine"/> is null.</exception>
    /// <exception cref="PageIteratorException">If image does not contain text.</exception>
    /// <exception cref="ImageNotSetException">If <see cref="TessEngine.SetImage(Pix)"/> is not called before init.</exception>
    /// <exception cref="NullPointerException">
    /// If <paramref name="engine"/>.Handle is <see cref="IntPtr.Zero"/> 
    /// or iterator cannot be copied during iteration.
    /// </exception>
    public PageIterable(TessEngine engine, PageIteratorLevel level = PageIteratorLevel.TextLine)
    {
        // TODO: Comment exceptions
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
    /// <exception cref="ArgumentNullException">If <paramref name="iterator"/> is null.</exception>
    /// <exception cref="NullPointerException">If iterator cannot be copied.</exception>
    /// <exception cref="ObjectDisposedException">If <see cref="_iterator"/> is disposed before or during iteration.</exception>
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
    public IEnumerator<SpanLayout> GetEnumerator()
    {
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
