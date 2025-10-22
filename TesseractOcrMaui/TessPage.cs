using TesseractOcrMaui.ImportApis;

namespace TesseractOcrMaui;

/// <summary>
/// Page that is used to contain information during and after image recognizion.
/// This object is IDisposable.
/// </summary>
public partial class TessPage : DisposableObject
{
    readonly ILogger _logger;

    static readonly Dictionary<TextFormat, Func<HandleRef, int, IntPtr>> _specialTextTypes = new()
    {
        [TextFormat.HOCR] = TesseractApi.GetHOCRText_Ptr,
        [TextFormat.ALTO] = TesseractApi.GetAltoText_Ptr,
        [TextFormat.Page] = TesseractApi.GetBoxText_Ptr,
        [TextFormat.TSV] = TesseractApi.GetTsvText_Ptr,
        [TextFormat.Box] = TesseractApi.GetBoxText_Ptr,
        [TextFormat.StrBox] = TesseractApi.GetWordStrBoxText_Ptr
    };


    /// <summary>
    /// New Tesseract page that is used to contain information when recognizing text in image.
    /// This object is IDisposable.
    /// </summary>
    /// <param name="engine"></param>
    /// <param name="image"></param>
    /// <param name="inputName"></param>
    /// <param name="region"></param>
    /// <param name="mode"></param>
    /// <param name="logger"></param>
    public TessPage(TessEngine engine, Pix image, string? inputName, Rect region, PageSegmentationMode? mode, ILogger? logger = null)
    {
        Engine = engine;
        Image = image;
        InputName = inputName ?? string.Empty;
        Region = region;
        Mode = mode;
        _logger = logger ?? NullLogger.Instance;
    }

    /// <summary>
    /// Tesseract engine that uses native Tesseract library.
    /// </summary>
    public TessEngine Engine { get; }

    /// <summary>
    /// Image where text is recognized from.
    /// </summary>
    public Pix Image { get; }

    /// <summary>
    /// Name of input image.
    /// </summary>
    public string InputName { get; }

    /// <summary>
    /// Region to be recognized.
    /// </summary>
    public Rect Region { get; }

    /// <summary>
    /// Segmentation mode that Tesseract will use.
    /// </summary>
    public PageSegmentationMode? Mode { get; }

    /// <summary>
    /// Is current image already recognized.
    /// </summary>
    public bool AlreadyRecognized => Engine.IsRecognized;

    /// <summary>
    /// Directory where output image is saved.
    /// </summary>
    public string OutputDirectory { get; init; } 
        = Path.Combine(Utilities.FileSystemHelper.GetCacheFolder(), "tessoutput");



    /// <summary>
    /// Recognize text from image in UTF-8 format.
    /// </summary>
    /// <returns>UTF-8 encoded string representing recognized text.</returns>
    /// <exception cref="InvalidOperationException">If passed invalid <see cref="TextFormat"/> or PageSegmentationMode is OsdOnly when recognizing.</exception>
    /// <exception cref="ImageRecognizionException">Native Library call returns failed status when recognizing.</exception>
    /// <exception cref="TesseractException">Can't get thresholded image when recognizing.</exception>
    /// <exception cref="StringMarshallingException">If recognition result was nullptr or pointer cannot be converted to UTF-8 string.</exception>
    public string GetText()
    {
        return GetText(TextFormat.TextOnly);
    }


