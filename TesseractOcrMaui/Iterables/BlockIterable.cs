using System.Collections;
using TesseractOcrMaui.Results;

namespace TesseractOcrMaui.Iterables;

/// <summary>
/// Text structure iterator. Analyze text structure and return the text in structurized form.
/// <para/>This class is <see cref="IDisposable"/> and <see cref="IEnumerable{BlockLevelCollection}"/>.
/// </summary>
public class BlockIterable : DisposableObject, IEnumerable<BlockLevelCollection>
{
    readonly TessEngine _engine;

    const PageIteratorLevel LowestAvailableLevel = PageIteratorLevel.Symbol;

    /// <summary>
    /// Text structure iterator. Analyze text structure and return the text in structurized form.
    /// </summary>
    /// <param name="languages">'+' -separated string of traineddata filenames without extension.</param>
    /// <param name="traineddataPath">Path to traineddata folder.</param>
    /// <param name="image">Image to be analyzed.</param>
    /// <param name="highestLevel">
    /// Biggest block size to be analyzed.
    /// This is the block level that is iterated on.
    /// Block -> returns IEnumerable of Paragraphs
    /// </param>
    /// <param name="lowestLevel">Smallest block size to be analyzed.</param>
    /// <param name="logger"></param>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="highestLevel"/> is higher block level than <paramref name="lowestLevel"/>.
    /// </exception>
    public BlockIterable(string languages,
        string traineddataPath,
        Pix image,
        PageIteratorLevel highestLevel = PageIteratorLevel.Block,
        PageIteratorLevel lowestLevel = PageIteratorLevel.Symbol,
        ILogger? logger = null)
    {
        _engine = new(languages, traineddataPath, logger);
        _engine.SetImage(image);
        _engine.Recognize();

        // Higher level -> smaller integer value
        if (HighestLevelToSearch < LowestLevelToSearch)
        {
            throw new InvalidOperationException("Highest level to search cannot be smaller than lowest to search level.");
        }

        HighestLevelToSearch = highestLevel;
        LowestLevelToSearch = lowestLevel;
    }

    /// <summary>
    /// Biggest block size to be analyzed.
    /// </summary>
    public PageIteratorLevel HighestLevelToSearch { get; }

    /// <summary>
    /// Smallest block size to be analyzed.
    /// </summary>
    public PageIteratorLevel LowestLevelToSearch { get; }

    /// <summary>
    /// Retums an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    /// <exception cref="ObjectDisposedException">If object disposed during iteration.</exception>
    /// <exception cref="NullPointerException">If <see cref="_engine"/>.Handle Intptr.Zero meaning it is disposed.</exception>
    /// <exception cref="ResultIteratorException">Native asset null, make bug report with used data if thrown.</exception>
    /// <exception cref="TesseractInitException">If native copying iterator failed.</exception>
    public IEnumerator<BlockLevelCollection> GetEnumerator()
    {
        // ArgumentNullException: _engine cannot be null -> cannot throw
        using SyncIterator iter = new(_engine, HighestLevelToSearch);


        while (iter.MoveNext())
        {
            BlockLevelCollection? result = GetLower(iter, HighestLevelToSearch);
            if (result is null)
            {
                continue;
            }
            yield return result;
        }

        BlockLevelCollection? GetLower(in SyncIterator higherLevelIter, PageIteratorLevel last)
        {
            /* Check if already at lowest level
             * Bigger value means more precise level 
             * like Symbol (4) > Word (3) */
            if (last >= LowestAvailableLevel || last > LowestAvailableLevel)
            {
                return null;
            }
            if (last == LowestLevelToSearch)
            {
                return null;
            }

            // Configure iterators
            PageIteratorLevel currentLevel = last.GetLevelLower();
            using SyncIterator iter = higherLevelIter.CopyAtCurrentIndex(currentLevel);

            // Start iteration
            List<TextSpan> spans = new();
            List<BlockLevelCollection> lowerLevels = new();
            bool hasLowerLevels = false;

            do
            {
                BlockLevelCollection? lowerCollection = GetLower(iter, currentLevel);

                // Null only returned if already at lowest level
                if (lowerCollection is not null)
                {
                    hasLowerLevels = true;
                }

                if (lowerCollection is null)
                {
                    spans.Add(iter.GetTextSpan());
                }
                else
                {
                    lowerLevels.Add(lowerCollection);
                }
                if (iter.IsAtFinalElement(last))
                {
                    break;
                }
            }
            while (iter.MoveNext());

            // Only lower levels or current level is returned, both cannot be defined at the same time
            return hasLowerLevels
                ? new(lowerLevels, currentLevel)
                : new(spans, currentLevel);
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        _engine.Dispose();
    }
}
