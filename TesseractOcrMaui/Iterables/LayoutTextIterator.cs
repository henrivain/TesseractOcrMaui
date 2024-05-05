using System.Collections;
using TesseractOcrMaui.Results;

namespace TesseractOcrMaui.Iterables;


/// <summary>
/// Enables Synchronized iteration to text and layout iteration.
/// Inherits from <see cref="IDisposable"/>.
/// </summary>
internal class LayoutTextIterator : DisposableObject, IEnumerable<RecognitionSpan>
{
    public LayoutTextIterator(Pix pix, PageIteratorLevel level)
    {
        Level = level;
    }

    public PageIteratorLevel Level { get; }


    public IEnumerator<RecognitionSpan> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    protected override void Dispose(bool disposing)
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
