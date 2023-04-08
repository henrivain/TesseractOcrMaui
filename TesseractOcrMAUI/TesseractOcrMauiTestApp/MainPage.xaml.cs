using Microsoft.Extensions.Logging;
using TesseractOcrMAUILib;
using TesseractOcrMAUILib.Extensions;

namespace TesseractOcrMauiTestApp;

public partial class MainPage : ContentPage
{
    public MainPage(ITesseract tesseract, ILogger<MainPage> logger)
    {
        InitializeComponent();
        Tesseract = tesseract;

        logger.LogInformation($"--------------------------------");
        logger.LogInformation($"-   {nameof(TesseractOcrMAUILib)} Demo   -");
        logger.LogInformation($"--------------------------------");
    }

    ITesseract Tesseract { get; }


    private async void Button_Clicked(object sender, EventArgs e)
    {
        // Make user pick file
        var pickResult = await FilePicker.PickAsync(new PickOptions()
        {
            PickerTitle = "Pick png image",
            // Currently usable image types
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>()
            {
                [DevicePlatform.Android] = new List<string>() { "image/png", "image/jpeg" },
                [DevicePlatform.WinUI] = new List<string>() { ".png", ".jpg", ".jpeg" },
            })
        });
        // null if user cancelled the operation
        if (pickResult is null)
        {
            return;
        }

        // Recognize image 
        var result = await Tesseract.RecognizeTextAsync(pickResult.FullPath);

        // Show output
        confidenceLabel.Text = $"Confidence: {result.Confidence}";
        if (result.NotSuccess())
        {
            resultLabel.Text = $"Recognizion failed: {result.Status}";
            return;
        }
        resultLabel.Text = result.RecognisedText;
    }
}

