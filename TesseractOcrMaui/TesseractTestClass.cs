#if DEBUG
#if !IOS
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using TesseractOcrMaui.ImportApis;
using TesseractOcrMaui.Tessdata;
using static System.Net.Mime.MediaTypeNames;

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
        using var engine = new TessEngine(languages, tessDataFolder, EngineMode.Default,
            new Dictionary<string, object>(), _logger);

        using var pix = Pix.LoadFromFile(imagePath);

        TesseractApi.SetImage(engine.Handle, pix.Handle);

        IntPtr pageIterator = TesseractApi.AnalyseLayoutToPageIterator(engine.Handle);
        IntPtr resultIterator = TesseractApi.GetResultIterator(engine.Handle);


        
        
    }
}



#endif
#endif