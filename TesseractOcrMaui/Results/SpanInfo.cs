using System.Diagnostics.CodeAnalysis;
using TesseractOcrMaui.Imaging;

namespace TesseractOcrMaui.Results;

/// <summary>
/// Information about recognized text span.
/// </summary>
public readonly struct SpanInfo
{
    /// <summary>
    /// Information about recognized text span.
    /// </summary>
    /// <param name="info">Information about image text paragraph layout.</param>
    /// <param name="box">Text span location.</param>
    [SetsRequiredMembers]
    public SpanInfo(ParagraphInfo info, BoundingBox box)
    {
        Info = info;
        Box = box;
    }

    /// <summary>
    /// Information about image text paragraph layout.
    /// </summary>
    public required ParagraphInfo Info { get; init; }
    
    /// <summary>
    /// 2d bounding box inside image borders.
    /// </summary>
    public required BoundingBox Box { get; init; }
}
