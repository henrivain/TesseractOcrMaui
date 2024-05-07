using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TesseractOcrMaui.Results;

/// <summary>
/// Data structure to store text structure from recognizion.
/// </summary>
public class BlockLevelCollection
{
    static readonly string _paragraphLineEnding = $"{Environment.NewLine}{Environment.NewLine}";

    /// <summary>
    /// New BlockLevelCollection to store text structure from recognizion.
    /// <para/>Either <see cref="LowerLevelData"/> or <see cref="Data"/> is defined, not both at the same time.
    /// </summary>
    /// <param name="data">Data from current <see cref="PageIteratorLevel"/>.</param>
    /// <param name="level"><see cref="PageIteratorLevel"/> this data collection presents.</param>
    public BlockLevelCollection(List<TextSpan> data, PageIteratorLevel level)
    {
        ArgumentNullException.ThrowIfNull(data);
        Data = data.ToImmutableArray();
        LowerLevelData = null;
        BlockLevel = level;
        IsDataLayer = true;
    }

    /// <summary>
    /// New BlockLevelCollection to store text structure from recognizion.
    /// <para/>Either <see cref="LowerLevelData"/> or <see cref="Data"/> is defined, not both at the same time.
    /// </summary>
    /// <param name="lowerLevelData">Data from lower <see cref="PageIteratorLevel"/>.</param>
    /// <param name="level"><see cref="PageIteratorLevel"/> this data collection presents.</param>
    public BlockLevelCollection(List<BlockLevelCollection> lowerLevelData, PageIteratorLevel level)
    {
        ArgumentNullException.ThrowIfNull(lowerLevelData);
        Data = null;
        LowerLevelData = lowerLevelData.ToImmutableArray();
        BlockLevel = level;
        IsDataLayer = false;
    }

    /// <summary>
    /// Value 
    /// </summary>
    [MemberNotNullWhen(true, nameof(Data))]
    [MemberNotNullWhen(false, nameof(LowerLevelData))]
    public bool IsDataLayer { get; }

    /// <summary>
    /// What block level data is. TextLine -> Contains single text line, meaning array of words.
    /// </summary>
    public PageIteratorLevel BlockLevel { get; }

    /// <summary>
    /// Data from lower <see cref="PageIteratorLevel"/>. 
    /// </summary>
    /// <returns>
    /// <see cref="ImmutableArray{BlockLevelCollection}"/> of <see cref="BlockLevelCollection"/> 
    /// from lower block level when <see cref="IsDataLayer"/> is <see langword="false"/>, otherwise <see langword="null"/>.
    /// </returns>
    public ImmutableArray<BlockLevelCollection>? LowerLevelData { get; }

    /// <summary>
    /// Data from current level. 
    /// </summary>
    /// <returns>
    ///  <see cref="ImmutableArray{BlockLevelCollection}"/> of <see cref="TextSpan"/>
    ///  from current block level when <see cref="IsDataLayer"/> is is <see langword="true"/>, 
    ///  otherwise <see langword="null"/>.
    /// </returns>
    public ImmutableArray<TextSpan>? Data { get; }

    /// <summary>
    /// Build recognized text into user suitable string.
    /// </summary>
    /// <returns>StringBuilder that represents recognized text object.</returns>
    /// <exception cref="NotImplementedException">If <see cref="BlockLevel"/> is <see cref="PageIteratorLevel.Block"/> or any invalid value.</exception>
    public StringBuilder Build(out Average confidence)
    {
        confidence = new();
        return BlockLevel switch
        {
            PageIteratorLevel.Paragraph => GetBlock(this, ref confidence),
            PageIteratorLevel.TextLine => GetParagraph(this, ref confidence),
            PageIteratorLevel.Word => GetTextLine(this, ref confidence),
            PageIteratorLevel.Symbol => GetWord(this, ref confidence),
            PageIteratorLevel.Block or _ => 
                throw new InvalidOperationException("Block type invalid, give any valid enum value except Block."),
        };
    }