    /// <summary>
    /// Get text from image in special format UTF-8 encoded string.
    /// </summary>
    /// <param name="type">Type of special format requested.</param>
    /// <param name="pageNumber">Page number to be used in result.</param>
    /// <returns>UTF-8 encoded string in given fromat representing recognized text.</returns>
    /// <exception cref="InvalidOperationException">If passed invalid <see cref="TextFormat"/> or PageSegmentationMode is OsdOnly when recognizing.</exception>
    /// <exception cref="ImageRecognizionException">Native Library call returns failed status when recognizing.</exception>
    /// <exception cref="TesseractException">Can't get thresholded image when recognizing.</exception>
    /// <exception cref="StringMarshallingException">If recognition result was nullptr or pointer cannot be converted to UTF-8 string.</exception>
    public string GetText(TextFormat type, int pageNumber = 0)
    {
        if (type is not TextFormat.TextOnly && _specialTextTypes.ContainsKey(type) is false)
        {
            _logger.LogError("Text type '{type}' is not supported.", type);
            throw new InvalidOperationException($"Text type '{type}' not supported.");
        }

        _logger.LogInformation("Try to get text from image in {format} format.", type);


        // Recognize image first
        Recognize();

        // Get given text type
        IntPtr ptr = IntPtr.Zero;
        if (type == TextFormat.TextOnly)
        {
            // Skips page number as not needed
            ptr = TesseractApi.GetUTF8Text_Ptr(Engine.Handle);
        }
        else if (_specialTextTypes.TryGetValue(type, out var getter))
        {
            ptr = getter(Engine.Handle, pageNumber);
        }

        if (ptr == IntPtr.Zero)
        {
            _logger.LogError("Recognizion returned nulltpr.");
            throw new StringMarshallingException("Recognition returned nullptr.");
        }

        // Marshal to UTF-8 string
        string? utf8Result = Marshal.PtrToStringUTF8(ptr);

        // Delete native string
        TesseractApi.DeleteString(ptr);

        if (utf8Result is null)
        {
            _logger.LogError("Cannot encode text to UTF-8.");
            throw new StringMarshallingException("Failed to encode recognition result to UTF-8.");
        }

        _logger.LogInformation("Found '{count}' characters in image.", utf8Result.Length);
        return utf8Result;
    }


    /// <summary>
    /// Get image recognizion confidence.
    /// </summary>
    /// <returns>Confidence float between 0 - 1</returns>
    /// <exception cref="InvalidOperationException">PageSegmentationMode is OsdOnly when recognizing.</exception>
    /// <exception cref="ImageRecognizionException">Native Library call returns failed status when recognizing.</exception>
    public float GetConfidence()
    {
        _logger.LogInformation("Getting current Tesseract text recognizion confidence.");

        Recognize();
        float confidence = TesseractApi.GetMeanConfidence(Engine.Handle) / 100f;
        _logger.LogInformation("Confidence for image is '{value}'.", confidence);
        return confidence;
    }

    /// <summary>
    /// Run text Recognizion with defined Image. 
    /// </summary>
    /// <exception cref="InvalidOperationException">PageSegmentationMode is OsdOnly.</exception>
    /// <exception cref="ImageRecognizionException">Native Library call returns failed status.</exception>
    /// <exception cref="TesseractException">If can't get thresholded image.</exception>
    private void Recognize()
    {
        if (Engine.IsRecognized)
        {
            return;
        }
        if (Mode is PageSegmentationMode.OsdOnly)
        {
            _logger.LogError("Cannot use OsdOnly Segmentation mode to recognize image text.");
            throw new InvalidOperationException("Cannor OCR image using OSD only segmentation, " +
                "use DetectBestOrientation instead.");
        }

        Engine.Recognize();

        if (Engine.TryGetBoolVar("tessedit_write_images", out bool value) is false && value is false)
        {
            return;
        }

        using Pix pix = Engine.GetThresholdedImage();
        try
        {
            pix.Save(OutputDirectory, ImageFormat.TiffG4);
        }
        catch (IOException)
        {
            _logger.LogWarning("Failed to save image to tif format using path '{dir}'. IO Exception was thrown.", OutputDirectory);
        }
        catch (ArgumentException)
        {
            _logger.LogWarning("Failed to save image to '{dir}'. Could not get file name.", OutputDirectory);
        }
    }

    /// <summary>
    /// Dispose current page instance.
    /// </summary>
    /// <param name="disposing"></param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            TesseractApi.Clear(Engine.Handle);
        }
    }
}
