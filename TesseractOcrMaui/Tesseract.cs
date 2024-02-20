using System.Runtime.Versioning;
using TesseractOcrMaui.Exceptions;
using TesseractOcrMaui.Results;
using TesseractOcrMaui.Tessdata;

namespace TesseractOcrMaui;

/// <summary>
/// High-level functionality with Tesseract ocr. Default implementation for ITesseract interface.
/// </summary>
[UnsupportedOSPlatform("MACCATALYST")]
public class Tesseract : ITesseract
{
    /// <summary>
    /// New instance of High-level functionality with Tesseract ocr.
    /// </summary>
    /// <param name="tessDataProvider">Interface object used to provide needed paths to recognizion process.</param>
    /// <param name="logger"></param>
    public Tesseract(ITessDataProvider tessDataProvider, ILogger<ITesseract>? logger)
    {
        _tessDataProvider = tessDataProvider;
        _logger = logger ?? NullLogger<ITesseract>.Instance;
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
        _tessDataProvider = new TessDataProvider(traineddataCollection, new TessDataProviderConfiguration()
        {
            TessDataFolder = tessdataFolder
        });
        _logger = logger ?? NullLogger<ITesseract>.Instance;
    }

    /// <inheritdoc/>
    public string TessDataFolder => _tessDataProvider.TessDataFolder;

    /// <inheritdoc/>
    public Action<ITessEngineConfigurable>? EngineConfiguration { get; set; }

    /// <inheritdoc/>
    public EngineMode EngineMode { get; set; } = EngineMode.Default;

    readonly ITessDataProvider _tessDataProvider;
    readonly ILogger<ITesseract> _logger;

    /// <inheritdoc/>
    public async Task<DataLoadResult> LoadTraineddataAsync()
    {
        var result = await _tessDataProvider.LoadFromPackagesAsync();
        result.LogLoadErrorsIfNotAllSuccess(_logger);
        return result;
    }

    /// <inheritdoc/>
    public RecognizionResult RecognizeText(string imagePath)
    {
        _logger.LogInformation("Tesseract, recognize image from path '{path}'.", imagePath);
        if (File.Exists(imagePath) is false)
        {
            _logger.LogWarning("Cannot recognize text in '{path}', file does not exist.", imagePath);
            return new RecognizionResult
            {
                Status = RecognizionStatus.ImageNotFound,
                Message = "Image does not exist."
            };
        }
        try
        {
            using Pix pix = Pix.LoadFromFile(imagePath);
            return Recognize(pix, TessDataFolder, _tessDataProvider.GetAllFileNames());
        }
        catch (IOException)
        {
            _logger.LogInformation("Cannot load pix from image path.");
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
        _logger.LogInformation("Tesseract, recognize byte image.");
        try
        {
            using Pix pix = Pix.LoadFromMemory(imageBytes);
            return Recognize(pix, TessDataFolder, _tessDataProvider.GetAllFileNames());
        }
        catch (IOException)
        {
            _logger.LogInformation("Cannot load pix from memory. Make sure your image is in right format.");
            return new RecognizionResult
            {
                Status = RecognizionStatus.InvalidImage,
                Message = "Invalid image, cannot be loaded",
            };
        }
        catch (KnownIssueException ex)
        {
            _logger.LogWarning("Cannot load pix from memory. '{ex}'", ex);
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
        _logger.LogInformation("Tesseract, recognize pix image.");
        return Recognize(image, TessDataFolder, _tessDataProvider.GetAllFileNames());
    }


    /// <inheritdoc/>
    public async Task<RecognizionResult> RecognizeTextAsync(string imagePath)
    {
        _logger.LogInformation("Tesseract, recognize image '{path}' async.", imagePath);

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
            return await Task.Run(() => Recognize(pix, TessDataFolder, _tessDataProvider.AvailableLanguages));
        }
        catch (IOException)
        {
            _logger.LogInformation("Cannot load pix from image path.");
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
        _logger.LogInformation("Tesseract, recognize byte image async.");
        var loaded = await LoadTraineddataIfNotLoadedAsync();
        if (loaded.NotSuccess())
        {
            return loaded;
        }
        try
        {
            using Pix pix = Pix.LoadFromMemory(imageBytes);
            return await Task.Run(() => Recognize(pix, TessDataFolder, _tessDataProvider.GetAllFileNames()));
        }
        catch (IOException)
        {
            _logger.LogInformation("Cannot load pix from memory. Make sure your image is in right format.");
            return new RecognizionResult
            {
                Status = RecognizionStatus.InvalidImage,
                Message = "Invalid image, cannot be loaded",
            };
        }
        catch (KnownIssueException ex)
        {
            _logger.LogWarning("Cannot load pix from memory. '{ex}'", ex);
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
        _logger.LogInformation("Tesseract, recognize pix image async.");
        var loaded = await LoadTraineddataIfNotLoadedAsync();
        if (loaded.NotSuccess())
        {
            return loaded;
        }
        return Recognize(image, TessDataFolder, _tessDataProvider.GetAllFileNames());
    }

    /// <inheritdoc/>
    public string? TryGetTesseractLibVersion() => TessEngine.TryGetVersion();

    internal RecognizionResult Recognize(Pix pix, string tessDataFolder, string[] traineddataFileNames)
    {
        var (traineddataStatus, languages) = TraineddataToLanguage(tessDataFolder, traineddataFileNames);
        if (traineddataStatus.NotSuccess())
        {
            return new RecognizionResult
            {
                Status = traineddataStatus,
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
            _logger.LogWarning("Tessdata folder is set to empty path => use 'assemblyLocation/tessdata'.");
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
            using var engine = new TessEngine(languages, tessDataFolder, EngineMode,
                new Dictionary<string, object>(), _logger);

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
        catch (DllNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            (RecognizionStatus status, string message) = ex switch
            {
                PageNotDisposedException => (RecognizionStatus.ImageAlredyProcessed,
                    "Old image TessPage must be disposed after one (1) use"),
                ImageRecognizionException => (RecognizionStatus.CannotRecognizeText,
                    "Native library failed to recognize image"),
                InvalidBytesException => (RecognizionStatus.CannotRecognizeText,
                    "Invalid bytes in recognized text, see inner exception"),
                TesseractInitException => (RecognizionStatus.Failed,
                    "Invalid data to init Tesseract Engine, see exception"),
                StringMarshallingException => (RecognizionStatus.InvalidResultString,
                    "Native library returned invalid string, please file bug report with input image as attachment"),
                TesseractException => (RecognizionStatus.CannotRecognizeText,
                    "Library cannot get thresholded image when recognizing"),
                ArgumentException => (RecognizionStatus.InvalidImage,
                    "Cannot process Pix image, height or width has invalid values."),
                Exception => (RecognizionStatus.UnknowError,
                    $"Failed to ocr for unknown reason '{ex.GetType().Name}': '{ex.Message}'.")
            };
            return new RecognizionResult
            {
                Status = status,
                Message = message,
                Exception = ex
            };
        }


        _logger.LogInformation("Recognized image with confidence '{value}'", confidence);
        _logger.LogInformation("Image contained text with length '{value}'", text?.Length ?? 0);
        return new RecognizionResult
        {
            Status = RecognizionStatus.Success,
            RecognisedText = text,
            Confidence = confidence,
        };
    }

    private async Task<RecognizionResult> LoadTraineddataIfNotLoadedAsync()
    {
        if (_tessDataProvider.IsAllDataLoaded is false)
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
    private static (RecognizionStatus, string?) TraineddataToLanguage(in string tessdataFolder, params string[] traineddataFileNames)
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
}
