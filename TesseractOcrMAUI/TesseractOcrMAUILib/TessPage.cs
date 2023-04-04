using System.Text;
using TesseractOcrMAUILib.ImportApis;


namespace TesseractOcrMAUILib;
public class TessPage : DisposableObject
{
    public TessPage(TessEngine engine, Pix image, string inputName, Rect region, PageSegmentationMode? mode)
        : this(engine, image, inputName, region, mode, NullLogger.Instance)
    {
    }

    public TessPage(TessEngine engine, Pix image, string inputName, Rect region, PageSegmentationMode? mode, ILogger logger)
    {
        Engine = engine;
        Image = image;
        InputName = inputName;
        Region = region;
        Mode = mode;
        Logger = logger;
    }

    public TessEngine Engine { get; }
    public Pix Image { get; }
    public string InputName { get; }
    public Rect Region { get; }
    public PageSegmentationMode? Mode { get; }
    public bool AlreadyRecognized { get; private set; }
    public string OutputDirectory { get; init; } = Path.Combine(FileSystem.CacheDirectory, "tessoutput");
    public ILogger Logger { get; }

    public string GetText()
    {
        Recognize();

        string result = TesseractApi.GetUTF8Text(Engine.Handle);

        var bytes = new byte[result.Length];
        for (int i = 0; i < result.Length; i++)
        {
            bytes[i] = (byte)result[i];
        }
        return Encoding.UTF8.GetString(bytes);

    }

    private void Recognize()
    {
        if (Mode is PageSegmentationMode.OsdOnly)
        {
            throw new InvalidOperationException("Cannor OCR image using OSD only segmentation, " +
                "use DetectBestOrientation instead.");
        }
        if (AlreadyRecognized)
        {
            return;
        }
        int recognizionStatus = TesseractApi.Recognize(Engine.Handle, new HandleRef(this, IntPtr.Zero));
        if (recognizionStatus is not 0)
        {
            throw new InvalidOperationException("Recognizion failed.");
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
        catch
        {
            System.Diagnostics.Debug.WriteLine($"Failed to save image to tif format using path '{OutputDirectory}'.");
        }
    }

    private Pix GetThresholdedImage()
    {
        Recognize();
        IntPtr ptr = TesseractApi.GetThresholdedImage(Engine.Handle);
        if (ptr == IntPtr.Zero)
        {
            throw new TesseractException("Failed to get thresholded image.");
        }
        return Pix.Create(ptr);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            TesseractApi.Clear(Engine.Handle);
        }
    }

    internal float GetConfidence()
    {
        Recognize();
        return TesseractApi.GetMeanConfidence(Engine.Handle) / 100f;
    }
}
