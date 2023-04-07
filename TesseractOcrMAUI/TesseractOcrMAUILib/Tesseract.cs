using System.Runtime.Versioning;
using TesseractOcrMAUILib.Results;
using TesseractOcrMAUILib.Tessdata;

namespace TesseractOcrMAUILib;

[UnsupportedOSPlatform("MACCATALYST")]
[UnsupportedOSPlatform("IOS")]
public class Tesseract : ITesseract
{
    public Tesseract(ITessDataProvider tessDataProvider, ILogger<ITesseract> logger)
    {
        TessDataProvider = tessDataProvider;
        Logger = logger;
    }

    public string TessDataFolder => TessDataProvider.TessDataFolder;

    ITessDataProvider TessDataProvider { get; }
    ILogger<ITesseract> Logger { get; }

    public RecognizionResult RecognizeText(string imagePath)
    {
        Logger.LogInformation("Tesseract, recognize image '{path}'.", imagePath);

        var dataLoadTask = TessDataProvider.LoadFromPackagesAsync();
        bool isLoaded = dataLoadTask.Wait(TimeSpan.FromSeconds(2));
        if (isLoaded is false)
        {
            return new RecognizionResult { Status = RecognizionStatus.CannotLoadTessData };
        }
        
        var loadResult = dataLoadTask.Result;
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
        return Recognize(tessData, fileName, imagePath);

    }
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
        return Recognize(tessData, fileName, imagePath);
    }



    internal RecognizionResult Recognize(string tessDataFolder, string traineddataFileName, string imagePath)
    {
        var language = Path.GetFileNameWithoutExtension(traineddataFileName);
        if (string.IsNullOrWhiteSpace(language))
        {
            Logger.LogWarning("Tesseract language is not definedm, cannot recognize image.");
            return new RecognizionResult { Status = RecognizionStatus.NoLanguagesAvailable };
        }
        if (File.Exists(imagePath) is false)
        {
            Logger.LogWarning("Cannot recognize text in '{path}', file does not exist.", imagePath);
            return new RecognizionResult { Status = RecognizionStatus.ImageNotFound };
        }
        if (string.IsNullOrWhiteSpace(tessDataFolder))
        {
            Logger.LogWarning("Tessdata folder is set to empty path => use 'assemblyLocation/tessdata'.");
        }

        Logger.LogInformation("Recognize image at '{path}'", imagePath);

        string? text = null;
        float confidence = -1f;
        try
        {
            using var engine = new TessEngine(language, tessDataFolder, Logger);
            using var image = Pix.LoadFromFile(imagePath);
            using var page = engine.ProcessImage(image);
            confidence = page.GetConfidence();
            text = page.GetText();
        }
        catch 
        {
            throw;
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
}
