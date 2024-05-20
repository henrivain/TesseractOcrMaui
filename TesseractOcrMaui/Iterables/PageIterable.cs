using System.Collections;
using TesseractOcrMaui.Results;
using TesseractOcrMaui.Tessdata;

namespace TesseractOcrMaui.Iterables;

/// <summary>
/// <see cref="IEnumerable{SpanInfo}"/> implementation for <see cref="PageIterable"/>. 
/// Iterate over Text layout.
/// Note that this class is <see cref="IDisposable"/>.
/// </summary>
public class PageIterable : DisposableObject, IDisposable, IEnumerable<SpanLayout>
{
    readonly TessEngine _engine;
    readonly PageIterator _iterator;
    readonly bool _isEngineDisposalRequired = true;

    /// <summary>
    /// New <see cref="IEnumerable{SpanInfo}"/> implementation for <see cref="PageIterator"/>.
    /// Iterate over Text layout.
    /// Note that this class is <see cref="IDisposable"/>.
    /// </summary>
    /// <param name="image">Image to be processed. Image disposal is not handled by the <see cref="TextMetadataIterable"/>.</param>
    /// <param name="provider">Traineddata information to be used.</param>
    /// <param name="level">Text block size to be iterated with.</param>
    /// <param name="logger">Logger to be used, if null uses NullLogger.</param>
    /// <exception cref="ObjectDisposedException">If object is disposed during iteration.</exception>
    /// <exception cref="ImageRecognizionException">If image cannot be processed and recognition failed.</exception>
    /// <exception cref="NullPointerException">
    /// If <paramref name="image"/>.Handle is null orr iterator cannot be copied during iteration.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="provider"/> TessDataFolder or GetLanguagesString() returns 
    /// null or <paramref name="image"/> is null.
    /// </exception>
    public PageIterable(Pix image, ITessDataInformationProvider provider,
        PageIteratorLevel level = PageIteratorLevel.TextLine, ILogger? logger = null)
    {
        string? languages = provider?.GetLanguagesString();
        string? tessDataPath = provider?.TessDataFolder;

        ArgumentNullException.ThrowIfNull(languages);
        ArgumentNullException.ThrowIfNull(tessDataPath);
        ArgumentNullException.ThrowIfNull(image);
        NullPointerException.ThrowIfNull(image.Handle);

        _isEngineDisposalRequired = true;
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
    /// <param name="engine">
    /// Dependency <see cref="TessEngine"/>, must exist as long as created <see cref="PageIterable"/>. 
    /// Disposal of the engine is not handled.
    /// </param>
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

        _isEngineDisposalRequired = false;
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

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <param name="disposing"></param>
    protected override void Dispose(bool disposing)
    {
        if (_isEngineDisposalRequired)
        {
            _engine.Dispose();
        }
        _iterator.Dispose();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
