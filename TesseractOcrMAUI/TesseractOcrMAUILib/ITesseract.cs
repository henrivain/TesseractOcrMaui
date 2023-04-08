using MauiTesseractOcr.Results;

namespace MauiTesseractOcr;

/// <summary>
/// High level abstraction to use Tesseract ocr.
/// </summary>
public interface ITesseract
{
    /// <summary>
    /// Load traineddata files to configured TessDataFolder.
    /// </summary>
    /// <returns>DataLoadResult representing loaded trained data status, includes possible errors.</returns>
    Task<DataLoadResult> LoadTraineddataAsync();

    /// <summary>
    /// Recognize text from given image. DOES NOT LOAD required TRAINEDDATA!
    /// </summary>
    /// <param name="imagePath">Path to image containing file name and extension.</param>
    /// <returns>RecognizionResult representing status of recognizion, includes possible errors.</returns>
    RecognizionResult RecognizeText(string imagePath);

    /// <summary>
    /// Load traineddata files from app packages and run recognizion process async.
    /// </summary>
    /// <param name="imagePath">Path to image containing file name and extension.</param>
    /// <returns>Task of RecognizionResult representing status of recognizion, includes possible errors.</returns>
    Task<RecognizionResult> RecognizeTextAsync(string imagePath);

    /// <summary>
    /// Folder where tessdata should be stored, load with LoadTraineddataAsync();
    /// </summary>
    string TessDataFolder { get; }
}
