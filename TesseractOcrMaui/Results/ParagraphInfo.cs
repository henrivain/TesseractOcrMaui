using System.Diagnostics.CodeAnalysis;

namespace TesseractOcrMaui.Results;

/// <summary>
/// Information about image text paragraph layout.
/// </summary>
public readonly struct ParagraphInfo
{
    /// <summary>
    /// New information about image text paragraph layout.
    /// </summary>
    /// <param name="justification">How text is laid out.</param>
    /// <param name="isListItem">Is paragraph list item.</param>
    /// <param name="isCrown">Is text circular.</param>
    /// <param name="isFirstLineIndent">First line indent size.</param>
    [SetsRequiredMembers]
    public ParagraphInfo(ParagraphJustification justification, 
        bool isListItem, bool isCrown, int isFirstLineIndent)
    {
        Justification = justification;
        IsListItem = isListItem;
        IsCrown = isCrown;
        FirstLineIndent = isFirstLineIndent;
    }

    /// <summary>
    /// How paragraph text is laid out.
    /// </summary>
    public required ParagraphJustification Justification { get; init; } = ParagraphJustification.Unknown;

    /// <summary>
    /// Is paragraph a list item.
    /// </summary>
    public required bool IsListItem { get; init; } 

    /// <summary>
    /// Is text circular.
    /// </summary>
    public required bool IsCrown { get; init; }

    /// <summary>
    /// First line indent size. 
    /// </summary>
    public required int FirstLineIndent { get; init; }

}
