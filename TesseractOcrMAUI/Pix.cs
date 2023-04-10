// Parts copied from https://github.com/charlesw/tesseract (With a lot reformatting)
using TesseractOcrMaui.Imaging;
using TesseractOcrMaui.ImportApis;
using System.Runtime.CompilerServices;

namespace TesseractOcrMaui;

/// <summary>
/// Leptonica Image type used by tesseract.
/// </summary>
public unsafe sealed class Pix : DisposableObject, IEquatable<Pix>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="handle"></param>
    /// <exception cref="ArgumentNullException">Handle is zero ptr.</exception>
    private Pix(IntPtr handle)
    {
        if (handle == IntPtr.Zero)
        {
            throw new ArgumentNullException(nameof(handle), "Handle must have non Zero value (non null).");
        }

        Handle = new HandleRef(this, handle);
        Width = LeptonicaApi.PixGetWidth(Handle);
        Height = LeptonicaApi.PixGetHeight(Handle);
        Depth = LeptonicaApi.PixGetDepth(Handle);

        IntPtr colorMapHandle = LeptonicaApi.PixGetColormap(Handle);
        if (colorMapHandle != IntPtr.Zero)
        {
            _colormap = new PixColormap(colorMapHandle);
        }
    }


    PixColormap? _colormap;

    const float Deg2Rad = (float)(Math.PI / 180.0);

    const int DefaultBinarySearchReduction = 2; // binary search part

    const int DefaultBinaryThreshold = 130;

    /// <summary>
    /// Pointer handle used to access image through Leptonica.
    /// </summary>
    public HandleRef Handle { get; private set; }

    /// <summary>
    /// Bit depth of image.
    /// </summary>
    public int Depth { get; }

    /// <summary>
    /// Pixel height of image.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Pixel width of image.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Image's color map.
    /// </summary>
    public PixColormap? Colormap
    {
        get => _colormap;
        set
        {
            if (value is null)
            {
                return;
            }
            if (LeptonicaApi.PixSetColormap(Handle, value.Handle) is 0)
            {
                _colormap = value;
                return;
            }
            if (LeptonicaApi.PixDestroyColormap(Handle) is 0)
            {
                _colormap = null;
            }
        }
    }
    
    /// <summary>
    /// X-resolution of image.
    /// </summary>
    public int XResolution
    {
        get => LeptonicaApi.PixGetXRes(Handle);
        set => LeptonicaApi.PixSetXRes(Handle, value);
    }

    /// <summary>
    /// Y-resolution of image.
    /// </summary>
    public int YResolution
    {
        get => LeptonicaApi.PixGetYRes(Handle);
        set => LeptonicaApi.PixSetYRes(Handle, value);
    }


    /// <summary>
    /// Get Pix from pix handle.
    /// </summary>
    /// <param name="handle"></param>
    /// <returns>new Pix representing handle.</returns>
    /// <exception cref="ArgumentNullException">Handle is Zero ptr.</exception>
    public static Pix FromHandle(IntPtr handle) => new(handle);

    /// <summary>
    /// Create empty pix with given dimensions.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    /// <returns>new empty Pix</returns>
    /// <exception cref="ArgumentException">If any parameters are zero or less.</exception>
    /// <exception cref="InvalidOperationException">If Leptonica cannot create new pix. (for example too large image)</exception>
    public static Pix CreateEmpty(int width, int height, int depth)
    {
        if (AllowedDepths.Contains(depth) is false)
        {
            throw new ArgumentException("Depth must be 1, 2, 4, 8, 16, or 32 bits.", nameof(depth));
        }
        if (width <= 0)
        {
            throw new ArgumentException("Width must be greater than zero", nameof(width));
        }
        if (height <= 0)
        {
            throw new ArgumentException("Height must be greater than zero", nameof(height));
        }

        IntPtr handle = LeptonicaApi.PixCreate(width, height, depth);
        if (handle == IntPtr.Zero)
        {
            throw new InvalidOperationException("Failed to create pix, this normally occurs because the requested image size is too large, please check Standard Error Output.");
        }

        return new(handle);
    }

    /// <summary>
    /// Load pix from file.
    /// </summary>
    /// <param name="filePath">Full path to image file.</param>
    /// <returns>new Pix from given file.</returns>
    /// <exception cref="IOException">Could not load Pix from file.</exception>
    public static Pix LoadFromFile(string filePath)
    {
        IntPtr handle = LeptonicaApi.PixRead(filePath);
        if (handle == IntPtr.Zero)
        {
            throw new IOException($"Failed to load image '{filePath}'.");
        }
        return new(handle);
    }

    /// <summary>
    /// Create pix from byte array.
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns>New pix representing loaded memory.</returns>
    /// <exception cref="IOException">If image cannot be loaded from memory.</exception>
    public static Pix LoadFromMemory(byte[] bytes)
    {
        IntPtr handle;
        fixed (byte* ptr = bytes)
        {
            handle = LeptonicaApi.PixReadMem(ptr, bytes.Length);
        }
        if (handle == IntPtr.Zero)
        {
            throw new IOException("Failed to load image from memory.");
        }
        return new(handle);
    }

    /// <summary>
    /// Load Pix from byte array representing tiff image.
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns>New pix representing given tiff image.</returns>
    /// <exception cref="IOException">If cannot load pix image from memory.</exception>
    public static Pix LoadTiffFromMemory(byte[] bytes)
    {
        IntPtr handle;
        fixed (byte* ptr = bytes)
        {
            handle = LeptonicaApi.PixReadMemTiff(ptr, bytes.Length, 0);
        }
        if (handle == IntPtr.Zero)
        {
            throw new IOException("Failed to load image from memory.");
        }
        return new(handle);
    }

    /// <summary>
    /// Load multiple page tiff image from file.
    /// </summary>
    /// <param name="filePath">Full path to image.</param>
    /// <param name="offset"></param>
    /// <returns>New Pix image.</returns>
    /// <exception cref="IOException">If cannot read multi page tiff from file path.</exception>
    public static Pix PixReadFromMultipageTiff(string filePath, ref int offset)
    {
        IntPtr handle = LeptonicaApi.PixReadFromMultipageTiff(filePath, ref offset);
        if (handle == IntPtr.Zero)
        {
            throw new IOException($"Failed to load image from multi-page Tiff at offset {offset}.");
        }
        return new(handle);
    }

    /// <summary>
    /// Get PixData from current pix.
    /// </summary>
    /// <returns>data from current instance.</returns>
    public PixData GetData() => new(this);

    /// <summary>
    /// Saves image to given file.
    /// </summary>
    /// <param name="filePath">Path to file</param>
    /// <param name="format">Image save format. If null, guesses format from image extension.</param>
    /// <exception cref="IOException">Could not save file.</exception>
    /// <exception cref="ArgumentException">Could not get file extension from path.</exception>
    public void Save(string filePath, ImageFormat? format = null)
    {
        ImageFormat actualFormat;
        if (format.HasValue is false)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            if (extension is null || ImageFormats.TryGetValue(extension, out actualFormat) is false)
            {
                // couldn't find matching format, perhaps there is no extension or it's not recognised, fallback to default.
                actualFormat = ImageFormat.Default;
            }
        }
        else
        {
            actualFormat = format.Value;
        }

        if (LeptonicaApi.PixWrite(filePath, Handle, actualFormat) is not 0)
        {
            throw new IOException($"Failed to save image to '{filePath}'.");
        }
    }

    /// <summary>
    /// Get new reference to existing Pix. Reference count will be incremented in lib side.
    /// </summary>
    /// <remarks>
    /// A "clone" is simply a reference to an existing pix. It is implemented this way because
    /// image can be large and hence expensive to copy and extra handles need to be made with a simple
    /// policy to avoid double frees and memory leaks.
    ///
    /// The general usage protocol is:
    /// <list type="number">
    /// 	<item>Whenever you want a new reference to an existing <see cref="Pix" /> call <see cref="Clone" />.</item>
    ///     <item>
    /// 		Always call <see cref="Dispose" /> on all references. This decrements the reference count and
    /// 		will destroy the pix when the reference count reaches zero.
    /// 	</item>
    /// </list>
    /// </remarks>
    /// <returns>The pix with it's reference count incremented.</returns>
    /// <exception cref="ArgumentNullException">If cannot clone.</exception>
    public Pix Clone() => new(LeptonicaApi.PixClone(Handle));

    /// <summary>
    /// Binarization of the input image based on the passed parameters and the Otsu method
    /// </summary>
    /// <param name="sizeX"> sizeX Desired tile X dimension; actual size may vary.</param>
    /// <param name="sizeY"> sizeY Desired tile Y dimension; actual size may vary.</param>
    /// <param name="smoothX"> smoothX Half-width of convolution kernel applied to threshold array: use 0 for no smoothing.</param>
    /// <param name="smoothY"> smoothY Half-height of convolution kernel applied to threshold array: use 0 for no smoothing.</param>
    /// <param name="scoreFraction"> scoreFraction Fraction of the max Otsu score; typ. 0.1 (use 0.0 for standard Otsu).</param>
    /// <returns>The binarized image.</returns>
    /// <exception cref="LeptonicaException">Failed to binarize image.</exception>
    /// <exception cref="InvalidOperationException">Depth was not 8.</exception>
    /// <exception cref="ArgumentException">sizeX/Y was less than 16.</exception>
    public Pix BinarizeOtsuAdaptiveThreshold(int sizeX, int sizeY, int smoothX, int smoothY, float scoreFraction)
    {
        if (Depth is not 8)
        {
            throw new InvalidOperationException("Image must have a depth of 8 bits per pixel to be binerized using Otsu.");
        }
        if (sizeX < 16)
        {
            throw new ArgumentException("The sx parameter must be greater than or equal to 16");
        }
        if (sizeY < 16)
        {
            throw new ArgumentException("The sy parameter must be greater than or equal to 16");
        }

        int result = LeptonicaApi.PixOtsuAdaptiveThreshold(Handle, sizeX, sizeY, smoothX, smoothY, scoreFraction, out IntPtr ppixth, out IntPtr ppixd);

        if (ppixth != IntPtr.Zero)
        {
            // free memory held by ppixth, an array of threshold values found for each tile
            LeptonicaApi.PixDestroy(ref ppixth);
        }
        if (result is not 0)
        {
            throw new LeptonicaException("Failed to binarize image, result was not 0.");
        }
        if (ppixd == IntPtr.Zero)
        {
            throw new LeptonicaException("Failed to binarize image, result ptr was zero.");
        }
        return new(ppixd);
    }

    /// <summary>
    /// If doesn't throw, everything is good
    /// </summary>
    /// <exception cref="InvalidOperationException">Depth is not 8 OR Colormap is null.</exception>
    /// <exception cref="ArgumentException">
    /// windowHalfWidth is less than 2 or more than image maximum value
    /// OR Factor is less than 0
    /// </exception>
    private void ValidateObjectValues(int windowHalfWidth, float factor, [CallerMemberName] string callerName = "")
    {
        if (Depth is not 8)
        {
            throw new InvalidOperationException($"Source image must be 8bpp. Caller '{callerName}'.");
        }
        if (Colormap is null)
        {
            throw new InvalidOperationException($"Source image must not be color mapped. Caller '{callerName}'.");
        }
        if (windowHalfWidth < 2)
        {
            throw new ArgumentException($"The window half-width {windowHalfWidth} must be greater than 2. Caller '{callerName}'.");
        }
        int maxWindowHalfWidth = Math.Min((Width - 3) / 2, (Height - 3) / 2);
        if (windowHalfWidth >= maxWindowHalfWidth)
        {
            throw new ArgumentException($"The window half-width {windowHalfWidth} must be less than {maxWindowHalfWidth} for image. Caller '{callerName}'.");
        }
        if (factor < 0)
        {
            throw new ArgumentException($"Factor must be greater than zero (0). Caller '{callerName}'.");
        }
    }

    /// <summary>
    /// Binarization of the input image using the Sauvola local thresholding method.
    ///
    /// Note: The source image must be 8 bpp grayscale; not colormapped.
    /// </summary>
    /// <remarks>
    /// <list type="number">
    ///     <listheader>Notes</listheader>
    ///     <item>The window width and height are 2 * <paramref name="whsize"/> + 1. The minimum value for <paramref name="whsize"/> is 2; typically it is >= 7.</item>
    ///     <item>The local statistics, measured over the window, are the average and standard deviation.</item>
    ///     <item>
    ///     The measurements of the mean and standard deviation are performed inside a border of (<paramref name="whsize"/> + 1) pixels.
    ///     If source pix does not have these added border pixels, use <paramref name="addborder"/> = <c>True</c> to add it here; otherwise use
    ///     <paramref name="addborder"/> = <c>False</c>.
    ///     </item>
    ///     <item>
    ///     The Sauvola threshold is determined from the formula:  t = m * (1 - k * (1 - s / 128)) where t = local threshold, m = local mean,
    ///     k = <paramref name="factor"/>, and s = local standard deviation which is maximised at 127.5 when half the samples are 0 and the other
    ///     half are 255.
    ///     </item>
    ///     <item>
    ///     The basic idea of Niblack and Sauvola binarization is that the local threshold should be less than the median value,
    ///     and the larger the variance, the closer to the median it should be chosen. Typical values for k are between 0.2 and 0.5.
    ///     </item>
    /// </list>
    /// </remarks>
    /// <param name="whsize">the window half-width for measuring local statistics.</param>
    /// <param name="factor">The factor for reducing threshold due to variances greater than or equal to zero (0). Typically around 0.35.</param>
    /// <param name="addborder">If <c>True</c> add a border of width (<paramref name="whsize"/> + 1) on all sides.</param>
    /// <returns>The binarized image.</returns>
    /// <exception cref="InvalidOperationException">Depth or Colormap had invalid value.</exception>
    /// <exception cref="ArgumentException">whsize or factor had invalid value.</exception>
    /// <exception cref="LeptonicaException">Failed to binarize image.</exception>
    public Pix BinarizeSauvola(int whsize, float factor, bool addborder)
    {
        ValidateObjectValues(whsize, factor);

        int result = LeptonicaApi.PixSauvolaBinarize(Handle, whsize, factor, addborder ? 1 : 0,
            out IntPtr ppixm, out IntPtr ppixsd, out IntPtr ppixth, out IntPtr ppixd);

        // Free memory held by other unused pix's

        if (ppixm != IntPtr.Zero)
        {
            LeptonicaApi.PixDestroy(ref ppixm);
        }
        if (ppixsd != IntPtr.Zero)
        {
            LeptonicaApi.PixDestroy(ref ppixsd);
        }
        if (ppixth != IntPtr.Zero)
        {
            LeptonicaApi.PixDestroy(ref ppixth);
        }
        if (result is not 0)
        {
            throw new LeptonicaException("Failed to binarize image, result was not 0.");
        }
        if (ppixd == IntPtr.Zero)
        {
            throw new LeptonicaException("Failed to binarize image, result ptr was zero.");
        }

        return new(ppixd);
    }

    /// <summary>
    /// Binarization of the input image using the Sauvola local thresholding method on tiles
    /// of the source image.
    ///
    /// Note: The source image must be 8 bpp grayscale; not colormapped.
    /// </summary>
    /// <remarks>
    /// A tiled version of Sauvola can become neccisary for large source images (over 16M pixels) because:
    ///
    /// * The mean value accumulator is a uint32, overflow can occur for an image with more than 16M pixels.
    /// * The mean value accumulator array for 16M pixels is 64 MB. While the mean square accumulator array for 16M pixels is 128 MB.
    ///   Using tiles reduces the size of these arrays.
    /// * Each tile can be processed independently, in parallel, on a multicore processor.
    /// </remarks>
    /// <param name="windowHalfWidth">The window half-width for measuring local statistics</param>
    /// <param name="factor">The factor for reducing threshold due to variances greater than or equal to zero (0). Typically around 0.35.</param>
    /// <param name="nx">The number of tiles to subdivide the source image into on the x-axis.</param>
    /// <param name="ny">The number of tiles to subdivide the source image into on the y-axis.</param>
    /// <returns>The binarized image.</returns>
    /// <exception cref="InvalidOperationException">Depth is not 8 or Colormap is null</exception>
    /// <exception cref="ArgumentException">windowHalfWidth or factor has invalid value.</exception>
    /// <exception cref="LeptonicaException">Could not binarize image.</exception>
    public Pix BinarizeSauvolaTiled(int windowHalfWidth, float factor, int nx, int ny)
    {
        ValidateObjectValues(windowHalfWidth, factor);

        int result = LeptonicaApi.PixSauvolaBinarizeTiled(Handle, windowHalfWidth, factor, nx, ny, out IntPtr ppixth, out IntPtr ppixd);

        // Free memory held by other unused pix's
        if (ppixth != IntPtr.Zero)
        {
            LeptonicaApi.PixDestroy(ref ppixth);
        }

        if (result is not 0)
        {
            throw new LeptonicaException("Failed to binarize image, result was not 0.");
        }
        if (ppixd == IntPtr.Zero)
        {
            throw new LeptonicaException("Failed to binarize image, result ptr was zero.");
        }
        return new(ppixd);
    }

    /// <summary>
    /// Conversion from RBG to 8bpp grayscale.
    /// </summary>
    /// <returns>The Grayscale pix.</returns>
    /// <exception cref="LeptonicaException">Could not convert image to gray.</exception>
    public Pix ConvertRGBToGray()
    {
        return ConvertRGBToGray(0, 0, 0);
    }

    /// <summary>
    /// Conversion from RBG to 8bpp grayscale using the specified weights. Note red, green, blue weights should add up to 1.0.
    /// </summary>
    /// <param name="redWeight">Red weight</param>
    /// <param name="greenWeight">Green weight</param>
    /// <param name="blueWeight">Blue weight</param>
    /// <returns>The Grayscale pix.</returns>
    /// <exception cref="InvalidOperationException">
    /// Depth is not 32 OR 
    /// redWeight, greenWeight or blueWeight has value smaller than 0.
    /// </exception>
    /// <exception cref="LeptonicaException">Could not convert image to gray.</exception>
    public Pix ConvertRGBToGray(float redWeight, float greenWeight, float blueWeight)
    {
        if (Depth is not 32)
        {
            throw new InvalidOperationException("The source image must have a depth of 32 (32 bpp).");
        }
        if (redWeight < 0)
        {
            throw new InvalidOperationException("All weights must be greater than or equal to zero; red was not.");
        }
        if (greenWeight < 0)
        {
            throw new InvalidOperationException("All weights must be greater than or equal to zero; green was not.");
        }
        if (blueWeight < 0)
        {
            throw new InvalidOperationException("All weights must be greater than or equal to zero; blue was not.");
        }

        var resultPixHandle = LeptonicaApi.PixConvertRGBToGray(Handle, redWeight, greenWeight, blueWeight);
        if (resultPixHandle == IntPtr.Zero)
        {
            throw new LeptonicaException("Failed to convert image to grayscale.");
        }
        return new(resultPixHandle);
    }

    /// <summary>
    /// Removes horizontal lines from a grayscale image. 
    /// The algorithm is based on Leptonica <code>lineremoval.c</code> example.
    /// See <a href="http://www.leptonica.com/line-removal.html">line-removal</a>.
    /// </summary>
    /// <returns>image with lines removed</returns>
    /// <exception cref="TesseractException">Failed to remove lines from image.</exception>
    public Pix RemoveLines()
    {
        IntPtr pix1, pix2, pix3, pix4, pix5, pix6, pix7, pix8, pix9;
        pix1 = pix2 = pix3 = pix4 = pix5 = pix6 = pix7 = pix9 = IntPtr.Zero;

        try
        {
            /* threshold to binary, extracting much of the lines */
            pix1 = LeptonicaApi.PixThresholdToBinary(Handle, 170);

            /* find the skew angle and deskew using an interpolated
             * rotator for anti-aliasing (to avoid jaggies) */
            LeptonicaApi.PixFindSkew(new HandleRef(this, pix1), out float angle, out float conf);
            pix2 = LeptonicaApi.PixRotateAMGray(Handle, (float)(Deg2Rad * angle), 255);

            /* extract the lines to be removed */
            pix3 = LeptonicaApi.PixCloseGray(new HandleRef(this, pix2), 51, 1);

            /* solidify the lines to be removed */
            pix4 = LeptonicaApi.PixErodeGray(new HandleRef(this, pix3), 1, 5);

            /* clean the background of those lines */
            pix5 = LeptonicaApi.PixThresholdToValue(new HandleRef(this, IntPtr.Zero), new HandleRef(this, pix4), 210, 255);

            pix6 = LeptonicaApi.PixThresholdToValue(new HandleRef(this, IntPtr.Zero), new HandleRef(this, pix5), 200, 0);

            /* get paint-through mask for changed pixels */
            pix7 = LeptonicaApi.PixThresholdToBinary(new HandleRef(this, pix6), 210);

            /* add the inverted, cleaned lines to orig.  Because
             * the background was cleaned, the inversion is 0,
             * so when you add, it doesn't lighten those pixels.
             * It only lightens (to white) the pixels in the lines! */
            LeptonicaApi.PixInvert(new HandleRef(this, pix6), new HandleRef(this, pix6));
            pix8 = LeptonicaApi.PixAddGray(new HandleRef(this, IntPtr.Zero), new HandleRef(this, pix2), new HandleRef(this, pix6));

            pix9 = LeptonicaApi.PixOpenGray(new HandleRef(this, pix8), 1, 9);

            LeptonicaApi.PixCombineMasked(new HandleRef(this, pix8), new HandleRef(this, pix9), new HandleRef(this, pix7));
            if (pix8 == IntPtr.Zero)
            {
                throw new TesseractException("Failed to remove lines from image.");
            }

            return new(pix8);
        }
        finally
        {
            // destroy any created intermediate pix's, regardless of if the process 
            // failed for any reason.
            if (pix1 != IntPtr.Zero)
            {
                LeptonicaApi.PixDestroy(ref pix1);
            }
            if (pix2 != IntPtr.Zero)
            {
                LeptonicaApi.PixDestroy(ref pix2);
            }
            if (pix3 != IntPtr.Zero)
            {
                LeptonicaApi.PixDestroy(ref pix3);
            }
            if (pix4 != IntPtr.Zero)
            {
                LeptonicaApi.PixDestroy(ref pix4);
            }
            if (pix5 != IntPtr.Zero)
            {
                LeptonicaApi.PixDestroy(ref pix5);
            }
            if (pix6 != IntPtr.Zero)
            {
                LeptonicaApi.PixDestroy(ref pix6);
            }
            if (pix7 != IntPtr.Zero)
            {
                LeptonicaApi.PixDestroy(ref pix7);
            }
            if (pix9 != IntPtr.Zero)
            {
                LeptonicaApi.PixDestroy(ref pix9);
            }
        }
    }

    /// <summary>
    /// Reduces speckle noise in image. The algorithm is based on Leptonica
    /// <code>speckle_reg.c</code> example demonstrating morphological method of
    /// removing speckle.
    /// </summary>
    /// <param name="selStr">hit-miss sels in 2D layout; SEL_STR2 and SEL_STR3 are predefined values</param>
    /// <param name="selSize">2 for 2x2, 3 for 3x3</param>
    /// <returns></returns>
    /// <exception cref="TesseractException">Failed to despeckle image, result ptr was zero.</exception>
    public Pix Despeckle(string selStr, int selSize)
    {
        IntPtr pix1, pix2, pix3;
        IntPtr pix4, pix5, pix6;
        IntPtr sel1, sel2;

        /*  Normalize for rapidly varying background */
        pix1 = LeptonicaApi.PixBackgroundNormFlex(Handle, 7, 7, 1, 1, 10);

        /* Remove the background */
        pix2 = LeptonicaApi.PixGammaTRCMasked(new HandleRef(this, IntPtr.Zero), new HandleRef(this, pix1), new HandleRef(this, IntPtr.Zero), 1.0f, 100, 175);

        /* Binarize */
        pix3 = LeptonicaApi.PixThresholdToBinary(new HandleRef(this, pix2), 180);

        /* Remove the speckle noise up to selSize x selSize */
        sel1 = LeptonicaApi.SelCreateFromString(selStr, selSize + 2, selSize + 2, "speckle" + selSize);
        pix4 = LeptonicaApi.PixHMT(new HandleRef(this, IntPtr.Zero), new HandleRef(this, pix3), new HandleRef(this, sel1));
        sel2 = LeptonicaApi.SelCreateBrick(selSize, selSize, 0, 0, SelType.SEL_HIT);
        pix5 = LeptonicaApi.PixDilate(new HandleRef(this, IntPtr.Zero), new HandleRef(this, pix4), new HandleRef(this, sel2));
        pix6 = LeptonicaApi.PixSubtract(new HandleRef(this, IntPtr.Zero), new HandleRef(this, pix3), new HandleRef(this, pix5));

        LeptonicaApi.SelDestroy(ref sel1);
        LeptonicaApi.SelDestroy(ref sel2);

        LeptonicaApi.PixDestroy(ref pix1);
        LeptonicaApi.PixDestroy(ref pix2);
        LeptonicaApi.PixDestroy(ref pix3);
        LeptonicaApi.PixDestroy(ref pix4);
        LeptonicaApi.PixDestroy(ref pix5);

        if (pix6 == IntPtr.Zero)
        {
            throw new TesseractException("Failed to despeckle image.");
        }

        return new(pix6);
    }

    /// <summary>
    /// Determines the scew angle and if confidence is high enough returns the descewed image as the result, otherwise returns clone of original image.
    /// </summary>
    /// <remarks>
    /// This binarizes if necessary and finds the skew angle.  If the
    /// angle is large enough and there is sufficient confidence,
    /// it returns a deskewed image; otherwise, it returns a clone.
    /// </remarks>
    /// <returns>Returns deskewed image if confidence was high enough, otherwise returns clone of original pix.</returns>
    /// <exception cref="TesseractException">Failed to deskew, output ptr was zero.</exception>
    public Pix Deskew() => Deskew(DefaultBinarySearchReduction, out var _);

    /// <summary>
    /// Determines the scew angle and if confidence is high enough returns the descewed image as the result, otherwise returns clone of original image.
    /// </summary>
    /// <remarks>
    /// This binarizes if necessary and finds the skew angle.  If the
    /// angle is large enough and there is sufficient confidence,
    /// it returns a deskewed image; otherwise, it returns a clone.
    /// </remarks>
    /// <param name="scew">The scew angle and confidence</param>
    /// <returns>Returns deskewed image if confidence was high enough, otherwise returns clone of original pix.</returns>
    /// <exception cref="TesseractException">Failed to deskew, output ptr was zero.</exception>
    public Pix Deskew(out Scew scew) => Deskew(DefaultBinarySearchReduction, out scew);

    /// <summary>
    /// Determines the scew angle and if confidence is high enough returns the descewed image as the result, otherwise returns clone of original image.
    /// </summary>
    /// <remarks>
    /// This binarizes if necessary and finds the skew angle.  If the
    /// angle is large enough and there is sufficient confidence,
    /// it returns a deskewed image; otherwise, it returns a clone.
    /// </remarks>
    /// <param name="redSearch">The reduction factor used by the binary search, can be 1, 2, or 4.</param>
    /// <param name="scew">The scew angle and confidence</param>
    /// <returns>Returns deskewed image if confidence was high enough, otherwise returns clone of original pix.</returns>
    /// <exception cref="TesseractException">Failed to deskew, output ptr was zero.</exception>
    public Pix Deskew(int redSearch, out Scew scew)
    {
        return Deskew(ScewSweep.Default, redSearch, DefaultBinaryThreshold, out scew);
    }

    /// <summary>
    /// Determines the scew angle and if confidence is high enough returns the descewed image as the result, otherwise returns clone of original image.
    /// </summary>
    /// <remarks>
    /// This binarizes if necessary and finds the skew angle.  If the
    /// angle is large enough and there is sufficient confidence,
    /// it returns a deskewed image; otherwise, it returns a clone.
    /// </remarks>
    /// <param name="sweep">linear sweep parameters</param>
    /// <param name="redSearch">The reduction factor used by the binary search, can be 1, 2, or 4.</param>
    /// <param name="thresh">The threshold value used for binarizing the image.</param>
    /// <param name="scew">The scew angle and confidence</param>
    /// <returns>Returns deskewed image if confidence was high enough, otherwise returns clone of original pix.</returns>
    /// <exception cref="TesseractException">Failed to deskew, output ptr was zero.</exception>
    public Pix Deskew(ScewSweep sweep, int redSearch, int thresh, out Scew scew)
    {
        IntPtr resultPixHandle = LeptonicaApi.PixDeskewGeneral(Handle, sweep.Reduction, sweep.Range, sweep.Delta, redSearch, thresh, out float pAngle, out float pConf);
        if (resultPixHandle == IntPtr.Zero)
        {
            throw new TesseractException("Failed to deskew image.");
        }

        scew = new Scew(pAngle, pConf);
        return new(resultPixHandle);
    }

    /// <summary>
    /// Creates a new image by rotating this image about it's centre.
    /// </summary>
    /// <remarks>
    /// Please note the following:
    /// <list type="bullet">
    /// <item>
    /// Rotation will bring in either white or black pixels, as specified by <paramref name="fillColor" /> from
    /// the outside as required.
    /// </item>
    /// <item>Above 20 degrees, sampling rotation will be used if shear was requested.</item>
    /// <item>Colormaps are removed for rotation by area map and shear.</item>
    /// <item>
    /// The resulting image can be expanded so that no image pixels are lost. To invoke expansion,
    /// input the original width and height. For repeated rotation, use of the original width and heigh allows
    /// expansion to stop at the maximum required size which is a square of side = sqrt(w*w + h*h).
    /// </item>
    /// </list>
    /// <para>
    /// Please note there is an implicit assumption about RGB component ordering.
    /// </para>
    /// </remarks>
    /// <param name="angleInRadians">The angle to rotate by, in radians; clockwise is positive.</param>
    /// <param name="method">The rotation method to use.</param>
    /// <param name="fillColor">The fill color to use for pixels that are brought in from the outside.</param>
    /// <param name="width">The original width; use 0 to avoid embedding</param>
    /// <param name="height">The original height; use 0 to avoid embedding</param>
    /// <returns>The image rotated around it's centre.</returns>
    /// <exception cref="ArgumentNullException">If cannot construct Pix from return value handle.</exception>
    /// <exception cref="LeptonicaException">Cannot rotate image.</exception>
    public Pix Rotate(float angleInRadians, RotationMethod method = RotationMethod.AreaMap, RotationFill fillColor = RotationFill.White, int? width = null, int? height = null)
    {
        width ??= Width;
        height ??= Height;

        if (Math.Abs(angleInRadians) < VerySmallAngle)
        {
            return Clone();
        }

        IntPtr resultHandle;

        var rotations = 2 * angleInRadians / Math.PI;
        if (Math.Abs(rotations - Math.Floor(rotations)) < VerySmallAngle)
        {
            // handle special case of orthoganal rotations (90, 180, 270)
            resultHandle = LeptonicaApi.PixRotateOrth(Handle, (int)rotations);
        }
        else
        {
            // handle general case
            resultHandle = LeptonicaApi.PixRotate(Handle, angleInRadians, method, fillColor, width.Value, height.Value);
        }

        if (resultHandle == IntPtr.Zero)
        {
            throw new LeptonicaException("Failed to rotate image around its centre.");
        }

        return new(resultHandle);
    }

    /// <summary>
    /// 90 degree rotation.
    /// </summary>
    /// <param name="direction">1 = clockwise,  -1 = counter-clockwise</param>
    /// <returns>rotated image</returns>
    /// <exception cref="LeptonicaException">Failed to rotate.</exception>
    public Pix Rotate90(int direction)
    {
        IntPtr resultHandle = LeptonicaApi.PixRotate90(Handle, direction);

        if (resultHandle == IntPtr.Zero)
        {
            throw new LeptonicaException("Failed to rotate image.");
        }
        return new(resultHandle);
    }

    /// <summary>
    /// Inverts pix.
    /// </summary>
    /// <exception cref="LeptonicaException">Failed to invert.</exception>
    public Pix Invert()
    {
        IntPtr resultHandle = LeptonicaApi.PixInvert(new HandleRef(this, IntPtr.Zero), Handle);

        if (resultHandle == IntPtr.Zero)
        {
            throw new LeptonicaException("Failed to invert image.");
        }
        return new(resultHandle);
    }

    /// <summary>
    /// Top-level conversion to 8 bits per pixel.
    /// </summary>
    /// <param name="cmapflag"></param>
    /// <exception cref="LeptonicaException">Failed to convert to 8 bits per pixel.</exception>
    public Pix ConvertTo8(int cmapflag)
    {
        IntPtr resultHandle = LeptonicaApi.PixConvertTo8(Handle, cmapflag);
        if (resultHandle == IntPtr.Zero)
        {
            throw new LeptonicaException("Failed to convert image to 8 bpp.");
        }
        return new(resultHandle);
    }

    /// <summary>
    /// Scales the current pix by the specified <paramref name="scaleX"/> and <paramref name="scaleY"/> factors returning a new <see cref="Pix"/> of the areSame depth. 
    /// </summary>
    /// <param name="scaleX"></param>
    /// <param name="scaleY"></param>
    /// <returns>The scaled image.</returns>
    /// <remarks>
    /// <para>
    ///  This function scales 32 bpp RGB; 2, 4 or 8 bpp palette color;
    /// 2, 4, 8 or 16 bpp gray; and binary images.
    /// </para>
    /// <para>
    /// When the input has palette color, the colormap is removed and
    /// the result is either 8 bpp gray or 32 bpp RGB, depending on whether
    /// the colormap has color entries.  Imaging with 2, 4 or 16 bpp are
    /// converted to 8 bpp.
    /// </para>
    /// <para>
    /// Because Scale() is meant to be a very simple interface to a
    /// number of scaling functions, including the use of unsharp masking,
    /// the type of scaling and the sharpening parameters are chosen
    /// by default.  Grayscale and color images are scaled using one
    /// of four methods, depending on the scale factors:
    /// <list type="number">
    /// <item>
    /// <description>
    /// antialiased subsampling (lowpass filtering followed by
    /// subsampling, implemented here by area mapping), for scale factors
    /// less than 0.2
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// antialiased subsampling with sharpening, for scale factors
    /// between 0.2 and 0.7.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// linear interpolation with sharpening, for scale factors between
    /// 0.7 and 1.4.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// linear interpolation without sharpening, for scale factors >= 1.4.
    /// </description>
    /// </item>
    /// </list>
    /// One could use subsampling for scale factors very close to 1.0,
    /// because it preserves sharp edges.  Linear interpolation blurs
    /// edges because the dest pixels will typically straddle two src edge
    /// pixels.  Subsmpling removes entire columns and rows, so the edge is
    /// not blurred.  However, there are two reasons for not doing 
    /// First, it moves edges, so that a straight line at a large angle to
    /// both horizontal and vertical will have noticable kinks where
    /// horizontal and vertical rasters are removed.  Second, although it
    /// is very fast, you get good results on sharp edges by applying
    /// a sharpening filter.
    /// </para>
    /// <para>
    /// For images with sharp edges, sharpening substantially improves the
    /// image quality for scale factors between about 0.2 and about 2.0.
    /// pixScale() uses a small amount of sharpening by default because
    /// it strengthens edge pixels that are weak due to anti-aliasing.
    /// The default sharpening factors are:
    /// <list type="bullet">
    /// <item>
    /// <description><![CDATA[for scaling factors < 0.7:   sharpfract = 0.2    sharpwidth = 1]]></description>
    /// </item>
    /// <item>
    /// <description>for scaling factors >= 0.7:  sharpfract = 0.4    sharpwidth = 2</description>
    /// </item>
    /// </list>
    /// The cases where the sharpening halfwidth is 1 or 2 have special
    /// implementations and are about twice as fast as the general case.
    /// </para>
    /// <para>
    /// However, sharpening is computationally expensive, and one needs
    /// to consider the speed-quality tradeoff:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// For upscaling of RGB images, linear interpolation plus default
    /// sharpening is about 5 times slower than upscaling alone.</description>
    /// </item>
    /// <item>
    /// <description>
    /// For downscaling, area mapping plus default sharpening is
    /// about 10 times slower than downscaling alone.
    /// </description>
    /// </item>
    /// </list>
    /// When the scale factor is larger than 1.4, the cost of sharpening,
    /// which is proportional to image area, is very large compared to the
    /// incremental quality improvement, so we cut off the default use of
    /// sharpening at 1.4.  Thus, for scale factors greater than 1.4,
    /// pixScale() only does linear interpolation.
    /// </para>
    /// <para>
    /// In many situations you will get a satisfactory result by scaling
    /// without sharpening: call pixScaleGeneral() with @sharpfract = 0.0.
    /// Alternatively, if you wish to sharpen but not use the default
    /// value, first call pixScaleGeneral() with @sharpfract = 0.0, and
    /// then sharpen explicitly using pixUnsharpMasking().
    /// </para>
    /// <para>
    /// Binary images are scaled to binary by sampling the closest pixel,
    /// without any low-pass filtering (averaging of neighboring pixels).
    /// This will introduce aliasing for reductions.  Aliasing can be
    /// prevented by using pixScaleToGray() instead.
    /// </para>
    /// <para>
    /// Warning: implicit assumption about RGB component order for LI color scaling
    /// </para>
    ///</remarks>
    ///<exception cref="InvalidOperationException">Failed to load pix.</exception>
    public Pix Scale(float scaleX, float scaleY)
    {
        IntPtr result = LeptonicaApi.PixScale(Handle, scaleX, scaleY);

        if (result == IntPtr.Zero)
        {
            throw new InvalidOperationException("Failed to scale pix.");
        }
        return new(result);
    }




    /// <summary>
    /// Check if given object value equals current instance.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>True if obj Pix and has same value</returns>
    /// <exception cref="TesseractException">Leptonica could not compare images.</exception>
    public override bool Equals(object? obj)
    {
        return obj is Pix pix && Equals(pix);
    }
    /// <summary>
    /// Check if same images
    /// </summary>
    /// <param name="other"></param>
    /// <returns>True if pixes are the same, otherwise false.</returns>
    /// <exception cref="TesseractException">Leptonica could not compare images.</exception>
    public bool Equals(Pix? other)
    {
        if (other is null)
        {
            return false;
        }
        if (LeptonicaApi.PixEqual(Handle, other.Handle, out int areSame) is not 0)
        {
            throw new TesseractException("Failed to compare pix");
        }
        return areSame is not 0;
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Handle, Depth, Height, Width, IsDisposed);


    /// <summary>
    /// HMT (with just misses) for speckle up to 2x2
    /// "oooo"
    /// "oC o"
    /// "o  o"
    /// "oooo"
    /// </summary>
    public const string SEL_STR2 = "oooooC oo  ooooo";

    /// <summary>
    /// HMT (with just misses) for speckle up to 3x3
    /// "oC  o"
    /// "o   o"
    /// "o   o"
    /// "ooooo"
    /// </summary>
    public const string SEL_STR3 = "ooooooC  oo   oo   oooooo";

    /// <summary>
    /// A small angle, in radians, for threshold checking. Equal to about 0.06 degrees.
    /// </summary>
    private const float VerySmallAngle = 0.001F;

    private static List<int> AllowedDepths { get; } = new() { 1, 2, 4, 8, 16, 32 };

    /// <summary>
    /// Used to lookup image formats by extension.
    /// </summary>
    private static Dictionary<string, ImageFormat> ImageFormats { get; } = new()
    {
        { ".jpg", ImageFormat.JfifJpeg },
        { ".jpeg", ImageFormat.JfifJpeg },
        { ".gif", ImageFormat.Gif },
        { ".tif", ImageFormat.Tiff },
        { ".tiff", ImageFormat.Tiff },
        { ".png", ImageFormat.Png },
        { ".bmp", ImageFormat.Bmp }
    };



    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        IntPtr pix = Handle.Handle;
        LeptonicaApi.PixDestroy(ref pix);
        Handle = new HandleRef(this, IntPtr.Zero);
    }
}