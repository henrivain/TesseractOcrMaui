using TesseractOcrMaui.ImportApis;
using TesseractOcrMaui.Iterables;
using TesseractOcrMaui.Utilities;

namespace TesseractOcrMaui;

/// <summary>
/// Tesseract engine that can process images with native library bindings.
/// </summary>
public partial class TessEngine : DisposableObject, ITessEngineConfigurable
{
    int _processCount = 0;

    readonly ILogger _logger;

    /// <summary>
    /// Create new Tess engine with native Tesseract api.
    /// </summary>
    /// <param name="languages">
    /// Language means traineddata file names without extension as a '+' separated list.
    /// For example 'fin+swe+eng'
    /// </param>
    /// <param name="logger">Logger to be used, by default NullLogger.</param>
    /// <exception cref="ArgumentNullException">If languages or traineddatapath is null.</exception>
    /// <exception cref="TesseractException">If Tesseract api cannot be initialized with given parameters or for some other reason.</exception>
    public TessEngine(string languages, ILogger? logger = null)
        : this(languages, string.Empty, logger)
    {
    }

    /// <summary>
    /// Create new Tess engine with native Tesseract api.
    /// </summary>
    /// <param name="languages">
    /// Language means traineddata file names without extension as a '+' separated list.
    /// For example 'fin+swe+eng'
    /// </param>
    /// <param name="traineddataPath">Full path to traineddata folder. Do not include file name.</param>
    /// <param name="logger">Logger to be used, by default NullLogger.</param>
    /// <exception cref="ArgumentNullException">If languages or traineddatapath is null.</exception>
    /// <exception cref="TesseractException">If Tesseract api cannot be initialized with given parameters or for some other reason.</exception>
    public TessEngine(string languages, string traineddataPath, ILogger? logger = null)
        : this(languages, traineddataPath, EngineMode.Default, new Dictionary<string, object>(), logger)
    {
    }

    /// <summary>
    /// Create new Tess engine with native Tesseract api.
    /// </summary>
    /// <param name="languages">
    /// Language means traineddata file names without extension as a '+' separated list.
    /// For example 'fin+swe+eng'
    /// </param>
    /// <param name="traineddataPath">Full path to traineddata folder. Do not include file name.</param>
    /// <param name="mode">Engine mode to be used when recognizing images.</param>
    /// <param name="initialOptions">Optional Tesseract parameters to be used.</param>
    /// <param name="logger">Logger to be used, by default NullLogger.</param>
    /// <exception cref="ArgumentNullException">If languages or traineddatapath is null.</exception>
    /// <exception cref="TesseractException">If Tesseract api cannot be initialized with given parameters or for some other reason.</exception>
    public TessEngine(string languages, string traineddataPath, EngineMode mode,
        IDictionary<string, object> initialOptions, ILogger? logger = null)
    {
        _logger = logger ?? NullLogger<TessEngine>.Instance;
        if (string.IsNullOrEmpty(languages))
        {
            _logger.LogError("Cannot initilize '{ctor}' with null or empty language.", nameof(TessEngine));
            throw new ArgumentNullException(nameof(languages));
        }
        if (traineddataPath is null)
        {
            _logger.LogError("Cannot initilize '{ctor}' with null trained data path.", nameof(TessEngine));
            throw new ArgumentNullException(nameof(traineddataPath));
        }
        // Debug: This line throws if dll are not copied to correct folder.
        Handle = new(this, TesseractApi.CreateApi());
        Initialize(languages, traineddataPath, mode, initialOptions);
    }



    /// <inheritdoc/>
    public PageSegmentationMode DefaultSegmentationMode { get; set; } = PageSegmentationMode.Auto;

    /// <summary>
    /// Is <see cref="SetImage(Pix)"/> called?
    /// </summary>
    public bool IsImageSet { get; private set; } = false;

    /// <summary>
    /// Check if currently set image is already recognized.
    /// </summary>
    /// <returns><see langword="true"/> if <see cref="Recognize(HandleRef?)"/> is called, otherwise <see langword="false"/>.</returns>
    public bool IsRecognized { get; private set; } = false;

