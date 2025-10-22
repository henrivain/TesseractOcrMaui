﻿using TesseractOcrMaui.Results;

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
    /// Try to get Tesseract library version 
    /// </summary>
    /// <returns>string representation of version if version available, otherwise null</returns>
    string? TryGetTesseractLibVersion();

    /// <summary>
    /// Folder where tessdata should be stored. 
    /// Provided by ITessDataProvider. 
    /// Load with LoadTraineddataAsync();
    /// </summary>
    string TessDataFolder { get; }

    /// <summary>
    /// Action to configure TessEngine used in recognizion. This action is run just before recognizion.
    /// </summary>
    Action<ITessEngineConfigurable>? EngineConfiguration { set; }

    /// <summary>
    /// Engine mode Tesseract should use, for example lstm or tesseract
    /// </summary>
    EngineMode EngineMode { get; set; }

    /// <summary>
    /// Format text output should be provided in.
    /// </summary>
    TextFormat OutputFormat { get; set; }

    /// <summary>
    /// Page number to be used when getting output in special formats.
    /// </summary>
    int PageNumber { get; set; }

}
