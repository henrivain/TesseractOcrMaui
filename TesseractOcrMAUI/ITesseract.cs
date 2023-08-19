using TesseractOcrMaui.Results;

namespace TesseractOcrMaui;

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
    /// Recognize text from given image. 
    /// <para/>DOES NOT LOAD required TRAINEDDATA!
    /// </summary>
    /// <param name="imagePath">Path to image containing file name and extension.</param>
    /// <returns>Recognizion info object that contains extracted data and possible errors.</returns>
    /// <exception cref="DllNotFoundException">[Default implementation] If tesseract or any other library is not found.</exception>
    RecognizionResult RecognizeText(string imagePath);

    /// <summary>
    /// Recognize text from given image bytes.
    /// <para/>DOES NOT LOAD required TRAINEDDATA!
    /// </summary>
    /// <param name="imageBytes">Image from memory as byte array</param>
    /// <returns>Recognizion info object that contains extracted data and possible errors.</returns>
    /// <exception cref="DllNotFoundException">[Default implementation] If tesseract or any other library is not found.</exception>
    RecognizionResult RecognizeText(byte[] imageBytes);

    /// <summary>
    /// Recognize text from given Leptonica Pix image.
    /// <para/>DOES NOT LOAD required TRAINEDDATA!
    /// </summary>
    /// <param name="image">Pix image to be recognized</param>
    /// <returns>Recognizion info object that contains extracted data and possible errors.</returns>
    /// <exception cref="DllNotFoundException">[Default implementation] If tesseract or any other library is not found.</exception>
    RecognizionResult RecognizeText(Pix image);



    /// <summary>
    /// Load traineddata files from app packages and recognize text from given image path async.
    /// </summary>
    /// <param name="imagePath">Path to image containing file name and extension.</param>
    /// <returns>Recognizion info object that contains extracted data and possible errors.</returns>
    /// <exception cref="DllNotFoundException">[Default implementation] If tesseract or any other library is not found.</exception>
    Task<RecognizionResult> RecognizeTextAsync(string imagePath);

    /// <summary>
    /// Load traineddata files from app packages and recognize text from given image bytes async
    /// </summary>
    /// <param name="imageBytes">Image as byte array</param>
    /// <returns>Recognizion info object that contains extracted data and possible errors.</returns>
    /// <exception cref="DllNotFoundException">[Default implementation] If tesseract or any other library is not found.</exception>
    Task<RecognizionResult> RecognizeTextAsync(byte[] imageBytes);

    /// <summary>
    /// Load traineddata files from app packages and recognize text from Leptonica Pix image async
    /// </summary>
    /// <param name="image">Image as Pix -object</param>
    /// <returns>Recognizion info object that contains extracted data and possible errors.</returns>
    /// <exception cref="DllNotFoundException">[Default implementation] If tesseract or any other library is not found.</exception>
    Task<RecognizionResult> RecognizeTextAsync(Pix image);

    /// <summary>
    /// Folder where tessdata should be stored. 
    /// Provided by ITessDataProvider. 
    /// Load with LoadTraineddataAsync();
    /// </summary>
    string TessDataFolder { get; }
}
