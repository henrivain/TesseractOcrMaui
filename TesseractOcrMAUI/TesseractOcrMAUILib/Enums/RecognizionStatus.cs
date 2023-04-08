namespace TesseractOcrMAUILib.Enums;

/// <summary>
/// Status returned by tesseract image recognizion process.
/// </summary>
public enum RecognizionStatus
{
    Success, Failed, NoLanguagesAvailable,
    CannotLoadTessData,
    ImageNotFound
}
