using Microsoft.Extensions.Logging;
using TesseractOcrMaui;
using TesseractOcrMaui.Results;
#nullable enable
namespace TesseractOcrMauiTestApp;

public partial class MainPage : ContentPage
{
    public MainPage(ITesseract tesseract, ILogger<MainPage> logger)
    {
        InitializeComponent();
        Tesseract = tesseract;

        logger.LogInformation($"--------------------------------");
        logger.LogInformation($"-   {nameof(TesseractOcrMaui)} Demo   -");
        logger.LogInformation($"--------------------------------");
    }

    ITesseract Tesseract { get; }

    // This class includes examples of using the TesseractOcrMaui library.

    private async void DEMO_Recognize_AsImage(object sender, EventArgs e)
    {
        // Select image (Not important)
        var path = await GetUserSelectedPath();
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
        var path = await GetUserSelectedPath();
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
        var path = await GetUserSelectedPath();
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
            engine.DefaultSegmentationMode = TesseractOcrMaui.Enums.PageSegmentationMode.SingleWord;
            
            engine.SetCharacterWhitelist("abcdefgh");   // These characters ocr is looking for
            engine.SetCharacterBlacklist("abc");        // These characters ocr is not looking for
            // Now ocr should be only finding characters 'defgh'

        };

        Tesseract.EngineMode = TesseractOcrMaui.Enums.EngineMode.TesseractOnly;

        // Recognize image 
        var result = await Tesseract.RecognizeTextAsync(path);



        // For this example I reset engine configuration, because same Object is used in other examples
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

    private static async Task<string?> GetUserSelectedPath()
    {
        var pickResult = await FilePicker.PickAsync(new PickOptions()
        {
            PickerTitle = "Pick jpeg or png image",
            // Currently usable image types
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>()
            {
                [DevicePlatform.Android] = new List<string>() { "image/png", "image/jpeg" },
                [DevicePlatform.WinUI] = new List<string>() { ".png", ".jpg", ".jpeg" },
            })
        });
        return pickResult?.FullPath;
    }

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

 
}

