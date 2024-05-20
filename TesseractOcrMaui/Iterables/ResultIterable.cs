using System.Collections;
using TesseractOcrMaui.Results;
using TesseractOcrMaui.Tessdata;

namespace TesseractOcrMaui.Iterables;

/// <summary>
/// IEnumerable implementation of <see cref="ResultIterator"/>. 
/// Iterate over different text block sizes.
/// Note that this iterable is <see cref="IDisposable"/>.
/// </summary>
public class ResultIterable : DisposableObject, IDisposable, IEnumerable<TextSpan>
{
    readonly TessEngine _engine;
    readonly bool _isEngineDisposalRequired = true;


    /// <summary>
    /// New IEnumerable implementation of <see cref="ResultIterator"/>. Iterate over different text block sizes.
    /// Note that this iterable is <see cref="IDisposable"/>.
    /// </summary>
    /// <param name="image">Image to be processed. Disposal of the image is not handled by the <see cref="ResultIterable"/>.</param>
    /// <param name="provider">Traineddata information.</param>
    /// <param name="level">Text block size to be used.</param>
    /// <param name="logger"></param>
    /// <exception cref="TesseractException">If Tesseract cannot be initialized with given parameters.</exception>
    /// <exception cref="NullPointerException">If <paramref name="image"/>.Handle is null.</exception>
    /// <exception cref="ImageRecognizionException">If image cannot be processed and recognition failed.</exception>
    /// <exception cref="ObjectDisposedException">Object disposed during iteration.</exception>
    /// <exception cref="ResultIteratorException">If native state is invalid, file bug report with input data if thrown.</exception>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="provider"/> TessDataFolder or GetLanguagesString() returns 
    /// null or <paramref name="image"/> is null.
    /// </exception>
    public ResultIterable(
        Pix image,
        ITessDataInformationProvider provider, 
        PageIteratorLevel level = PageIteratorLevel.TextLine, 
        ILogger? logger = null)
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

        Level = level;
    }


    /// <summary>
    /// New IEnumerable implementation of <see cref="ResultIterator"/>. Iterate over different text block sizes.
    /// Note that this iterable is <see cref="IDisposable"/>.
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
    internal ResultIterable(TessEngine engine, PageIteratorLevel level = PageIteratorLevel.TextLine)
    {
        _isEngineDisposalRequired = false;
        ArgumentNullException.ThrowIfNull(engine);

        _engine = engine;
        Level = level;
    }

    /// <summary>
    /// Text block size to be used.
    /// </summary>
    public PageIteratorLevel Level { get; }


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
        using ResultIterator iterator = new(_engine, Level);

        while (iterator.MoveNext())
        {
            // IndexOutOfBoundsException: MoveNext() called -> cannot throw
            yield return iterator.Current;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <param name="disposing"></param>
    protected override void Dispose(bool disposing)
    {
        // Handle engine disposal if engine was created by the ResultIterable ctor.
        if (_isEngineDisposalRequired)
        {
            _engine.Dispose();
        }
    }
}
