namespace TesseractOcrMaui.Enums;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Possible page layout analysis modes
/// </summary>
public enum PageSegmentationMode
{
    OsdOnly,
    AutoOsd,
    AutoOnly,
    Auto,
    SingleColumn,
    SingleBlockVertText,
    SingleBlock,
    SingleLine,
    SingleWord,
    CircleWord,
    SingleChar,
    SparseText,
    SparseTextOsd,
    RawLine,
    Count
}
