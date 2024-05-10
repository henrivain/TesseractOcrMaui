using System.Collections;
using TesseractOcrMaui.Results;
using TesseractOcrMaui.Tessdata;

namespace TesseractOcrMaui.Iterables;


/// <summary>
/// Enables Synchronized iteration to text and layout.
/// This class is <see cref="IDisposable"/>.
/// </summary>
public class TextMetadataIterable : DisposableObject, IEnumerable<RecognitionSpan>
{
    readonly TessEngine _engine;

    /// <summary>
    /// Enables Synchronized iteration to text and layout.
    /// This class is <see cref="IDisposable"/>.
    /// </summary>
    /// <param name="provider">Traineddata information.</param>
    /// <param name="image">Image to be processed.</param>
    /// <param name="level">Text block size to be iterated with.</param>
    /// <param name="logger">Logger to be used, if null uses NullLogger.</param>
    /// <exception cref="TesseractException">If Tesseract cannot be initialized with given parameters.</exception>
    /// <exception cref="NullPointerException">If <paramref name="image"/>.Handle is null.</exception>
    /// <exception cref="ImageRecognizionException">If image cannot be processed and recognition failed.</exception>
    /// <exception cref="ObjectDisposedException">Object disposed during iteration.</exception>
    /// <exception cref="ResultIteratorException">If native state is invalid, file bug report with input data if thrown.</exception>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="provider"/> TessDataFolder or GetLanguagesString() returns 
    /// null or <paramref name="image"/> is null.
    /// </exception>
    public TextMetadataIterable(Pix image, ITessDataInformationProvider provider, 
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

        ImageHeight = image.Height;
        ImageWidth = image.Width;

        Level = level;
    }

    /// <summary>
    /// Iteration text block size like TextLine or symbol.
    /// </summary>
    public PageIteratorLevel Level { get; }

    /// <summary>
    /// Height of processed image.
    /// </summary>
    public int ImageHeight { get; }

    /// <summary>
    /// Width of processed Image.
    /// </summary>
    public int ImageWidth { get; }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    /// <exception cref="ObjectDisposedException">If object disposed during iteration.</exception>
    /// <exception cref="ResultIteratorException">If native state is invalid, file bug report with input data if thrown.</exception>
    public IEnumerator<RecognitionSpan> GetEnumerator()
    {
        if (_engine.Handle.Handle == IntPtr.Zero)
        {
            // Can only be null if disposed
            throw new ObjectDisposedException(nameof(TextMetadataIterable));
        }

        // NullPointerException: Engine handle checked -> cannot throw
        // ArgumentNullException: Engine always not null -> cannot throw
        // TesseractInitException: .ctor calls SetImage() and Recognize() -> cannot throw
        using SyncIterator iter = new(_engine, Level);

        while (iter.MoveNext())
        {
            yield return new RecognitionSpan
            {
                Span = iter.GetTextSpan(),
                Layout = iter.GetSpanLayout(),
                Level = Level
            };
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        _engine.Dispose();
    }
}
