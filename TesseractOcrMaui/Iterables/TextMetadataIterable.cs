using System.Collections;
using TesseractOcrMaui.Results;

namespace TesseractOcrMaui.Iterables;


/// <summary>
/// Enables Synchronized iteration to text and layout iteration.
/// Inherits from <see cref="IDisposable"/>.
/// </summary>
public class TextMetadataIterable : DisposableObject, IEnumerable<RecognitionSpan>
{
    readonly TessEngine _engine;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="languages"></param>
    /// <param name="traineddataPath"></param>
    /// <param name="image"></param>
    /// <param name="level"></param>
    /// <param name="logger"></param>
    public TextMetadataIterable(
        string languages, 
        string traineddataPath, 
        Pix image, 
        PageIteratorLevel level = PageIteratorLevel.TextLine,
        ILogger? logger = null)
    {
        // TODO: Docs
        _engine = new(languages, traineddataPath, logger);
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
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerator<RecognitionSpan> GetEnumerator()
    {
        // TODO: Docs

        /* AsPageIterator uses the same pointer,
         * so iterators are synchronized.
         * Same native reference is used. 
         */
        using ResultIterator resultIter = new(_engine, Level);
        using PageIterator pageIter = resultIter.AsPageIterator();

        // This is used to move from index -1 state in c# side
        pageIter.MoveNext();

        while (resultIter.MoveNext())
        {
            yield return new RecognitionSpan
            {
                Span = resultIter.Current,
                Layout = pageIter.Current,
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
