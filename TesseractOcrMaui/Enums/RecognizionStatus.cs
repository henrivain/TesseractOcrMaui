namespace TesseractOcrMaui.Enums;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member


/// <summary>
/// Status returned by tesseract image recognizion process.
/// </summary>
public enum RecognizionStatus
{
    InProgressSuccess = -1,
    Success = 0,
    Failed = 1, 
    NoLanguagesAvailable,
    CannotLoadTessData,
    ImageNotFound,
    TraineddataNotLoaded,
    UnknowError,
    InvalidImage,
    ImageAlredyProcessed,
    CannotRecognizeText,
    TessDataFolderNotProvided
}
