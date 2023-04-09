#if WINDOWS
using System.Text;
#endif
using MauiTesseractOcr.ImportApis;

namespace MauiTesseractOcr;

/// <summary>
/// Page that is used to contain information during and after image recognizion.
/// This object is IDisposable.
/// </summary>
public class TessPage : DisposableObject
{
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
        Logger = logger ?? NullLogger.Instance;
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
    public bool AlreadyRecognized { get; private set; }

    /// <summary>
    /// Directory where output image is saved.
    /// </summary>
    public string OutputDirectory { get; init; } = Path.Combine(FileSystem.CacheDirectory, "tessoutput");
    
    ILogger Logger { get; }

    /// <summary>
    /// Get text from image. Runs recognizion if it is not already done. Uses UTF-8.
    /// </summary>
    /// <returns>Text that apprears in image.</returns>
    /// <exception cref="InvalidOperationException">PageSegmentationMode is OsdOnly when recognizing.</exception>
    /// <exception cref="ImageRecognizionException">Native Library call returns failed status when recognizing.</exception>
    /// <exception cref="TesseractException">Can't get thresholded image when recognizing.</exception>
    /// <exception cref="InvalidBytesException">[WINDOWS] Invalid byte sequence in string.</exception>
    public string GetText()
    {
        Logger.LogInformation("Try to get text from image.");

        Recognize();

        string result = TesseractApi.GetUTF8Text(Engine.Handle);

        Logger.LogInformation("Found '{count}' characters in image.", result.Length);

        // My Windows seems to use different encoding than UTF-8 by default, so this should help.
        // Android uses UTF-8 as default so all good.
#if WINDOWS
        var bytes = new byte[result.Length];
        for (int i = 0; i < result.Length; i++)
        {
            bytes[i] = (byte)result[i];
        }
        if (bytes is null)
        {
            return string.Empty;
        }
        try
        {
            return Encoding.UTF8.GetString(bytes);
        }
        catch (Exception ex)
        {
            throw new InvalidBytesException("Cannot encode current byte array, because it contains invalid bytes.", ex);
        }
#else
        return result;
#endif
    }

    /// <summary>
    /// Get image recognizion confidence.
    /// </summary>
    /// <returns>Confidence float between 0 - 1</returns>
    /// <exception cref="InvalidOperationException">PageSegmentationMode is OsdOnly when recognizing.</exception>
    /// <exception cref="ImageRecognizionException">Native Library call returns failed status when recognizing.</exception>
    public float GetConfidence()
    {
        Logger.LogInformation("Getting current Tesseract text recognizion confidence.");

        Recognize();
        float confidence = TesseractApi.GetMeanConfidence(Engine.Handle) / 100f;
        Logger.LogInformation("Confidence for image is '{value}'.", confidence);
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
        if (AlreadyRecognized)
        {
            return;
        }
        if (Mode is PageSegmentationMode.OsdOnly)
        {
            Logger.LogError("Cannot use OsdOnly Segmentation mode to recognize image text.");
            throw new InvalidOperationException("Cannor OCR image using OSD only segmentation, " +
                "use DetectBestOrientation instead.");
        }
        int recognizionStatus = TesseractApi.Recognize(Engine.Handle, new HandleRef(this, nint.Zero));
        if (recognizionStatus is not 0)
        {
            Logger.LogError("Image recognizion failed");
            throw new ImageRecognizionException("Recognizion failed.");
        }

        AlreadyRecognized = true;
        if (Engine.TryGetBoolVar("tessedit_write_images", out bool value) is false &&
            value is false)
        {
            return;
        }

        using Pix pix = GetThresholdedImage();
        try
        {
            pix.Save(OutputDirectory, ImageFormat.TiffG4);
        }
        catch (IOException)
        {
            Logger.LogInformation("Failed to save image to tif format using path '{dir}'. IO Exception was thrown.", OutputDirectory);
        }
        catch (ArgumentException)
        {
            Logger.LogWarning("Failed to save image to '{dir}'. Could not get file name.", OutputDirectory);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="TesseractException">If can't get thresholded image (Ptr Zero).</exception>
    /// <exception cref="InvalidOperationException">PageSegmentationMode is OsdOnly when recognizing.</exception>
    /// <exception cref="ImageRecognizionException">Native Library call returns failed status when recognizing.</exception>
    private Pix GetThresholdedImage()
    {
        Recognize();
        nint handle = TesseractApi.GetThresholdedImage(Engine.Handle);
        if (handle == nint.Zero)
        {
            Logger.LogError("Tesseract cannot get thresholded image");
            throw new TesseractException("Failed to get thresholded image.");
        }
        return Pix.FromHandle(handle);
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
