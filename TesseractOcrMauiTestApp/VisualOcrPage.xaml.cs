using System.Text;
using TesseractOcrMaui;
using TesseractOcrMaui.Enums;
using TesseractOcrMaui.Iterables;
using TesseractOcrMaui.Results;
using TesseractOcrMaui.Tessdata;
using BoundingBox = TesseractOcrMaui.Imaging.BoundingBox;

namespace TesseractOcrMauiTestApp;

public partial class VisualOcrPage : ContentPage
{
    private readonly ITessDataProvider _provider;

    public VisualOcrPage(ITessDataProvider provider)
    {
        InitializeComponent();
        _provider = provider;

        graphicsView.Drawable = new Drawable();
    }

    public PageIteratorLevel TextBlockSize { get; set; } = PageIteratorLevel.TextLine;

    private async void SelectImageButton_Clicked(object sender, EventArgs e)
    {
        // Load traineddata from app packages
        var dataLoaded = await _provider.LoadFromPackagesAsync();
        if (dataLoaded.NotSuccess())
        {
            throw new Exception(dataLoaded.Message);
        }

        // Let user select file, exit if user cancelled
        string? imagePath = await ImageSelecter.LetUserSelect();
        if (imagePath is null)
        {
            return;
        }

        // Get load image
        using var pix = Pix.LoadFromFile(imagePath);

        // Create iterable, Change TextBlockSize to try different sized text blocks
        using var iter = new TextMetadataIterable(pix, _provider, TextBlockSize);

        // Clear old data

        Canvas.Data.Clear();

        // Iterate
        List<TextSpan> recognizedText = new();
        foreach (var (text, layout) in iter)
        {
            Canvas.Data.Add(layout.Box);
            recognizedText.Add(text);
        }


        // Set ui image size to not have scaling problems
        ocrGrid.HeightRequest = pix.Height;
        ocrGrid.WidthRequest = pix.Width;

        // Load image, show output
        uiImage.Source = new FileImageSource
        {
            File = imagePath
        };
        ParseOutput(recognizedText);
    }



    private void ParseOutput(List<TextSpan> recognizedText)
    {
        string textSeparator = TextBlockSize switch
        {
            PageIteratorLevel.Block or
            PageIteratorLevel.Paragraph or
            PageIteratorLevel.TextLine => Environment.NewLine + Environment.NewLine,
            PageIteratorLevel.Word => " ",
            PageIteratorLevel.Symbol => " ",
            _ => " "
        };

        StringBuilder builder = new();
        foreach (var text in recognizedText)
        {
            builder.Append(text.Text.Replace('\n', ' ').Replace('\r', ' '));
            builder.Append(textSeparator);
        }
        textOutput.Text = builder.ToString();
    }


    Drawable Canvas => (Drawable)graphicsView.Drawable;

    class Drawable : IDrawable
    {
        public List<BoundingBox> Data { get; set; } = new();

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Red;

            foreach (BoundingBox box in Data)
            {
                int width = box.X2 - box.X1;
                int height = box.Y2 - box.Y1;
                canvas.DrawRectangle(box.X1, box.Y1, width, height);
            }
        }
    }
}