namespace TesseractOcrMaui.Enums;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Tesseract engine mode to be used when recognizing text for images.
/// </summary>
public enum EngineMode
{
    TesseractOnly,
    LstmOnly,
    TesseractAndLstm,
    Default
}