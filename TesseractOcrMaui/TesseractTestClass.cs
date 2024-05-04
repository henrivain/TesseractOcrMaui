#if DEBUG
#if !IOS
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Diagnostics;
using TesseractOcrMaui.Iterables;
using TesseractOcrMaui.Results;
using TesseractOcrMaui.Tessdata;


namespace TesseractOcrMaui;


public class TesseractTestClass
{
    private readonly ITessDataProvider _tessDataProvider;
    private readonly ILogger<TesseractTestClass> _logger;

    public TesseractTestClass(ITessDataProvider tessDataProvider, ILogger<TesseractTestClass>? logger)
    {
        _tessDataProvider = tessDataProvider;
        _logger = logger ?? NullLogger<TesseractTestClass>.Instance;
    }

    public async void RunAsync()
    {
        // Load tessdata for testing purposes, configure in MauiProgram.cs, do not touch here
        var result = await _tessDataProvider.LoadFromPackagesAsync();
        string tessDataFolder = _tessDataProvider.TessDataFolder;
        string languages = string.Join('+', _tessDataProvider.AvailableLanguages.Select(x => x.Replace(".traineddata", "")));

        // [INPUT] give image here
        //string imagePath = @"C:\Users\henri\Downloads\tess version wsl.png";
        string imagePath = @"C:\Users\henri\Downloads\clearTextImage.png";


        // nulls are alredy checked, can't throw.
        // This TessEngine must exist as long as any iterator from it
        using var engine = new TessEngine(languages, tessDataFolder, _logger);
        using var pix = Pix.LoadFromFile(imagePath);



        // EXAMPLE 1 Create iterator 
        using var iterator = engine.GetResultIterator(pix, PageIteratorLevel.Word);

        // EXAMPLE 2 Get output
        //iterator.MoveNext();    // move to index 0
        //TextSpan line = iterator.Current;   // get line

        //iterator.Level = PageIteratorLevel.Word;    // Set block size
        //iterator.MoveNext();    // move to index 0
        //TextSpan word = iterator.Current;   // get word




        // EXAMPLE 2 Copying iterator
        //void Copying()
        //{
        //    iterator.MoveNext();

        //    TextSpan span = iterator.Current;

        //    using ResultIterator? iterator2 = iterator.CopyToCurrentIndex();
        //    ArgumentNullException.ThrowIfNull(iterator2);

        //    TextSpan copiedSpan = iterator2.Current;

        //    // should be same
        //    bool match = span.Text == copiedSpan.Text;

        //    iterator2.MoveNext();
        //    iterator2.MoveNext();

        //    // Can be deconstructed
        //    var (text, confidence) = iterator2.Current;
        //}




        // EXAMPLE 3 As iterable and loop
        //List<TextSpan> spans = new();
        //foreach (var i in engine.GetResultIterable(pix))
        //{
        //    spans.Add(i);
        //}

        // EXAMPLE 4 Getting image layout data with PageIterator
        //var pageIter = new PageIterable(engine);

        //List<SpanInfo> spanInfos = new();
        //foreach (var spanInfo in pageIter)
        //{
        //    spanInfos.Add(spanInfo);
        //}

    }
}

#endif
#endif