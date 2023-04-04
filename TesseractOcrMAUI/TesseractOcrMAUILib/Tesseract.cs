using TesseractOcrMAUILib.Results;
using TesseractOcrMAUILib.Tessdata;

namespace TesseractOcrMAUILib;
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
        var task = RecognizeTextAsync(imagePath);
        task.Wait();
        return task.Result;
    }

    public async Task<RecognizionResult> RecognizeTextAsync(string imagePath)
    {
        Logger.LogInformation("Tesseract, recognize image '{path}'.", imagePath);

        await TessDataProvider.LoadFromPackagesAsync();

        var tessData = TessDataProvider.TessDataFolder;
        var language = TessDataProvider.AvailableLanguages.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(language))
        {
            throw new InvalidOperationException("Languge should not be null or empty.");
        }
        
        using var engine = new TessEngine(language, tessData);
        using var image = Pix.LoadFromFile(imagePath);
        using var page = engine.Process(image);

        float confidence = page.GetConfidence();
        string text = page.GetText();

        Logger.LogInformation("Recognized image with confidence '{value}'", confidence);
        Logger.LogInformation("Image contained text with length '{value}'", text.Length);


        return new RecognizionResult
        {
            Status = RecognizionStatus.Success,
            RecognisedText = text,
            Confidence = confidence
        };
    }
}
