using System.Runtime.Versioning;
using TesseractOcrMaui.Results;
using TesseractOcrMaui.Tessdata;

namespace TesseractOcrMaui;

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
    /// <param name="tessDataProvider">Interface object used to provide needed paths to recognizion process.</param>
    /// <param name="logger"></param>
    public Tesseract(ITessDataProvider tessDataProvider, ILogger<ITesseract>? logger)
    {
        TessDataProvider = tessDataProvider;
        Logger = logger ?? NullLogger<ITesseract>.Instance;
    }

    /// <summary>
    /// New instance of High-level functionality with Tesseract ocr.
    /// </summary>
    /// <param name="tessdataFolder">Path to tessdata folder.</param>
    /// <param name="traineddataFileNames">Array of traineddata file names that exist in tessdata folder. Include extensions, not path.</param>
    /// <param name="logger">Logger to be used when running recognizion and other processes.</param>
    /// <exception cref="ArgumentNullException">
    /// If traineddata file name is null or empty 
    /// OR tessdataFolder is null.
    /// </exception>
    public Tesseract(string tessdataFolder, string[] traineddataFileNames, ILogger<ITesseract>? logger = null)
    {
        if (tessdataFolder is null)
        {
            throw new ArgumentNullException(nameof(tessdataFolder));
        }

        var traineddataCollection = new TrainedDataCollection();
        foreach (var file in traineddataFileNames)
        {
            traineddataCollection.AddNonPackageFile(file);
        }
        TessDataProvider = new TessDataProvider(traineddataCollection, new TessDataProviderConfiguration()
        {
            TessDataFolder = tessdataFolder
        });
        Logger = logger ?? NullLogger<ITesseract>.Instance;
    }

    /// <inheritdoc/>
    public string TessDataFolder => TessDataProvider.TessDataFolder;

    /// <inheritdoc/>
    public Action<TessEngine>? EngineConfiguration { get; set; }


    ITessDataProvider TessDataProvider { get; }
    ILogger<ITesseract> Logger { get; }

    /// <inheritdoc/>
    public async Task<DataLoadResult> LoadTraineddataAsync()
    {
        var result = await TessDataProvider.LoadFromPackagesAsync();
        result.LogLoadErrorsIfNotAllSuccess(Logger);
        return result;
    }


    
    /// <inheritdoc/>
    public RecognizionResult RecognizeText(string imagePath)
    {
        Logger.LogInformation("Tesseract, recognize image from path '{path}'.", imagePath);
        if (File.Exists(imagePath) is false)
        {
            Logger.LogWarning("Cannot recognize text in '{path}', file does not exist.", imagePath);
            return new RecognizionResult
            {
                Status = RecognizionStatus.ImageNotFound,
                Message = "Image does not exist."
            };
        }
        try
        {
            using Pix pix = Pix.LoadFromFile(imagePath);
            return Recognize(pix, TessDataFolder, TessDataProvider.GetAllFileNames());
        }
        catch (IOException)
        {
            Logger.LogInformation("Cannot load pix from image path.");
            return new()
            {
                Status = RecognizionStatus.InvalidImage,
                Message = "Invalid image, cannot be loaded"
            };
        }
    }

    /// <inheritdoc/>
    public RecognizionResult RecognizeText(byte[] imageBytes)
    {
        Logger.LogInformation("Tesseract, recognize byte image.");
        try
        {
            using Pix pix = Pix.LoadFromMemory(imageBytes);
            return Recognize(pix, TessDataFolder, TessDataProvider.GetAllFileNames());
        }
        catch (IOException)
        {
            Logger.LogInformation("Cannot load pix from memory. Make sure your image is in right format.");
            return new RecognizionResult
            {
                Status = RecognizionStatus.InvalidImage,
                Message = "Invalid image, cannot be loaded",
            };
        }
        catch (KnownIssueException ex)
        {
            Logger.LogWarning("Cannot load pix from memory. '{ex}'", ex);
            return new RecognizionResult
            {
                Status = RecognizionStatus.InvalidImage,
                Message = $"Cannot load pix from memory. (This is a known issue, see '{ex.HelpLink}'.)"
            };
        }
    }

    /// <inheritdoc/>
    public RecognizionResult RecognizeText(Pix image)
    {
        Logger.LogInformation("Tesseract, recognize pix image.");
        return Recognize(image, TessDataFolder, TessDataProvider.GetAllFileNames());
    }


    /// <inheritdoc/>
    public async Task<RecognizionResult> RecognizeTextAsync(string imagePath)
    {
        Logger.LogInformation("Tesseract, recognize image '{path}' async.", imagePath);

        // Load traineddata if all files not loaded yet.
        var loaded = await LoadTraineddataIfNotLoadedAsync();
        if (loaded.NotSuccess())
        {
            return loaded;
        }
        if (File.Exists(imagePath) is false)
        {
            return new RecognizionResult
            {
                Status = RecognizionStatus.ImageNotFound,
                Message = "Image does not exist."
            };
        }
        try
        {
            using Pix pix = Pix.LoadFromFile(imagePath);
            return await Task.Run(() => Recognize(pix, TessDataFolder, TessDataProvider.AvailableLanguages));
        }
        catch (IOException)
        {
            Logger.LogInformation("Cannot load pix from image path.");
            return new()
            {
                Status = RecognizionStatus.InvalidImage,
                Message = "Invalid image, cannot be loaded"
            };
        }
    }

    /// <inheritdoc/>
    public async Task<RecognizionResult> RecognizeTextAsync(byte[] imageBytes)
    {
        Logger.LogInformation("Tesseract, recognize byte image async.");
        var loaded = await LoadTraineddataIfNotLoadedAsync();
        if (loaded.NotSuccess())
        {
            return loaded;
        }
        try
        {
            using Pix pix = Pix.LoadFromMemory(imageBytes);
            return await Task.Run(() => Recognize(pix, TessDataFolder, TessDataProvider.GetAllFileNames()));
        }
        catch (IOException)
        {
            Logger.LogInformation("Cannot load pix from memory. Make sure your image is in right format.");
            return new RecognizionResult
            {
                Status = RecognizionStatus.InvalidImage,
                Message = "Invalid image, cannot be loaded",
            };
        }
        catch (KnownIssueException ex) 
        {
            Logger.LogWarning("Cannot load pix from memory. '{ex}'", ex);
            return new RecognizionResult
            {
                Status = RecognizionStatus.InvalidImage,
                Message = $"Cannot load pix from memory. (This is a known issue, see '{ex.ReferringLink}'.)"
            };
        }
    }

    /// <inheritdoc/>
    public async Task<RecognizionResult> RecognizeTextAsync(Pix image)
    {
        Logger.LogInformation("Tesseract, recognize pix image async.");
        var loaded = await LoadTraineddataIfNotLoadedAsync();
        if (loaded.NotSuccess())
        {
            return loaded;
        }
        return Recognize(image, TessDataFolder, TessDataProvider.GetAllFileNames());
    }



    
    internal RecognizionResult Recognize(Pix pix, string tessDataFolder, string[] traineddataFileNames)
    {
        var (status, languages) = TrainedDataToLanguage(tessDataFolder, traineddataFileNames);
        if (status.NotSuccess())
        {
            return new RecognizionResult
            {
                Status = status,
                Message = "Failed to load traineddata files. See status to find reason."
            };
        }
        if (languages is null)
        {
            return new RecognizionResult
            {
                Status = RecognizionStatus.NoLanguagesAvailable,
                Message = "All languages given are invalid."
            };
        }
        if (string.IsNullOrWhiteSpace(tessDataFolder))
        {
            Logger.LogWarning("Tessdata folder is set to empty path => use 'assemblyLocation/tessdata'.");
            tessDataFolder ??= "";
        }
        if (pix is null)
        {
            return new RecognizionResult
            {
                Status = RecognizionStatus.ImageNotFound,
                Message = "Pix image cannot null"
            };
        }

        string? text = null;
        float confidence = -1f;

        try
        {
            // nulls are alredy checked, can't throw.
            using var engine = new TessEngine(languages, tessDataFolder, Logger);

            if (EngineConfiguration is not null)
            {
                EngineConfiguration(engine);
            }

            using var page = engine.ProcessImage(pix);

            // SegMode can't be OsdOnly in here.
            confidence = page.GetConfidence();

            // SegMode can't be OsdOnly in here.
            text = page.GetText();
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
                Message = "Image must be disposed after one (1) use."
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
        catch (TesseractInitException ex)
        {
            return new()
            {
                Status = RecognizionStatus.Failed,
                Message = "Cannot initialize instance of Tesseract engine, because of invalid data. "
                    + ex.InnerException is null ? ex.Message
                        : $"'{ex.Message}': '{ex.InnerException?.Message}'"
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
        catch (DllNotFoundException)
        {
            throw;
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

    private async Task<RecognizionResult> LoadTraineddataIfNotLoadedAsync()
    {
        if (TessDataProvider.IsAllDataLoaded is false)
        {
            var loadResult = await LoadTraineddataAsync();
            if (loadResult.NotSuccess())
            {
                return new RecognizionResult
                {
                    Status = RecognizionStatus.CannotLoadTessData,
                    Message = $"Failed to load '{loadResult.GetErrorCount()}' traineddata files. " +
                        $"Errors: '{loadResult.GetErrorsString()}'"
                };
            }
        }
        return RecognizionResult.InProgress;
    }

    /// <summary>
    /// Traineddata file array to validated '+' separated languages list. 
    /// </summary>
    /// <param name="tessdataFolder"></param>
    /// <param name="traineddataFileNames"></param>
    /// <returns>(null, ErrorResult) if failed, othewise (lang, null).</returns>
    private static (RecognizionStatus, string?) TrainedDataToLanguage(in string tessdataFolder, params string[] traineddataFileNames)
    {
        if (string.IsNullOrWhiteSpace(tessdataFolder))
        {
            return (RecognizionStatus.TessDataFolderNotProvided, null);
        }

        // Check if any file valid
        List<string> languages = new();
        foreach (var file in traineddataFileNames)
        {
            // not empty/null
            if (string.IsNullOrWhiteSpace(file))
            {
                continue;
            }
            // file not exists in file system
            string filePath = Path.Combine(tessdataFolder, file);
            if (File.Exists(filePath) is false)
            {
                continue;
            }
            // language in file name is invalid
            var lang = Path.GetFileNameWithoutExtension(file);
            if (string.IsNullOrWhiteSpace(lang))
            {
                continue;
            }
            languages.Add(lang);
        }
        if (languages.Count <= 0)
        {
            return (RecognizionStatus.NoLanguagesAvailable, null);
        }
        return (RecognizionStatus.InProgressSuccess, string.Join('+', languages));
    }

    /// <inheritdoc/>
    public string? TryGetTesseractLibVersion() => TessEngine.TryGetVersion();
}
