#if DEBUG
#if !IOS
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using TesseractOcrMaui.Iterables;
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
        var result = await _tessDataProvider.LoadFromPackagesAsync();
        string tessDataFolder = _tessDataProvider.TessDataFolder;

        string languages = string.Join('+', _tessDataProvider.AvailableLanguages.Select(x => x.Replace(".traineddata", "")));
        string imagePath = @"C:\Users\henri\Downloads\tess version wsl.png";


        // nulls are alredy checked, can't throw.
        using var engine = new TessEngine(languages, tessDataFolder, _logger);
        using var pix = Pix.LoadFromFile(imagePath);



        //IntPtr pageIterator = ResultIteratorApi.GetPageIterator(iterator.Handle);


        using var iterator = engine.GetResultIterator(pix);

        using PageIterator pageIter = new (iterator);









        //iterator.MoveNext();
        //string? lang = iterator.GetCurrentRecognizedLanguage();

        //using ResultIterator? iterator2 = iterator.CopyInCurrentIndex();
        //ArgumentNullException.ThrowIfNull(iterator2);

        //iterator2.Reset();

        //iterator2.MoveNext();
        //iterator2.MoveNext();
        //iterator2.MoveNext();
        //string text = iterator2.Current.ToString();





        //ResultIterable values = engine.GetResultIterable(pix);

        //List<TextSpan> spans = new();
        //foreach (var span in values)
        //{
        //    spans.Add(span);
        //}


        //List<TextSpan> spans2 = new();
        //foreach (var span in values)
        //{
        //    spans2.Add(span);
        //}





        //IntPtr pageIterator = TesseractApi.AnalyseLayoutToPageIterator(engine.Handle);


    }
}



#endif
#endif