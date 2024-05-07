using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TesseractOcrMaui.Results;

/// <summary>
/// Data structure to store text structure from recognizion.
/// </summary>
public readonly struct BlockLevelCollection
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="level"></param>
    public BlockLevelCollection(List<TextSpan>? data, PageIteratorLevel level)
    {
        // TODO: comment
        Data = data?.ToImmutableArray();
        LowerLevelData = null;
        BlockLevel = level;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lowerLevelData"></param>
    /// <param name="level"></param>
    public BlockLevelCollection(List<BlockLevelCollection>? lowerLevelData, PageIteratorLevel level)
    {
        // TODO: comment
        Data = null;
        LowerLevelData = lowerLevelData?.ToImmutableArray();
        BlockLevel = level;
    }


    /// <summary>
    /// New BlockLevelCollection to store text structure from recognizion.
    /// </summary>
    /// <param name="data">Data from current <see cref="PageIteratorLevel"/>.</param>
    /// <param name="lowerLevelData">Data from lower <see cref="PageIteratorLevel"/>.</param>
    /// <param name="blockLevel"><see cref="PageIteratorLevel"/> where <paramref name="data"/> was read from.</param>
    public BlockLevelCollection(List<TextSpan>? data,  List<BlockLevelCollection>? lowerLevelData, PageIteratorLevel blockLevel)
    {
        BlockLevel = blockLevel;
        Data = data?.ToImmutableArray();
        LowerLevelData = lowerLevelData?.ToImmutableArray();
    }

    /// <summary>
    /// What block level current data was read from.
    /// </summary>
    public PageIteratorLevel BlockLevel { get; }

    /// <summary>
    /// Data from lower <see cref="PageIteratorLevel"/>.
    /// </summary>
    /// <returns>
    /// <see cref="BlockLevelCollection"/> from lower level, if current is not the lowest, otherwise <see langword="null"/>.
    /// </returns>
    public ImmutableArray<BlockLevelCollection>? LowerLevelData { get; }

    /// <summary>
    /// Data from current level.
    /// </summary>
    public ImmutableArray<TextSpan>? Data { get; }







}
