using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace TesseractOcrMaui.Results;


/// <summary>
/// Result iterator recognizion result
/// </summary>
public readonly record struct TextSpan
{
    /// <summary>
    /// New Result iterator TextSpan
    /// </summary>
    public TextSpan() { }

    /// <summary>
    /// New Result iterator TextSpan
    /// </summary>
    [SetsRequiredMembers]
    public TextSpan(string text, float confidence, PageIteratorLevel level)
    {
        Text = text;
        Confidence = confidence;
        Level = level;
    }
    /// <summary>
    /// Recognized text span.
    /// </summary>
    public required string Text { get; init; }

    /// <summary>
    /// Recognizion confidence for current text span.
    /// </summary>
    public required float Confidence { get; init; }

    /// <summary>
    /// PageIteratorLevel that was used to get this text span.
    /// </summary>
    public required PageIteratorLevel Level { get; init; }

    /// <inheritdoc/>
    public override string ToString()
    {
#if DEBUG
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
             WriteIndented = true,
        });
#else
        return base.ToString() ?? string.Empty;
#endif
    }
}
