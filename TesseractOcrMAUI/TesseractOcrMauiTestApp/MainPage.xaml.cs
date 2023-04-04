using TesseractOcrMAUILib;

namespace TesseractOcrMauiTestApp;

public partial class MainPage : ContentPage
{
    int _count = 0;


    public MainPage(ITesseract tesseract)
    {
        InitializeComponent();
        Tesseract = tesseract;
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        _count++;

        if (_count == 1)
            CounterBtn.Text = $"Clicked {_count} time";
        else
            CounterBtn.Text = $"Clicked {_count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }


    ITesseract Tesseract { get; }

    private void Label_Loaded(object sender, EventArgs e)
    {
        if (sender is Label label)
        {
            var imagePath = @"C:\1 Henri\github\TesseractOcrMaui\TesseractOcrMAUI\TesseractOcrMAUILib\TestImages\test1.png";

            //string result = string.Empty;
            //var tessData = @"C:\1 Henri\github\TesseractOcrMaui\TesseractOcrMAUI\TesseractOcrMAUILib\Tessdata\";
            //using var engine = new TessEngine("fin", tessData);
            //using var image = Pix.LoadFromFile(imagePath);
            //using var page = engine.Process(image);
            //result = page.GetText();

            var result = Tesseract.RecognizeText(imagePath);
            
            label.Text = result.RecognisedText;
        }
    }
}

