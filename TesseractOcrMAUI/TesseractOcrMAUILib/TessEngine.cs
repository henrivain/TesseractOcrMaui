using TesseractOcrMAUILib.ImportApis;


namespace TesseractOcrMAUILib;
public class TessEngine : DisposableObject
{
    public TessEngine(string language, ILogger? logger = null) 
        : this(language, string.Empty, logger) 
    { 
    }

    public TessEngine(string language, string traineddataPath, ILogger? logger = null)
        : this(language, traineddataPath, EngineMode.Default, new Dictionary<string, object>(), logger)
    {
    }

    public TessEngine(string language, string traineddataPath, EngineMode mode, 
        IDictionary<string, object> initialOptions, ILogger? logger = null)
    {
        Logger = logger ?? NullLogger<TessEngine>.Instance;
        if (language is null)
        {
            Logger.LogError("Cannot initilize '{ctor}' with null language.", nameof(TessEngine));
            throw new ArgumentNullException(nameof(language));
        }
        if (traineddataPath is null)
        {
            Logger.LogError("Cannot initilize '{ctor}' with null trained data path.", nameof(TessEngine));
            throw new ArgumentNullException(nameof(traineddataPath));
        }
        Handle = new(this, TesseractApi.CreateApi());
        Initialize(language, traineddataPath, mode, initialOptions);
    }




    public HandleRef Handle { get; private set; }
    public int ProcessCount { get; private set; } = 0;
    public PageSegmentationMode DefaultSegmentationMode { get; set; } = PageSegmentationMode.Auto;
    public static string Version => Marshal.PtrToStringAnsi(TesseractApi.GetVersion()) ?? string.Empty;
    ILogger Logger { get; }

    public TessPage Process(Pix image, PageSegmentationMode? mode = null)
    {
        return Process(image, null, new Rect(0, 0, image.Width, image.Height), mode);
    }

    public TessPage Process(Pix image, string? inputName, Rect region, PageSegmentationMode? mode)
    {
        if (image is null)
        {
            throw new ArgumentNullException(nameof(image));
        }
        if (region.X1 < 0 || region.Y1 < 0 || region.X1 > image.Width ||
            region.Y2 > image.Height)
        {
            throw new ArgumentException($"Image {region} out of bounds, " +
                $"must be within image bounds", nameof(region));
        }
        if (ProcessCount > 0)
        {
            throw new InvalidOperationException("Only one image can be processed. " +
                "You must dispose page after using.");
        }
        ProcessCount++;
        TesseractApi.SetPageSegmentationMode(Handle, mode ?? DefaultSegmentationMode);
        TesseractApi.SetImage(Handle, image.Handle);
        if (string.IsNullOrEmpty(inputName) is false)
        {
            TesseractApi.SetInputName(Handle, inputName);
        }

        TessPage page = new(this, image, inputName ?? string.Empty, region, mode, Logger);
        page.Disposed += OnIteratorDisposed;
        return page;
    }

    public bool SetDebugVariable(string name, string value)
    {
        return TesseractApi.SetDebugVariable(Handle, name, value) is not 0;
    }
    public bool SetVariable(string name, string value)
    {
        return TesseractApi.SetVariable(Handle, name, value) is not 0;
    }

    public bool TryGetBoolVar(string name, out bool value)
    {
        if (TesseractApi.GetBoolVariable(Handle, name, out int result) is not 0)
        {
            value = result is not 0;
            return true;
        }
        value = false;
        return false;
    }
    public bool TryGetIntVar(string name, out int value)
    {
        return TesseractApi.GetIntVariable(Handle, name, out value) is not 0;
    }
    public bool TryGetDoubleVar(string name, out double value)
    {
        return TesseractApi.GetDoubleVariable(Handle, name, out value) is not 0;
    }
    public string GetStringVar(string name)
    {
        return TesseractApi.GetStringVariable(Handle, name) ?? string.Empty;
    }
    public bool TryPrintVariablesToFile(string fileName)
    {
        return TesseractApi.PrintVariablesToFile(Handle, fileName) is not 0;
    }


    /// <summary>
    /// Initialize new Tesseract engine with given data.
    /// </summary>
    /// <param name="language">'+' separated list of languages to be used with api. 
    /// Must have equivalent traineddatafile in specified directory.</param>
    /// <param name="traineddataPath">Path to directory containing traineddata files.</param>
    /// <param name="mode">Tesseract mode to be used.</param>
    /// <param name="initialOptions">Dictionary of option configurations.</param>
    /// <exception cref="TesseractException">If Api cannot be initialized.</exception>
    private void Initialize(string language, string traineddataPath,
        EngineMode mode, IDictionary<string, object> initialOptions)
    {
        language ??= string.Empty;
        traineddataPath ??= string.Empty;

        int apiStatus = TessApi.BaseApiInit(Handle, language, traineddataPath, mode, initialOptions);
        if (apiStatus is not 0)
        {
            Dispose();
            throw new TesseractException("Cannot initialize Tesseract Api");
        }
    }

    private void OnIteratorDisposed(object? sender, EventArgs e)
    {
        ProcessCount--;
    }
    protected override void Dispose(bool disposing)
    {
        if (Handle.Handle != IntPtr.Zero)
        {
            TesseractApi.DeleteApi(Handle);
            Handle = new HandleRef(this, IntPtr.Zero);
        }
    }
}
