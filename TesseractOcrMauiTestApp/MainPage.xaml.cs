using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using TesseractOcrMaui;
using TesseractOcrMaui.Enums;
using TesseractOcrMaui.Results;
using TesseractOcrMaui.Tessdata;
#nullable enable
namespace TesseractOcrMauiTestApp;

public partial class MainPage : ContentPage
{
    readonly ITessDataProvider _provider;
    private readonly ResultIteratorExample _resultIteratorExample;

    public MainPage(ITesseract tesseract, ILogger<MainPage> logger, ITessDataProvider provider, ResultIteratorExample resultIteratorExample)
    {


        InitializeComponent();
        Tesseract = tesseract;
        _provider = provider;
        logger.LogInformation($"--------------------------------");
        logger.LogInformation($"-   {nameof(TesseractOcrMaui)} Demo   -");
        logger.LogInformation($"--------------------------------");

        _resultIteratorExample = resultIteratorExample;
        var rid = RuntimeInformation.RuntimeIdentifier;
        logger.LogInformation("Running on rid '{rid}'", rid);
    }

    ITesseract Tesseract { get; }

    private async void DEMO_Recognize_AsImage(object sender, EventArgs e)
    {
        // Select image (Not important)
        var path = await ImageSelecter.LetUserSelect();
        if (path is null)
        {
            return;
        }

        // Recognize image 
        var result = await Tesseract.RecognizeTextAsync(path);

        // Show output (Not important)
        ShowOutput("FromPath", result);
    }

    private async void DEMO_Recognize_AsBytes(object sender, EventArgs e)
    {
        // Select image (Not important)
        var path = await ImageSelecter.LetUserSelect();
        if (path is null)
        {
            return;
        }

        // File to byte array (Use your own way)
        using FileStream stream = new(path, FileMode.Open, FileAccess.Read);
        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer);

        // recognize bytes
        var result = await Tesseract.RecognizeTextAsync(buffer);


        // Show output (Not important)
        ShowOutput("FromBytes", result);
    }

    private async void DEMO_Recognize_AsConfigured(object sender, EventArgs e)
    {
        // Select image (Not important)
        var path = await ImageSelecter.LetUserSelect();
        if (path is null)
        {
            return;
        }

        Tesseract.EngineConfiguration = (engine) =>
        {
            // Engine uses DefaultSegmentationMode, if no other is passed as method parameter.
            // If ITesseract is injected to page, this is only way of setting PageSegmentationMode.
            // PageSegmentationMode defines how ocr tries to look for text, for example singe character or single word.
            // By default uses PageSegmentationMode.Auto.
            engine.DefaultSegmentationMode = PageSegmentationMode.SingleWord;

            engine.SetCharacterWhitelist("abcdefgh");   // These characters ocr is looking for
            engine.SetCharacterBlacklist("abc");        // These characters ocr is not looking for
            // Now ocr should be only finding characters 'defgh'
            // You can also notice that setting character listing will set ocr confidence to 0

        };

        // You can also set engine mode by uncommenting line below
        //Tesseract.EngineMode = TesseractOcrMaui.Enums.EngineMode.TesseractOnly;

        // Recognize image 
        var result = await Tesseract.RecognizeTextAsync(path);


        // For this example I reset engine configuration, because same object is used in other examples
        Tesseract.EngineConfiguration = null;

        // Show output (Not important)
        ShowOutput("FromPath, Configured", result);

    }


    private async void DEMO_GetVersion(object sender, EventArgs e)
    {
        string version = Tesseract.TryGetTesseractLibVersion() ?? "Failed";
        await DisplayAlert("Tesseract version", version, "OK");
    }


    // Not important for package 

    private void ShowOutput(string imageMode, RecognizionResult result)
    {
        // Show output (Not important)
        fileModeLabel.Text = $"File mode: {imageMode}";
        if (result.NotSuccess())
        {
            confidenceLabel.Text = $"Confidence: -1";
            resultLabel.Text = $"Recognizion failed: {result.Status}";
            return;
        }
        confidenceLabel.Text = $"Confidence: {result.Confidence}";
        resultLabel.Text = result.RecognisedText;
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        _ = await _resultIteratorExample.GetImageTextLines();
    }
}

