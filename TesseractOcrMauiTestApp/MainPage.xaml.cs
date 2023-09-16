using Microsoft.Extensions.Logging;
using TesseractOcrMaui;
using TesseractOcrMaui.Extensions;
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


        
        // Give output (Not important)
        fileModeLabel.Text = $"File mode: FromPath";
        if (result.NotSuccess())
        {
            confidenceLabel.Text = $"Confidence: -1";
            resultLabel.Text = $"Recognizion failed: {result.Status}";
            return;
        }
        confidenceLabel.Text = $"Confidence: {result.Confidence}";
        resultLabel.Text = result.RecognisedText;
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
        
        
        // Give output (Not important)
        fileModeLabel.Text = $"File mode: FromBytes";
        if (result.NotSuccess())
        {
            confidenceLabel.Text = $"Confidence: -1";
            resultLabel.Text = $"Recognizion failed: {result.Status}";
            return;
        }
        confidenceLabel.Text = $"Confidence: {result.Confidence}";
        resultLabel.Text = result.RecognisedText;
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

    
}