    /// <summary>
    /// Handle to used Tesseract api.
    /// </summary>
    internal HandleRef Handle { get; private set; }


    /// <summary>
    /// Version of used Tesseract api. Returns null if version cannot be obtained.
    /// </summary>
    /// <returns>Version string if successful, otherwise null</returns>
    public static string? TryGetVersion()
        => Marshal.PtrToStringAnsi(TesseractApi.GetVersion());

    /*  Next methods should be called only one and once before cleaning (Cleaning not Supported yet)
     *  - ProcessImage(Pix, PageSegmentationMode)
     *  - ProcessImage(Pix, string?, Rect, PageSegmentationMode)
     *  - GetResultIterator(Pix, PageIteratorLevel)
     *  - GetResultIterable(Pix, PageIteratorLevel)
     *  - GetPageIterator(Pix, PageIteratorLevel)
     *  - GetPageIterable(Pix, PageIteratorLevel)
     */

    /// <summary>
    /// Process image to TessPage.
    /// </summary>
    /// <param name="image"></param>
    /// <param name="mode"></param>
    /// <returns>New Tess page containing information for recognizion.</returns>
    /// <exception cref="ArgumentNullException">image is null.</exception>
    /// <exception cref="ArgumentException">Image width or height has invalid values.</exception>
    /// <exception cref="PageNotDisposedException">Image already processed. You must dispose page after using.</exception>
    public TessPage ProcessImage(Pix image, PageSegmentationMode? mode = null)
        => ProcessImage(image, null, new Rect(0, 0, image.Width, image.Height), mode);

    /// <summary>
    /// Process image to TessPage.
    /// </summary>
    /// <param name="image"></param>
    /// <param name="inputName"></param>
    /// <param name="region"></param>
    /// <param name="mode"></param>
    /// <returns>New Tess page containing information for recognizion.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="image"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="region"/> is out of bounds.</exception>
    /// <exception cref="PageNotDisposedException">Image already processed. You must dispose page after using.</exception>
    /// <exception cref="NullPointerException">If <paramref name="image"/>.Handle is IntPtr.Zero.</exception>
    public TessPage ProcessImage(Pix image, string? inputName, Rect region, PageSegmentationMode? mode)
    {
        if (region.X1 < 0 || region.Y1 < 0 || region.X1 > image.Width || region.Y2 > image.Height)
        {
            _logger.LogError("{cls}: Image region out of bounds, cannot process.", nameof(TessEngine));
            throw new ArgumentException($"Image {region} out of bounds, " +
                $"must be within image bounds", nameof(region));
        }
        if (_processCount > 0)
        {
            _logger.LogError("{cls}: Already has one image process. You must dispose {page} after using it.",
                nameof(TessEngine), nameof(TessPage));
            throw new PageNotDisposedException("You must dispose old TessPage after using it.");
        }
        _processCount++;


        TesseractApi.SetPageSegmentationMode(Handle, mode ?? DefaultSegmentationMode);
        SetImage(image);

        if (string.IsNullOrEmpty(inputName) is false)
        {
            TesseractApi.SetInputName(Handle, inputName);
        }

        TessPage page = new(this, image, inputName, region, mode, _logger);
        page.Disposed += OnIteratorDisposed;
        return page;
    }


    /// <summary>
    /// Get iterator that is used to iterate over recognized text with different block sizes.
    /// Created instance points to current TessEngine, which must exist as long as created iterator.
    /// <para/>If you want multiple iterators use <see cref="ResultIterator.CopyToCurrentIndex"/>
    /// </summary>
    /// <param name="image">Pix image to be processed.</param>
    /// <param name="level">Text block size to take in one iteration, for example TextLine, Paragraph or Symbol.</param>
    /// <returns>
    /// ResultIterator to iterate over recognized text.
    /// </returns>
    /// <exception cref="ImageRecognizionException">If failed to recognize.</exception>
    /// <exception cref="ArgumentNullException">If <paramref name="image"/> is null.</exception>
    /// <exception cref="NullPointerException">If <see cref="Handle"/> or <see cref="Pix.Handle"/> is IntPtr.Zero.</exception>
    /// <exception cref="InvalidOperationException">If Image is already set.</exception>
    /// <exception cref="ResultIteratorException">If native asset is null, consider making bug report with current data.</exception>
    public ResultIterator GetResultIterator(Pix image, PageIteratorLevel level = PageIteratorLevel.TextLine)
    {
        // ImageNotSetException: SetImage always called -> cannot throw
        // TesseractInitException: SetImage() and Recognize() always called -> cannot throw
        SetImage(image);
        Recognize();
        return new(this, level);
    }

