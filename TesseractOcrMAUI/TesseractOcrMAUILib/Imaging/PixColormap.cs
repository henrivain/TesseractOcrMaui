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
    internal HandleRef Handle { get; private set; }

    internal PixColormap(IntPtr handle)
    {
        Handle = new HandleRef(this, handle);
    }

    public static PixColormap Create(int depth)
    {
        if (!(depth == 1 || depth == 2 || depth == 4 || depth == 8))
        {
            throw new ArgumentOutOfRangeException(nameof(depth), "Depth must be 1, 2, 4, or 8 bpp.");
        }

        IntPtr handle = LeptonicaApi.PixcmapCreate(depth);
        if (handle == IntPtr.Zero)
        {
            throw new InvalidOperationException("Failed to create colormap.");
        }
        return new PixColormap(handle);
    }

    public static PixColormap CreateLinear(int depth, int levels)
    {
        if ((depth == 1 || depth == 2 || depth == 4 || depth == 8) is false)
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
        return new PixColormap(handle);
    }

    public static PixColormap CreateLinear(int depth, bool firstIsBlack, bool lastIsWhite)
    {
        if (!(depth == 1 || depth == 2 || depth == 4 || depth == 8))
        {
            throw new ArgumentOutOfRangeException(nameof(depth), "Depth must be 1, 2, 4, or 8 bpp.");
        }

        IntPtr handle = LeptonicaApi.PixcmapCreateRandom(depth, firstIsBlack ? 1 : 0, lastIsWhite ? 1 : 0);
        if (handle == IntPtr.Zero)
        {
            throw new InvalidOperationException("Failed to create colormap.");
        }
        return new PixColormap(handle);
    }


    public int Depth => LeptonicaApi.PixcmapGetDepth(Handle);
    public int Count => LeptonicaApi.PixcmapGetCount(Handle);
    public int FreeCount => LeptonicaApi.PixcmapGetFreeCount(Handle);

    public bool AddColor(PixColor color)
    {
        return LeptonicaApi.PixcmapAddColor(Handle, color.Red, color.Green, color.Blue) == 0;
    }

    public bool AddNewColor(PixColor color, out int index)
    {
        return LeptonicaApi.PixcmapAddNewColor(Handle, color.Red, color.Green, color.Blue, out index) == 0;
    }

    public bool AddNearestColor(PixColor color, out int index)
    {
        return LeptonicaApi.PixcmapAddNearestColor(Handle, color.Red, color.Green, color.Blue, out index) == 0;
    }

    public bool AddBlackOrWhite(int color, out int index)
    {
        return LeptonicaApi.PixcmapAddBlackOrWhite(Handle, color, out index) == 0;
    }

    public bool SetBlackOrWhite(bool setBlack, bool setWhite)
    {
        return LeptonicaApi.PixcmapSetBlackAndWhite(Handle, setBlack ? 1 : 0, setWhite ? 1 : 0) == 0;
    }

    public bool IsUsableColor(PixColor color)
    {
        if (LeptonicaApi.PixcmapUsableColor(Handle, color.Red, color.Green, color.Blue, out int usable) == 0)
        {
            return usable == 1;
        }
        else
        {
            throw new InvalidOperationException("Failed to detect if color was usable or not.");
        }
    }

    public void Clear()
    {
        if (LeptonicaApi.PixcmapClear(Handle) != 0)
        {
            throw new InvalidOperationException("Failed to clear color map.");
        }
    }

    public PixColor this[int index]
    {
        get
        {
            if (LeptonicaApi.PixcmapGetColor32(Handle, index, out int color) == 0)
            {
                return PixColor.FromRgb((uint)color);
            }
            else
            {
                throw new InvalidOperationException("Failed to retrieve color.");
            }
        }
        set
        {
            if (LeptonicaApi.PixcmapResetColor(Handle, index, value.Red, value.Green, value.Blue) != 0)
            {
                throw new InvalidOperationException("Failed to reset color.");
            }
        }
    }

    public void Dispose()
    {
        IntPtr cmap = Handle.Handle;
        LeptonicaApi.PixcmapDestroy(ref cmap);
        Handle = new HandleRef(this, IntPtr.Zero);
    }
}