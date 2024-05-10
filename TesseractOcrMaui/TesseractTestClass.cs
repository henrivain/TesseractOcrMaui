#if DEBUG
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

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

    public string? Languages { get; set; }
    public string? TessDataFolder { get; set; }

    public async Task Load()
    {
        // Load tessdata for testing purposes, configure in MauiProgram.cs, do not touch here
        await _tessDataProvider.LoadFromPackagesAsync();
        TessDataFolder = _tessDataProvider.TessDataFolder;
        Languages = string.Join('+', _tessDataProvider.AvailableLanguages.Select(x => x.Replace(".traineddata", "")));
    }


    public void Run()
    {
        // [INPUT] give image here
        //string imagePath = @"C:\Users\henri\Downloads\tess version wsl.png";
        //string imagePath = @"C:\Users\henri\Downloads\clearTextImage.png";


        // nulls are alredy checked, can't throw.
        // This TessEngine must exist as long as any iterator from it
        //using var engine = new TessEngine(Languages!, TessDataFolder!, _logger);
        //using var pix = Pix.LoadFromFile(imagePath);



        /* EXAMPLE 1 Create iterator */
        //using var iterator = engine.GetResultIterator(pix, PageIteratorLevel.Word);

        // EXAMPLE 2 Get output
        //iterator.MoveNext();    // move to index 0
        //TextSpan line = iterator.Current;   // get line

        //iterator.Level = PageIteratorLevel.Word;    // Set block size
        //iterator.MoveNext();    // move to index 0
        //TextSpan word = iterator.Current;   // get word




        /* EXAMPLE 3 Copying iterator */
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




        /* EXAMPLE 4 As iterable and loop */
        //List<TextSpan> spans = new();
        //foreach (var i in engine.GetResultIterable(pix))
        //{
        //    spans.Add(i);
        //}




        /* EXAMPLE 5 Getting image layout data with PageIterator */
        //var pageIter = new PageIterable(engine);

        //List<SpanInfo> spanInfos = new();
        //foreach (var spanInfo in pageIter)
        //{
        //    spanInfos.Add(spanInfo);
        //}




        /* EXAMPLE 5 Layout-text combine iterator
        *  This works, because both iterators are using same pointer,
        *  So calling next() to one moves both */





        //using ResultIterator resultIter = engine.GetResultIterator(pix);
        //using PageIterator pageIter = resultIter.AsPageIterator();

        //Debug.WriteLine(resultIter.Level);
        //Debug.WriteLine(pageIter.Level);

        //pageIter.MoveNext();    

        //List<(TextSpan, SpanLayout)> spans = new();
        //while (resultIter.MoveNext())
        //{
        //    yield return (resultIter.Current, pageIter.Current);
        //}

    }
}


#endif