    /// <summary>
    /// Get <see cref="IEnumerable{T}"/> to iterate over different sized recognized text blocks.
    /// Created instance points to current TessEngine, which must exist as long as created iterable.
    /// </summary>
    /// <param name="image">Pix image to be processed.</param>
    /// <param name="level">Text block size to take in one iteration, for example TextLine, Paragraph or Symbol.</param>
    /// <returns>ResultIterable that can iterate over image text blocks.</returns>
    /// <exception cref="NullPointerException">If <see cref="Handle"/> or <see cref="Pix.Handle"/> is <see cref="IntPtr.Zero"/>.</exception>
    /// <exception cref="InvalidOperationException">If image already set.</exception>
    /// <exception cref="ArgumentNullException">If <paramref name="image"/> is null.</exception>
    /// <exception cref="ImageRecognizionException">If failed to recognize.</exception>
    /// <exception cref="ObjectDisposedException">If engine is disposed.</exception>
    public ResultIterable GetResultIterable(Pix image, PageIteratorLevel level = PageIteratorLevel.TextLine)
    {
        // ImageNotSetException: SetImage always called -> cannot throw
        // ArgumentNullException: this cannot be null -> cannot throw
        // TesseractInitException: SetImage() and Recognize() always called -> cannot throw
        SetImage(image);
        Recognize();
        return new(this, level);
    }

    /// <summary>
    /// Get iterator to iterate over recognized image text layout.
    /// Created instance points to current TessEngine, which must exist as long as created iterator.
    /// <para/>If you want multiple iterators use <see cref="PageIterator.Copy"/> or <see cref="PageIterator.CopyToCurrentIndex"/>
    /// </summary>
    /// <param name="image">Pix image to be processed.</param>
    /// <param name="level">Text block size to take in one iteration, for example TextLine, Paragraph or Symbol.</param>
    /// <returns>ResultIterator to iterate over recognized text layout.</returns>
    /// <exception cref="NullPointerException">If <see cref="Handle"/> or <see cref="Pix.Handle"/> is <see cref="IntPtr.Zero"/>.</exception>
    /// <exception cref="InvalidOperationException">If image already set.</exception>
    /// <exception cref="ArgumentNullException">If <paramref name="image"/> is null.</exception>
    /// <exception cref="PageIteratorException">If image does not contain text.</exception>
    public PageIterator GetPageIterator(Pix image, PageIteratorLevel level = PageIteratorLevel.TextLine)
    {
        // ImageNotSetException: SetImage() always called -> cannot throw
        SetImage(image);
        return new(this, level);
    }

    /// <summary>
    /// Get <see cref="IEnumerable{T}"/> to iterate over different sized recognized text block layouts.
    /// Created instance points to current TessEngine, which must exist as long as created iterable.
    /// </summary>
    /// <param name="image"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    /// <exception cref="ObjectDisposedException">If engine is disposed during iteration.</exception>
    /// <exception cref="PageIteratorException">If page is empty.</exception>
    /// <exception cref="NullPointerException"><paramref name="image"/>.Handle or <see cref="Handle"/> is <see cref="IntPtr.Zero"/></exception>
    public PageIterable GetPageIterable(Pix image, PageIteratorLevel level = PageIteratorLevel.TextLine)
    {
        // TesseractInitException: SetImage() alway called -> cannot throw
        SetImage(image);
        return new(this, level);
    }




    #region EngineGettersAndSetters

