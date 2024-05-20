using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics;
using TesseractOcrMaui;
using TesseractOcrMaui.Iterables;
using TesseractOcrMaui.Results;
using TesseractOcrMaui.Tessdata;

namespace TesseractOcrMauiTestApp;
public class ResultIteratorExample
{

    private readonly ITessDataProvider _provider;
    private readonly ILogger<ResultIteratorExample> _logger;

    public ResultIteratorExample(ITessDataProvider provider, ILogger<ResultIteratorExample> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public async Task<List<string>?> GetImageTextLines()
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
            return null;
        }

        _logger.LogInformation("Image at {}.", imagePath);

        // Load image
        Stopwatch sw = Stopwatch.StartNew();

        using Pix pix = Pix.LoadFromFile(imagePath);

        long ms1 = sw.ElapsedMilliseconds;
        PrintTime(sw, "Pix.LoadFromFile(string)");

        // Create itearble, set image and recognize
        sw.Restart();

        using ResultIterable iterable = new(pix, _provider, TesseractOcrMaui.Enums.PageIteratorLevel.TextLine, _logger);

        long ms2 = sw.ElapsedMilliseconds;
        PrintTime(sw, "new ResultIterable()");

        // Iterate over result
        sw.Restart();

        List<string> lines = new ();

        foreach (var item in iterable)
        {
            lines.Add(item.Text);
        }

        PrintTime(sw, "foreach(ResultIterable)");
        return lines;
    }


    private void PrintTime(Stopwatch sw, string methodName)
    {
        long ms = sw.ElapsedMilliseconds;
        _logger.LogInformation("[{ms} ms] [HIGH LEVEL] to execute {method}.", ms, methodName);
    }
}