    static StringBuilder GetWord(BlockLevelCollection collection, ref Average confidence)
    {
        if (collection.BlockLevel is not PageIteratorLevel.Symbol)
        {
            throw new InvalidOperationException("Collection iterator level should be SYMBOL to parse word.");
        }
        if (collection.IsDataLayer is false)
        {
            throw new NotImplementedException("Currently symbol level collection should always be datalayer.");
        }
        
        StringBuilder builder = new();
        foreach (TextSpan symbol in collection.Data)
        {
            confidence.ReCalculate(symbol.Confidence);
            builder.Append(symbol.Text);
        }
        return builder;
    }

    static StringBuilder GetTextLine(BlockLevelCollection collection, ref Average confidence)
    {
        if (collection.BlockLevel is not PageIteratorLevel.Word)
        {
            throw new InvalidOperationException("Collection iterator level should be WORD to parse line.");
        }

        StringBuilder builder = new();
        if (collection.IsDataLayer)
        {
            foreach (TextSpan word in collection.Data)
            {
                confidence.ReCalculate(word.Confidence);
                builder.Append(word.Text)
                       .Append(' ');
            }
        }
        else
        {
            foreach (var wordData in collection.LowerLevelData)
            {
                builder.Append(GetWord(wordData, ref confidence))
                       .Append(' ');
            }
        }
        
        return builder;
    }

    static StringBuilder GetParagraph(BlockLevelCollection collection, ref Average confidence, bool addNewLines = true)
    {
        if (collection.BlockLevel is not PageIteratorLevel.TextLine)
        {
            throw new InvalidOperationException("Collection iterator level should be TEXTLINE to parse paragraph.");
        }

        StringBuilder builder = new();
        if (collection.IsDataLayer)
        {
            foreach (TextSpan line in collection.Data)
            {
                confidence.ReCalculate(line.Confidence);
                builder.Append(line.Text);

                // Replace wrong line endings with correct one
                RemoveLineEndingFromBuilder(line.Text, ref builder);
                if (addNewLines)
                {
                    builder.AppendLine();
                }
            }
        }
        else
        {
            foreach (var lineData in collection.LowerLevelData)
            {
                builder.Append(GetTextLine(lineData, ref confidence));
                if (addNewLines)
                {
                    builder.AppendLine();
                }
            }
        }
        return builder;
    }

    static StringBuilder GetBlock(BlockLevelCollection collection, ref Average confidence)
    {
        if (collection.BlockLevel is not PageIteratorLevel.Paragraph)
        {
            throw new InvalidOperationException("Collection iterator level should be PARAGRAPH to parse BLOCK.");
        }
        StringBuilder builder = new();
        if (collection.IsDataLayer)
        {
            foreach(TextSpan paragraph in collection.Data) 
            { 
                if (builder.Length is not 0)
                {
                    builder.Append($" {_paragraphLineEnding}");
                }
                confidence.ReCalculate(paragraph.Confidence);
                builder.Append(paragraph.Text);
                RemoveLineEndingFromBuilder(paragraph.Text, ref builder);
            }
        }
        else
        {
            foreach (var paragraph in collection.LowerLevelData)
            {
                if (builder.Length is not 0)
                {
                    builder.Append($" {_paragraphLineEnding}");
                }
                builder.Append(GetParagraph(paragraph, ref confidence, false));
            }
        }
        return builder;
    }

    static void RemoveLineEndingFromBuilder(ReadOnlySpan<char> originalSpan, ref StringBuilder appendedBuilder)
    {
        // Orginal string must be appended to builder before calling!
        // This method removes any \n \r characters from the builder end (maximum of originalSpan.Length)

        int count = 0;
        for (int i = originalSpan.Length - 1; i >= 0; i--)
        {
            if (originalSpan[i] is not '\r' and not '\n')
            {
                break;
            }
            count++;
        }
        if (count > 0)
        {
            int startIndex = appendedBuilder.Length - count;
            appendedBuilder.Remove(startIndex, count);
        }
    }
}