    /// <summary>
    /// Get image after recognition process in its current form. 
    /// </summary>
    /// <returns>Recongized image.</returns>
    /// <exception cref="TesseractException">If can't get thresholded image (Ptr Zero).</exception>
    /// <exception cref="InvalidOperationException">PageSegmentationMode is OsdOnly when recognizing.</exception>
    /// <exception cref="ImageRecognizionException">Native Library call returns failed status when recognizing.</exception>
    public Pix GetThresholdedImage()
    {
        Recognize();
        IntPtr handle = TesseractApi.GetThresholdedImage(Handle);
        if (handle == IntPtr.Zero)
        {
            _logger.LogError("Tesseract cannot get thresholded image");
            throw new TesseractException("Failed to get thresholded image.");
        }
        return new(handle);
    }

    /// <summary>
    /// Set tesseract library variable for debug purposes.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns>True if success, otherwise false.</returns>
    public bool SetDebugVariable(string name, string value)
    {
        bool success = TesseractApi.SetDebugVariable(Handle, name, value) is not 0;
        _logger.LogInformation("Set Tesseract DEBUG variable '{name}' to value '{value}', success {success}",
            name, value, success);
        return success;
    }

    /// <inheritdoc/>
    public bool SetVariable(string name, string value)
    {
        bool success = TesseractApi.SetVariable(Handle, name, value) is not 0;
        _logger.LogInformation("Set Tesseract variable '{name}' to value '{value}', success {success}",
            name, value, success);
        return success;
    }

    /// <summary>
    /// Print all tesseract library variables to given file.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns>True if success, otherwise false.</returns>
    public bool TryPrintVariablesToFile(string fileName)
    {
        return TesseractApi.PrintVariablesToFile(Handle, fileName) is not 0;
    }

    /// <summary>
    /// Get bool variable from tesseract library.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns>True if success, otherwise false.</returns>
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

    /// <summary>
    /// Get int variable from tesseract library.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns>True if success, otherwise false.</returns>
    public bool TryGetIntVar(string name, out int value)
    {
        return TesseractApi.GetIntVariable(Handle, name, out value) is not 0;
    }

    /// <summary>
    /// Get double variable from tesseract library.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns>True if success, otherwise false.</returns>
    public bool TryGetDoubleVar(string name, out double value)
    {
        return TesseractApi.GetDoubleVariable(Handle, name, out value) is not 0;
    }

    /// <summary>
    /// Get string variable from tesseract library.
    /// </summary>
    /// <param name="name"></param>
    /// <returns>String value of library variable or empty if can't find with given name.</returns>
    public string GetStringVar(string name)
    {
        return TesseractApi.GetStringVariable(Handle, name) ?? string.Empty;
    }

    #endregion EngineGettersAndSetters


    /// <summary>
    /// Set source image for recognizion process. 
    /// </summary>
    /// <param name="image"></param>
    /// <exception cref="NullPointerException">If <paramref name="image"/>.Handle is Intptr.Zero.</exception>
    /// <exception cref="InvalidOperationException">If one image is already set with current engine.</exception>
    /// <exception cref="ArgumentNullException">If <paramref name="image"/> is null.</exception>
    internal void SetImage(Pix image)
    {
        ArgumentNullException.ThrowIfNull(image);
        NullPointerException.ThrowIfNull(Handle);
        NullPointerException.ThrowIfNull(image.Handle);

        if (IsImageSet)
        {
            throw new InvalidOperationException("Engine image already set.");
        }

        TesseractApi.SetImage(Handle, image.Handle);
        IsImageSet = true;
    }

    /// <summary>
    /// Process image so that result iterator can be created.
    /// </summary>
    /// <returns>True if recognized successfully, otherwise false</returns>
    /// <exception cref="ImageNotSetException">If source image is not set before calling this method.</exception>
    /// <exception cref="ImageRecognizionException">If image cannot be processed and recognition failed.</exception>
    internal void Recognize(HandleRef? monitorHandle = null)
    {
        monitorHandle ??= new HandleRef(null, IntPtr.Zero);
        if (IsImageSet is false)
        {
            throw new ImageNotSetException("Cannot recognize image. No image source set.");
        }
        if (IsRecognized)
        {
            _logger.LogInformation("{} image already recognized, skip recognition.", nameof(TessEngine));
            return;
        }

        // 0 -> success, -1 -> failed
        int status = TesseractApi.Recognize(Handle, monitorHandle.Value);
        if (status is not 0)
        {
            _logger.LogError("Could not Recognize image with api status {}", status);
            throw new ImageRecognizionException("Failed to Recognize image");
        }
        IsRecognized = true;
    }






