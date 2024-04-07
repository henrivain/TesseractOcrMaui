#if !IOS

using System.Collections;
using TesseractOcrMaui.Results;

namespace TesseractOcrMaui.Iterables;

/// <summary>
/// IEnumerable implementation of <see cref="ResultIterator"/>.
/// <see cref="IDisposable"/> handles disposing of <see cref="ResultIterator"/>.
/// </summary>
public class ResultIterable : IEnumerable<TextSpan>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="engine"></param>
    public ResultIterable(TessEngine engine)
    {
        ArgumentNullException.ThrowIfNull(engine);
        NullPointerException.ThrowIfNull(engine.Handle);

        Handle = new TessEngineHandle(engine);
    }


    TessEngineHandle Handle { get; }


    /// <inheritdoc/>
    public IEnumerator<TextSpan> GetEnumerator()
    {
        using ResultIterator iterator = new(Handle);
        while (iterator.MoveNext())
        {
            yield return iterator.Current;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

#endif