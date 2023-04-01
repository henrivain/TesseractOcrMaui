using Microsoft.Maui.Handlers;
using TesseractOcrMAUILib;

namespace TesseractOcrMauiTestApp;

public partial class MainPage : ContentPage
{
    int _count = 0;

    public MainPage()
    {
        InitializeComponent();
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

    private void Label_Loaded(object sender, EventArgs e)
    {
        if (sender is Label label)
        {
            var tessData = @"C:\1 Henri\github\TesseractOcrMaui\TesseractOcrMAUI\TesseractOcrMAUILib\tessdata\";
            var imagePath = @"C:\1 Henri\github\TesseractOcrMaui\TesseractOcrMAUI\TesseractOcrMAUILib\TestImages\test1.png";
            using var engine = new TessEngine("fin", tessData);
            using var image = Pix.LoadFromFile(imagePath);
            using var page = engine.Process(image);
            var result = page.GetText();

            label.Text = result;
        }
    }
}

