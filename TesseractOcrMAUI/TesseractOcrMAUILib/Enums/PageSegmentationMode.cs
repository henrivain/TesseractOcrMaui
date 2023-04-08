namespace MauiTesseractOcr.Enums;


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
