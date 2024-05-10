using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

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
    /// True if data is stored in this BlockLevelCollection, false if data is in LowerLevelData collections.
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
    /// Print output text in ACSII tree. 
    /// This is mostly for testing purposes, and isn't optimized for anything.
    /// </summary>
    /// <param name="writeLine">Function to print output.</param>
    /// <exception cref="ArgumentOutOfRangeException">If lower level data references make a loop. Max depth 100.</exception>
    public void PrintStructureToOutput(Action<string>? writeLine)
    {
        if (writeLine is null)
        {
            return;
        }

        Dictionary<PageIteratorLevel, string> names = new()
        {
            [PageIteratorLevel.Paragraph] = "Paragraph",
            [PageIteratorLevel.TextLine] = "TextLine",
            [PageIteratorLevel.Word] = "Word",
            [PageIteratorLevel.Symbol] = "Symbol",
            [PageIteratorLevel.Block] = "Block",
        };

        char dir = '├';
        char hor = '─';
        char vert = '│';


        PrintLevel(this, 1);

        void PrintLevel(BlockLevelCollection level, int depth)
        {
            // Check to make sure no stackoverflow can happen
            if (depth > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(depth), "Depth should not be this big!");
            }

            // Add block level title
            var titleLine = FillWithStructure(stackalloc char[depth * 3]);
            writeLine($"{titleLine}{names[level.BlockLevel]} /");

            if (level.IsDataLayer)
            {
                // Add data
                ReadOnlySpan<char> horizontals = FillWithStructure(stackalloc char[(depth + 1) * 3]);
                foreach (var text in level.Data)
                {
                    writeLine($"{horizontals}{GetWithoutNewLine(text.Text)}");
                }
            }
            else
            {
                // Print data from lower levels using recursion
                foreach (var block in level.LowerLevelData)
                {
                    PrintLevel(block, depth + 1);
                }
                return;
            }
        }

        ReadOnlySpan<char> FillWithStructure(Span<char> span)
        {
            /* Build horizontal line
            * Every three characters are '│  '
            * Last three are '├─ '
            * Creates line like '│  │  ├─ '
            */
            for (int i = 0; i < span.Length; i++)
            {
                if (i == span.Length - 2)
                {
                    span[i] = hor;
                }
                else if (i == span.Length - 3)
                {
                    span[i] = dir;
                }
                else if (i % 3 == 0)
                {
                    span[i] = vert;
                }
                else
                {
                    span[i] = ' ';
                }
            }
            return span;
        }
    }

    /// <summary>
    /// Build recognized text into user suitable string.
    /// </summary>
    /// <returns>StringBuilder that represents recognized text object.</returns>
    /// <exception cref="NotImplementedException">If <see cref="BlockLevel"/> is <see cref="PageIteratorLevel.Block"/> or any invalid value.</exception>
    public StringBuilder Build(ref IAverage confidence)
    {
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

    /// <inheritdoc/>
    public override string ToString()
    {
#if DEBUG
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
        });
#else
        IAverage average = new Average();
        return Build(ref average).ToString();
#endif
    }

    static StringBuilder GetWord(BlockLevelCollection collection, ref IAverage confidence)
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

    static StringBuilder GetTextLine(BlockLevelCollection collection, ref IAverage confidence)
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

    static StringBuilder GetParagraph(BlockLevelCollection collection, ref IAverage confidence, bool addNewLines = true)
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

    static StringBuilder GetBlock(BlockLevelCollection collection, ref IAverage confidence)
    {
        if (collection.BlockLevel is not PageIteratorLevel.Paragraph)
        {
            throw new InvalidOperationException("Collection iterator level should be PARAGRAPH to parse BLOCK.");
        }
        StringBuilder builder = new();
        if (collection.IsDataLayer)
        {
            foreach (TextSpan paragraph in collection.Data)
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

    static ReadOnlySpan<char> GetWithoutNewLine(ReadOnlySpan<char> text)
    {
        int count = 0;
        for (int j = text.Length - 1; j >= 0; j--)
        {
            if (text[j] is not '\r' and not '\n')
            {
                break;
            }
            count++;
        }
        return text[..^count];
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
