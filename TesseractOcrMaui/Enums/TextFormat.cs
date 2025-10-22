namespace TesseractOcrMaui.Enums;

/// <summary>
/// Text output format types.
/// </summary> 
public enum TextFormat
{
    /// <summary>
    /// Normal UTF8 text string containing only recognized characters.
    /// </summary>
    TextOnly,

    /// <summary>
    /// HTML-formatted string with hOCR markup
    /// </summary>
    HOCR,

    /// <summary>
    /// XML-formatted string with ALTO markup
    /// </summary>
    ALTO,

    /// <summary>
    /// XML-formatted string with PAGE markup
    /// </summary>
    Page,

    /// <summary>
    /// TSV-formatted string.
    /// </summary>
    TSV,

    /// <summary>
    /// Char* coded as a UTF8 box file
    /// </summary>
    Box,

    /// <summary>
    /// UTF8 box file with WordStr strings.
    /// </summary>
    StrBox
}