namespace TesseractOcrMAUILib.Enums;

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