    /// <summary>
    /// Fill Tesseract 5 native constructor and initialize new engine.
    /// </summary>
    /// <param name="handle">Engine handle</param>
    /// <param name="languages">'+' -seaprated list of traineddata file names without extensions.</param>
    /// <param name="traineddataPath">Path to traineddata folder with no file name</param>
    /// <param name="mode">Which tesseract engine mode should be used</param>
    /// <param name="initialOptions"></param>
    /// <returns>0 if success, otherwise some other api status code</returns>
    /// <exception cref="NullPointerException"><paramref name="handle"/> is IntPtr.Zero</exception>
    /// <exception cref="ArgumentNullException"><paramref name="languages"/> is null</exception>
    internal static int InitializeTesseractApi5(HandleRef handle, string languages, string traineddataPath,
        EngineMode mode, IDictionary<string, object> initialOptions)
    {
        NullPointerException.ThrowIfNull(handle);
        ArgumentNullException.ThrowIfNull(languages);

        traineddataPath ??= string.Empty;

        List<string> optionVariables = new();
        List<string> optionValues = new();
        foreach (var (variable, values) in initialOptions)
        {
            string? result = TessConverter.TryToString(values);
            if (result is not null && string.IsNullOrWhiteSpace(variable) is false)
            {
                optionVariables.Add(variable);
                optionValues.Add(result!);
            }
        }
        string[] configs = Array.Empty<string>();
        string[] options = optionVariables.ToArray();
        string[] optVals = optionValues.ToArray();

        int initState = TesseractApi.BaseApi5Init(handle, traineddataPath, 0, languages,
            mode, configs, configs.Length, options, optVals,
            (nuint)options.Length, false);

        return initState;
    }

    /// <summary>
    /// Initialize new Tesseract engine with given data.
    /// </summary>
    /// <param name="languages">
    /// '+' separated list of languages to be used with api. 
    /// Must have equivalent traineddatafile in specified traineddata directory.
    /// Do not include '.traineddata' extension or path.
    /// </param>
    /// <param name="traineddataPath">Path to directory containing traineddata files.</param>
    /// <param name="mode">Tesseract mode to be used.</param>
    /// <param name="initialOptions">Dictionary of option configurations.</param>
    /// <exception cref="TesseractException">If Api cannot be initialized for some reason.</exception>
    private void Initialize(string languages, string traineddataPath,
        EngineMode mode, IDictionary<string, object> initialOptions)
    {
        _logger.LogInformation("Initilize new '{cls}' with language '{lang}' and traineddata path '{path}'",
            nameof(TessEngine), languages, traineddataPath);

        languages ??= string.Empty;
        traineddataPath ??= string.Empty;

        int apiStatus = InitializeTesseractApi5(Handle, languages, traineddataPath, mode, initialOptions);
        if (apiStatus is not 0)
        {
            _logger.LogError("Could not initialize new Tesseract api for {cls}. Api status {status}",
                nameof(TessEngine), apiStatus);
            Dispose();

            // check if traineddata exists 
            bool traineddataExists = AnyTessdataFileExists(traineddataPath, languages.Split('+'));
            Exception? inner = traineddataExists ? null :
                    new InvalidOperationException("No traineddata files found from path. " +
                    "Do you have correct path and file names?");

            throw new TesseractInitException("Cannot initialize Tesseract Api", inner);
        }
    }


    private static bool AnyTessdataFileExists(string path, string[] languages)
    {
        foreach (var language in languages)
        {
            string filePath = Path.Combine(path, $"{language}.traineddata");
            if (File.Exists(filePath))
            {
                return true;
            }
        }
        return false;
    }

    private void OnIteratorDisposed(object? sender, EventArgs e) => _processCount--;

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (Handle.Handle != nint.Zero)
        {
            TesseractApi.DeleteApi(Handle);
            Handle = new HandleRef(this, nint.Zero);
        }
    }
}
