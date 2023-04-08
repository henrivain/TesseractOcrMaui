// Code copied from https://github.com/charlesw/tesseract
using TesseractOcrMAUILib.ImportApis;

namespace TesseractOcrMAUILib.Imaging;

/// <summary>
/// Represents a colormap.
/// </summary>
/// <remarks>
/// Once the colormap is assigned to a pix it is owned by that pix and will be disposed off automatically 
/// when the pix is disposed off.
/// </remarks>
public sealed class PixColormap : IDisposable
{

    internal PixColormap(IntPtr handle)
    {
        Handle = new HandleRef(this, handle);
    }

    /// <summary>
    /// Depth of the image.
    /// </summary>
    public int Depth => LeptonicaApi.PixcmapGetDepth(Handle);
    public int Count => LeptonicaApi.PixcmapGetCount(Handle);
    public int FreeCount => LeptonicaApi.PixcmapGetFreeCount(Handle);
    internal HandleRef Handle { get; private set; }

    static HashSet<int> ValidDepths { get; } = new() { 1, 2, 4, 8 };


    /// <summary>
    /// Create colormap with given depth.
    /// </summary>
    /// <param name="depth"></param>
    /// <returns>PixColorMap with given depth.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Invalid depth.</exception>
    /// <exception cref="InvalidOperationException">Leptonica cannot create colormap.</exception>
    public static PixColormap Create(int depth)
    {
        if (ValidDepths.Contains(depth) is false)
        {
            throw new ArgumentOutOfRangeException(nameof(depth), "Depth must be 1, 2, 4, or 8 bpp.");
        }

        IntPtr handle = LeptonicaApi.PixcmapCreate(depth);
        if (handle == IntPtr.Zero)
        {
            throw new InvalidOperationException("Failed to create colormap.");
        }
        return new(handle);
    }

    /// <summary>
    /// Create linear color map with given depth and levels.
    /// </summary>
    /// <param name="depth"></param>
    /// <param name="levels"></param>
    /// <returns>Colormap with given depth and levels.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Invalid depth.</exception>
    /// <exception cref="InvalidOperationException">Leptonica cannot create colormap.</exception>
    public static PixColormap CreateLinear(int depth, int levels)
    {
        if (ValidDepths.Contains(depth) is false)
        {
            throw new ArgumentOutOfRangeException(nameof(depth), "Depth must be 1, 2, 4, or 8 bpp.");
        }
        if (levels < 2 || levels > 2 << depth)
        {
            throw new ArgumentOutOfRangeException(nameof(levels), "Depth must be 2 and 2^depth (inclusive).");
        }

        IntPtr handle = LeptonicaApi.PixcmapCreateLinear(depth, levels);
        if (handle == IntPtr.Zero)
        {
            throw new InvalidOperationException("Failed to create colormap.");
        }
        return new(handle);
    }

    /// <summary>
    /// Create linear color map.
    /// </summary>
    /// <param name="depth"></param>
    /// <param name="firstIsBlack"></param>
    /// <param name="lastIsWhite"></param>
    /// <returns>Colormap with given values.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Invalid depth.</exception>
    /// <exception cref="InvalidOperationException">Leptonica cannot create colormap.</exception>
    public static PixColormap CreateLinear(int depth, bool firstIsBlack, bool lastIsWhite)
    {
        if (ValidDepths.Contains(depth) is false)
        {
            throw new ArgumentOutOfRangeException(nameof(depth), "Depth must be 1, 2, 4, or 8 bpp.");
        }

        IntPtr handle = LeptonicaApi.PixcmapCreateRandom(depth, firstIsBlack ? 1 : 0, lastIsWhite ? 1 : 0);
        if (handle == IntPtr.Zero)
        {
            throw new InvalidOperationException("Failed to create colormap.");
        }
        return new(handle);
    }


    /// <summary>
    /// Add color to current color map
    /// </summary>
    /// <param name="color"></param>
    /// <returns>True if success, otherwise false.</returns>
    public bool AddColor(PixColor color)
    {
        return LeptonicaApi.PixcmapAddColor(Handle, color.Red, color.Green, color.Blue) is 0;
    }

    /// <summary>
    /// Add new color to current color map.
    /// </summary>
    /// <param name="color"></param>
    /// <param name="index"></param>
    /// <returns>True if success, otherwise false.</returns>
    public bool AddNewColor(PixColor color, out int index)
    {
        return LeptonicaApi.PixcmapAddNewColor(Handle, color.Red, color.Green, color.Blue, out index) == 0;
    }

    /// <summary>
    /// Add color to 
    /// </summary>
    /// <param name="color"></param>
    /// <param name="index"></param>
    /// <returns>True if success, otherwise false.</returns>
    public bool AddNearestColor(PixColor color, out int index)
    {
        return LeptonicaApi.PixcmapAddNearestColor(Handle, color.Red, color.Green, color.Blue, out index) is 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="color"></param>
    /// <param name="index"></param>
    /// <returns>True if success, otherwise false.</returns>
    public bool AddBlackOrWhite(int color, out int index)
    {
        return LeptonicaApi.PixcmapAddBlackOrWhite(Handle, color, out index) is 0;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="setBlack"></param>
    /// <param name="setWhite"></param>
    /// <returns>True if success, otherwise false.</returns>
    public bool SetBlackOrWhite(bool setBlack, bool setWhite)
    {
        return LeptonicaApi.PixcmapSetBlackAndWhite(Handle, setBlack ? 1 : 0, setWhite ? 1 : 0) is 0;
    }

    public bool IsUsableColor(PixColor color)
    {
        if (LeptonicaApi.PixcmapUsableColor(Handle, color.Red, color.Green, color.Blue, out int usable) is 0)
        {
            return usable is 1;
        }
        else
        {
            throw new InvalidOperationException("Failed to detect if color was usable or not.");
        }
    }

    /// <summary>
    /// Clear current color map.
    /// </summary>
    /// <exception cref="InvalidOperationException">Leptonica failed to clear color map.</exception>
    public void Clear()
    {
        if (LeptonicaApi.PixcmapClear(Handle) != 0)
        {
            throw new InvalidOperationException("Failed to clear color map.");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">
    /// [GET] Leptonica cannot retrieve color.
    /// [SET] Leptonica cannot reset color.
    /// </exception>
    public PixColor this[int index]
    {
        get
        {
            if (LeptonicaApi.PixcmapGetColor32(Handle, index, out int color) is 0)
            {
                return PixColor.FromRgb((uint)color);
            }
            throw new InvalidOperationException("Failed to retrieve color.");
        }
        set
        {
            if (LeptonicaApi.PixcmapResetColor(Handle, index, value.Red, value.Green, value.Blue) is not 0)
            {
                throw new InvalidOperationException("Failed to reset color.");
            }
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        IntPtr cmap = Handle.Handle;
        LeptonicaApi.PixcmapDestroy(ref cmap);
        Handle = new HandleRef(this, IntPtr.Zero);
    }
}