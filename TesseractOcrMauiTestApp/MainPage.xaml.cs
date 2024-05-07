using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using TesseractOcrMaui;
using TesseractOcrMaui.Enums;
using TesseractOcrMaui.Imaging;
using TesseractOcrMaui.Iterables;
using TesseractOcrMaui.Results;
#nullable enable
namespace TesseractOcrMauiTestApp;

public partial class MainPage : ContentPage
{
#if !IOS && DEBUG
    public MainPage(ITesseract tesseract, TesseractTestClass testClass)
    {
        InitializeComponent();
        Tesseract = tesseract;
        TestClass = testClass;
    }
#else
    public MainPage(ITesseract tesseract, ILogger<MainPage> logger)
    {
        InitializeComponent();
        Tesseract = tesseract;

        logger.LogInformation($"--------------------------------");
        logger.LogInformation($"-   {nameof(TesseractOcrMaui)} Demo   -");
        logger.LogInformation($"--------------------------------");

        var rid = RuntimeInformation.RuntimeIdentifier;
        logger.LogInformation("Running on rid '{rid}'", rid);


    }
#endif

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

    private static async Task<string?> GetUserSelectedPath()
    {
#if IOS
        var pickResult = await MediaPicker.PickPhotoAsync(new MediaPickerOptions()
        {
            Title = "Pick jpeg or png image"
        });
#else
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
#endif
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

#if !IOS
    public TesseractTestClass TestClass { get; }

    private async void GraphicsView_Loaded(object sender, EventArgs e)
    {

        await TestClass.Load();

        //if (sender is GraphicsView graphicsView)
        //{

        //    Drawable canvas = new(TestClass);

        //    canvas.Drawn += (_, _) =>
        //    {
        //        grid.HeightRequest = canvas.ImageHeight;
        //        grid.WidthRequest = canvas.ImageWidth;

        //        label.Text = string.Join(' ', canvas._lines);
        //    };

        //    graphicsView.Drawable = canvas;
        //}
        string imagePath = @"C:\Users\henri\Downloads\clearTextImage.png";
        using var pix = Pix.LoadFromFile(imagePath);
        using var iter = new BlockIterable(TestClass.Languages!, TestClass.TessDataFolder!, pix,
            PageIteratorLevel.TextLine, PageIteratorLevel.Word
            );

        List<BlockLevelCollection> blocks = new();
        foreach (var item in iter)
        {
            blocks.Add(item);
        }

        string json = JsonSerializer.Serialize(blocks, new JsonSerializerOptions
        {
            WriteIndented = true,
        });
    }

    class Drawable : IDrawable
    {
        public Drawable(TesseractTestClass cls)
        {
            _cls = cls;
        }

        public event EventHandler? Drawn;

        readonly TesseractTestClass _cls;

        public int ImageHeight { get; private set; }
        public int ImageWidth { get; private set; }

        readonly List<BoundingBox> _data = new();

        internal readonly List<string> _lines = new();

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Red;

            if (_data.Count is 0)
            {
                string imagePath = @"C:\Users\henri\Downloads\clearTextImage.png";
                using var pix = Pix.LoadFromFile(imagePath);
                using var iter = new TextMetadataIterable(_cls.Languages!, _cls.TessDataFolder!, pix);

                ImageHeight = iter.ImageHeight;
                ImageWidth = iter.ImageWidth;

                foreach (var (text, layout) in iter)
                {
                    _data.Add(layout.Box);
                    _lines.Add(text.Text);
                }
                Drawn?.Invoke(this, EventArgs.Empty);
            }

            foreach (BoundingBox box in _data)
            {
                int width = box.X2 - box.X1;
                int height = box.Y2 - box.Y1;
                canvas.DrawRectangle(box.X1, box.Y1, width, height);
            }
        }
    }
#endif
}

