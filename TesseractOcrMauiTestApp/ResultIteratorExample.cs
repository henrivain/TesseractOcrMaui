using Microsoft.Extensions.Logging;
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

        // Load image
        Stopwatch sw = Stopwatch.StartNew();

        using Pix pix = Pix.LoadFromFile(imagePath);

        long ms1 = sw.ElapsedMilliseconds;
        _logger.LogInformation("Image loading took {ms1} ms", ms1);
        
        // Create itearble, set image and recognize
        sw.Restart();

        using ResultIterable iterable = new(pix, _provider, TesseractOcrMaui.Enums.PageIteratorLevel.TextLine, _logger);

        long ms2 = sw.ElapsedMilliseconds;
        _logger.LogInformation("iterable creation took {ms2} ms", ms2);

        // Iterate over result
        sw.Restart();

        List<string> lines = new ();

        foreach (var item in iterable)
        {
            lines.Add(item.Text);
        }

        long ms3 = sw.ElapsedMilliseconds;
        _logger.LogInformation("Iterating took {ms3} ms", ms3);

        return lines;
    }
}
