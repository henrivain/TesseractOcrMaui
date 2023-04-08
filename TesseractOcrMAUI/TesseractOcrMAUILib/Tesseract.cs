using IntelliJ.Lang.Annotations;
using MauiTesseractOcr.Results;
using MauiTesseractOcr.Tessdata;
using System.Runtime.Versioning;

namespace MauiTesseractOcr;

/// <summary>
/// High-level functionality with Tesseract ocr. Default implementation for ITesseract interface.
/// </summary>
[UnsupportedOSPlatform("MACCATALYST")]
[UnsupportedOSPlatform("IOS")]
public class Tesseract : ITesseract
{
    /// <summary>
    /// New instance of High-level functionality with Tesseract ocr.
    /// </summary>
    /// <param name="tessDataProvider"></param>
    /// <param name="logger"></param>
    public Tesseract(ITessDataProvider tessDataProvider, ILogger<ITesseract> logger)
    {
        TessDataProvider = tessDataProvider;
        Logger = logger;
    }

    /// <summary>
    /// Path to Traineddata folder provided by ITessDataProvider.
    /// </summary>
    public string TessDataFolder => TessDataProvider.TessDataFolder;

    ITessDataProvider TessDataProvider { get; }
    ILogger<ITesseract> Logger { get; }

    /// <summary>
    /// Load 
    /// </summary>
    /// <returns></returns>
    public async Task<DataLoadResult> LoadTraineddataAsync()
    {
        return await TessDataProvider.LoadFromPackagesAsync();
    }

    /// <inheritdoc/>
    public RecognizionResult RecognizeText(string imagePath)
    {
        Logger.LogInformation("Tesseract, recognize image '{path}'.", imagePath);

        var validFiles = GetTraineddataFilesThatExist();
        if (validFiles.Length < 1)
        {
            return new RecognizionResult
            {
                Status = RecognizionStatus.TraineddataNotLoaded,
                Message = "You must load traineddata files before running this method."
            };
        }
        var tessData = TessDataProvider.TessDataFolder;
        var fileName = validFiles.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return new RecognizionResult { Status = RecognizionStatus.NoLanguagesAvailable };
        };
        return Recognize(tessData, fileName, imagePath);

    }

    /// <inheritdoc/>
    public async Task<RecognizionResult> RecognizeTextAsync(string imagePath)
    {
        var loadResult = await TessDataProvider.LoadFromPackagesAsync();
        var tessData = TessDataProvider.TessDataFolder;
        var fileName = TessDataProvider.AvailableLanguages.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return new RecognizionResult { Status = RecognizionStatus.NoLanguagesAvailable };
        };
        if (loadResult.NotSuccess())
        {
            return new RecognizionResult { Status = RecognizionStatus.CannotLoadTessData };
        }
        return await Task.Run(() => Recognize(tessData, fileName, imagePath));
    }

    internal RecognizionResult Recognize(string tessDataFolder, string traineddataFileName, string imagePath)
    {
        if (traineddataFileName is null)
        {
            Logger.LogWarning("Tesseract language is not definedm, cannot recognize image.");
            return new RecognizionResult { Status = RecognizionStatus.NoLanguagesAvailable };
        }
        var language = Path.GetFileNameWithoutExtension(traineddataFileName);
        if (string.IsNullOrWhiteSpace(language))
        {
            Logger.LogWarning("Tesseract language is not definedm, cannot recognize image.");
            return new RecognizionResult 
            { 
                Status = RecognizionStatus.NoLanguagesAvailable,
                Message = "Language not provided."
            };
        }
        if (File.Exists(imagePath) is false)
        {
            Logger.LogWarning("Cannot recognize text in '{path}', file does not exist.", imagePath);
            return new RecognizionResult 
            { 
                Status = RecognizionStatus.ImageNotFound,
                Message = "Image does not exist."
            };
        }
        if (string.IsNullOrWhiteSpace(tessDataFolder))
        {
            Logger.LogWarning("Tessdata folder is set to empty path => use 'assemblyLocation/tessdata'.");
            return new RecognizionResult
            {
                Status = RecognizionStatus.TessDataFolderNotProvided,
                Message = "Traineddata folder was null or empty."
            };
        }

        Logger.LogInformation("Recognize image at '{path}'", imagePath);

        string? text = null;
        float confidence = -1f;

        // Recognize and catch exceptions
        try
        {
            // nulls are alredy checked, can't throw.
            using var engine = new TessEngine(language, tessDataFolder, Logger);    
            using var image = Pix.LoadFromFile(imagePath);
            
            // image can't be null here
            using var page = engine.ProcessImage(image);
            
            // SegMode can't be OsdOnly in here.
            confidence = page.GetConfidence();

            // SegMode can't be OsdOnly in here.
            text = page.GetText();
        }
        catch (IOException)
        {
            return new()
            {
                Status = RecognizionStatus.ImageNotFound,
                Message = $"Image cannot be loaded from '{imagePath}'."
            };
        }
        catch (ArgumentException)
        {
            return new()
            {
                Status = RecognizionStatus.InvalidImage,
                Message = "Cannot process Pix image, height or width has invalid value."
            };
        }
        catch (InvalidOperationException)
        {
            return new()
            {
                Status = RecognizionStatus.ImageAlredyProcessed,
                Message = "Image must be disposed after use."
            };
        }
        catch (ImageRecognizionException)
        {
            return new()
            {
                Status = RecognizionStatus.CannotRecognizeText,
                Message = "Library cannot recognize image for some reason."
            };
        }
        catch (InvalidBytesException ex)
        {
            return new()
            {
                Status = RecognizionStatus.CannotRecognizeText,
                Message = $"Recognized text contained invalid bytes. " +
                $"See inner exception '{ex.GetType().Name}', '{ex.Message}'."
            };
        }
        catch (TesseractException)
        {
            return new()
            {
                Status = RecognizionStatus.CannotRecognizeText,
                Message = "Library cannot thresholded image."
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                Status = RecognizionStatus.UnknowError,
                Message = $"Failed to ocr for unknown reason '{ex.GetType().Name}': '{ex.Message}'."
            };
        }


        Logger.LogInformation("Recognized image with confidence '{value}'", confidence);
        Logger.LogInformation("Image contained text with length '{value}'", text?.Length ?? 0);
        return new RecognizionResult
        {
            Status = RecognizionStatus.Success,
            RecognisedText = text,
            Confidence = confidence,
        };
    }


    private string[] GetTraineddataFilesThatExist()
    {
        Logger.LogInformation("Check if tessdata is alredy loaded.");

        var folder = TessDataProvider.TessDataFolder;
        return TessDataProvider
            .GetAllFileNames()
            .Where(x => x is not null)
            .Where(x => File.Exists(Path.Combine(folder, x)))
            .ToArray();
        
    }
}
