using System.Collections;
using TesseractOcrMaui.Results;
using TesseractOcrMaui.Tessdata;

namespace TesseractOcrMaui.Iterables;

/// <summary>
/// <see cref="IEnumerable{SpanInfo}"/> implementation for <see cref="PageIterable"/>. 
/// Iterate over Text layout.
/// </summary>
public class PageIterable : IEnumerable<SpanLayout>
{
    readonly TessEngine _engine;
    readonly PageIterator _iterator;

    /// <summary>
    /// New <see cref="IEnumerable{SpanInfo}"/> implementation for <see cref="PageIterator"/>.
    /// Iterate over Text layout.
    /// </summary>
    /// <param name="image"></param>
    /// <param name="provider"></param>
    /// <param name="level"></param>
    /// <param name="logger"></param>
    /// <exception cref="ObjectDisposedException">If object is disposed during iteration.</exception>
    /// <exception cref="ImageRecognizionException">If image cannot be processed and recognition failed.</exception>
    /// <exception cref="NullPointerException">
    /// If <paramref name="image"/>.Handle is null orr iterator cannot be copied during iteration.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="provider"/> TessDataFolder or GetLanguagesString() returns 
    /// null or <paramref name="image"/> is null.
    /// </exception>
    /// 
    public PageIterable(Pix image, ITessDataInformationProvider provider,
        PageIteratorLevel level = PageIteratorLevel.TextLine, ILogger? logger = null)
    {
        string? languages = provider?.GetLanguagesString();
        string? tessDataPath = provider?.TessDataFolder;

        ArgumentNullException.ThrowIfNull(languages);
        ArgumentNullException.ThrowIfNull(tessDataPath);
        ArgumentNullException.ThrowIfNull(image);
        NullPointerException.ThrowIfNull(image.Handle);

        // InvalidOperationException: Always init new engine -> cannot throw
        // ImageNotSetException: SetImage() always called -> cannot throw
        _engine = new(languages, tessDataPath, logger);
        _engine.SetImage(image);
        _engine.Recognize();
        _iterator = new(_engine, level);

        Level = level;
    }


    /// <summary>
    /// New <see cref="IEnumerable{SpanInfo}"/> implementation for <see cref="PageIterator"/>.
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
    internal PageIterable(TessEngine engine, PageIteratorLevel level)
    {
        ArgumentNullException.ThrowIfNull(engine);
        NullPointerException.ThrowIfNull(engine.Handle);

        _engine = engine;
        _iterator = new(_engine, level);

        Level = level;
    }

    /// <summary>
    /// Text block size to be iterated with.
    /// </summary>
    public PageIteratorLevel Level { get; }


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
        iterator.Level = Level;
        while (iterator.MoveNext())
        {
            // IndexOutOfRangeException: MoveNext() called -> cannot throw
            yield return iterator.Current;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
