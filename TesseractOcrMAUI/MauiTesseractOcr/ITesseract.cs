using MauiTesseractOcr.Results;

namespace MauiTesseractOcr;

/// <summary>
/// High level abstraction to use Tesseract ocr.
/// </summary>
public interface ITesseract
{
    /// <summary>
    /// Load traineddata files to configured TessDataFolder.
    /// This method should not throw.
    /// </summary>
    /// <returns>DataLoadResult representing loaded trained data status, including possible errors.</returns>
    Task<DataLoadResult> LoadTraineddataAsync();

    /// <summary>
    /// Recognize text from given image. DOES NOT LOAD required TRAINEDDATA!
    /// This method should not throw.
    /// </summary>
    /// <param name="imagePath">Path to image containing file name and extension.</param>
    /// <returns>RecognizionResult representing status of recognizion, including possible errors.</returns>
    RecognizionResult RecognizeText(string imagePath);

    /// <summary>
    /// Load traineddata files from app packages and run recognizion process async.
    /// This method should not throw.
    /// </summary>
    /// <param name="imagePath">Path to image containing file name and extension.</param>
    /// <returns>Task of RecognizionResult representing status of recognizion, including possible errors.</returns>
    Task<RecognizionResult> RecognizeTextAsync(string imagePath);

    /// <summary>
    /// Folder where tessdata should be stored. 
    /// Provided by ITessDataProvider. 
    /// Load with LoadTraineddataAsync();
    /// </summary>
    string TessDataFolder { get; }
}
