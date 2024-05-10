using System.Diagnostics;
using TesseractOcrMaui;
using TesseractOcrMaui.Enums;
using TesseractOcrMaui.Iterables;
using TesseractOcrMaui.Results;
using TesseractOcrMaui.Tessdata;

namespace TesseractOcrMauiTestApp;

public partial class TextIteratorPage : ContentPage
{
    private readonly ITessDataProvider _provider;

    public TextIteratorPage(ITessDataProvider provider)
	{
		InitializeComponent();
        _provider = provider;
    }

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

        /*  TextStructureIterable 
         *  
         *  Functionality
         *  - This iterable is meant to analyze text structure.
         *  - With example configuration [highestLevel: Paragraph, lowestLevel: Word] output will be:
         *  
         *  foreach loop -> returns [paragraphs]: Loops over every text paragraph    
         *   │
         *   ├─ in every paragraph -> property [LowerLevelData]: Every text line inside this paragraph
         *   │  │
         *   │  ├─ in every textline -> property [LowerLevelData]: Every word inside this text line   
         * 
         * 
         *  Paragraph as the highest level returned is defined in "biggestBlockSize"
         *  Word as the lowest level returned is defined in "smallestBlockSize"
         * 
         * 
         *  Specification 
         *  - TextStructureIterable returns recognized text in a structure where 
         *    LowerLevelData property ALWAYS is one level lower from current BlockLevelCollection object. 
         *  - The lowest LowerLevelData that is not null is ALWAYS data layer that contains 
         *    all the string data
         *  - If user does not want to parse from for example symbol level, user can set 
         *    lowestLevel to one level up -> Word
         *  
         * 
         *  Limitations
         *  Paragraph recognition doesn't seem to be very exact. 
         *  Paragraphs seem to be separated from every line that
         *  does not continue to the image right end.
         *  
         *  String data is only stored at the lowest level so it has to rebuilt 
         *  from lower levels if user wants to access higher level string.
         *  This can be achieved with BlockLevelCollection.Build(ref confidence). 
         *  You must pass average calculating ref object to the method, 
         *  for example TesseractOcrMaui.Results.Average.
         * 
         */





        // Configure between which block size levels are analyzed

        /*  highest: Paragraph, lowest: Word 
         *  -> contains TextLine between 
         *  
         *  Paragraphs are sliced into TextLines which are sliced into Words
         *  [Paragraph -> TextLine -> Word]
         */
        PageIteratorLevel highestLevel = PageIteratorLevel.Paragraph;
        PageIteratorLevel lowestLevel = PageIteratorLevel.Word;

        // Load image and create iterator
        using var image = Pix.LoadFromFile(imagePath);
        using var iter = new TextStructureIterable(image, _provider, highestLevel, lowestLevel);

        /* Examples of visualizing data
         * These are examples how to visualize data from TextStructureIterable.
         * These are not optimised and same result can be achieved in more efficient ways.
         * Iterator user should create their own way to parse output data.
         */ 
        foreach (BlockLevelCollection block in iter)
        {
            // ACSII tree
            static void WriteLine(string value) => Debug.WriteLine(value);
            block.PrintStructureToOutput(WriteLine);

            // Build method
            //IAverage confidence = new Average();
            //string stringified = block.Build(ref confidence).ToString();
        }
    }
}