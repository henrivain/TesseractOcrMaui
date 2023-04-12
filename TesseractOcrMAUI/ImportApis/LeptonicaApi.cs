#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments
#pragma warning disable SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time

namespace TesseractOcrMaui.ImportApis;
internal sealed partial class LeptonicaApi
{
#if WINDOWS
    const string DllName = @"leptonica-1.84.0.dll"; // lib\Windows\x86_64\

#elif ANDROID21_0_OR_GREATER
    const string DllName = "libleptonica";
#else
    const string DllName = "Use Windows or Android Platform";
#endif


    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixAddGray")]
    public static extern IntPtr PixAddGray(HandleRef handleRef1, HandleRef handleRef2, HandleRef handleRef3);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixBackgroundNormFlex")]
    public static extern IntPtr PixBackgroundNormFlex(HandleRef handle, int v1, int v2, int v3, int v4, int v5);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixClone")]
    public static extern IntPtr PixClone(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixCloseGray")]
    public static extern IntPtr PixCloseGray(HandleRef handleRef, int v1, int v2);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapAddBlackOrWhite")]
    public static extern int PixcmapAddBlackOrWhite(HandleRef handle, int color, out int index);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapAddColor")]
    public static extern int PixcmapAddColor(HandleRef handle, byte red, byte green, byte blue);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapAddNearestColor")]
    public static extern int PixcmapAddNearestColor(HandleRef handle, byte red, byte green, byte blue, out int index);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapAddNewColor")]
    public static extern int PixcmapAddNewColor(HandleRef handle, byte red, byte green, byte blue, out int index);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapClear")]
    public static extern int PixcmapClear(HandleRef handle);

    [LibraryImport(DllName, EntryPoint = "pixcmapCreate")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial IntPtr PixcmapCreate(int depth);

    [LibraryImport(DllName, EntryPoint = "pixcmapCreateLinear")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial IntPtr PixcmapCreateLinear(int depth, int levels);

    [LibraryImport(DllName, EntryPoint = "pixcmapCreateRandom")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial IntPtr PixcmapCreateRandom(int depth, int v1, int v2);

    [LibraryImport(DllName, EntryPoint = "pixcmapDestroy")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial void PixcmapDestroy(ref IntPtr tmpHandle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapGetColor32")]
    public static extern int PixcmapGetColor32(HandleRef handle, int index, out int color);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapGetCount")]
    public static extern int PixcmapGetCount(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapGetDepth")]
    public static extern int PixcmapGetDepth(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapGetFreeCount")]
    public static extern int PixcmapGetFreeCount(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapResetColor")]
    public static extern int PixcmapResetColor(HandleRef handle, int index, byte red, byte green, byte blue);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapSetBlackAndWhite")]
    public static extern int PixcmapSetBlackAndWhite(HandleRef handle, int v1, int v2);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapUsableColor")]
    public static extern int PixcmapUsableColor(HandleRef handle, byte red, byte green, byte blue, out int usable);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixCombineMasked")]
    public static extern void PixCombineMasked(HandleRef handleRef1, HandleRef handleRef2, HandleRef handleRef3);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixConvertRGBToGray")]
    public static extern IntPtr PixConvertRGBToGray(HandleRef handle, float rwt, float gwt, float bwt);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixConvertTo8")]
    public static extern IntPtr PixConvertTo8(HandleRef handle, int cmapflag);

    [LibraryImport(DllName, EntryPoint = "pixCreate")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial IntPtr PixCreate(int width, int height, int depth);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixDeskewGeneral")]    
    public static extern IntPtr PixDeskewGeneral(HandleRef handle, int reduction, float range, float delta,
        int redSearch, int thresh, out float pAngle, out float pConf);

    [LibraryImport(DllName, EntryPoint = "pixDestroy")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial void PixDestroy(ref IntPtr ppixth);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixDestroyColormap")]
    public static extern int PixDestroyColormap(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixDilate")]
    public static extern IntPtr PixDilate(HandleRef handleRef1, HandleRef handleRef2, HandleRef handleRef3);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixEndianByteSwap")]
    public static extern void PixEndianByteSwap(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixEqual")]
    public static extern int PixEqual(HandleRef handle1, HandleRef handle2, out int same);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixErodeGray")]
    public static extern IntPtr PixErodeGray(HandleRef handleRef, int v1, int v2);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixFindSkew")]
    public static extern void PixFindSkew(HandleRef handleRef, out float angle, out float conf);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixGammaTRCMasked")]
    public static extern IntPtr PixGammaTRCMasked(HandleRef handleRef1, HandleRef handleRef2, HandleRef handleRef3, float v1, int v2, int v3);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixGetColormap")]
    public static extern IntPtr PixGetColormap(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixGetData")]
    public static extern IntPtr PixGetData(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixGetDepth")]
    public static extern int PixGetDepth(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixGetHeight")]
    public static extern int PixGetHeight(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixGetWidth")]
    public static extern int PixGetWidth(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixGetWpl")]
    public static extern int PixGetWpl(HandleRef handle)
;

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixGetXRes")]
    public static extern int PixGetXRes(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixGetYRes")]
    public static extern int PixGetYRes(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixHMT")]
    public static extern IntPtr PixHMT(HandleRef handleRef1, HandleRef handleRef2, HandleRef handleRef3);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixInvert")]
    public static extern IntPtr PixInvert(HandleRef handleRef1, HandleRef handleRef2);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixOpenGray")]
    public static extern IntPtr PixOpenGray(HandleRef handleRef, int v1, int v2);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixOtsuAdaptiveThreshold")]
    public static extern int PixOtsuAdaptiveThreshold(HandleRef handle, int sx, int sy, int smoothx, int smoothy, float scorefract, out IntPtr ppixth, out IntPtr ppixd);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixRead")]
    public static extern IntPtr PixRead(string filename);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixReadFromMultipageTiff")]
    public static extern IntPtr PixReadFromMultipageTiff(string filename, ref int offset);

    [LibraryImport(DllName, EntryPoint = "pixReadMem")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static unsafe partial IntPtr PixReadMem(byte* ptr, int length);

    [LibraryImport(DllName, EntryPoint = "pixReadMemTiff")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static unsafe partial IntPtr PixReadMemTiff(byte* ptr, int length, int v);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixRotate")]
    public static extern IntPtr PixRotate(HandleRef handle, float angleInRadians, RotationMethod method, RotationFill fillColor, int value1, int value2);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixRotate90")]
    public static extern IntPtr PixRotate90(HandleRef handle, int direction);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixRotateAMGray")]
    public static extern IntPtr PixRotateAMGray(HandleRef handle, float v1, byte v2);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixRotateOrth")]
    public static extern IntPtr PixRotateOrth(HandleRef handle, int rotations);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixSauvolaBinarize")]
    public static extern int PixSauvolaBinarize(HandleRef handle, int whsize, float factor, int v, out IntPtr ppixm, out IntPtr ppixsd, out IntPtr ppixth, out IntPtr ppixd);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixSauvolaBinarizeTiled")]
    public static extern int PixSauvolaBinarizeTiled(HandleRef handle, int whsize, float factor, int nx, int ny, out IntPtr ppixth, out IntPtr ppixd);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixScale")]
    public static extern IntPtr PixScale(HandleRef handle, float scaleX, float scaleY);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixSetColormap")]
    public static extern int PixSetColormap(HandleRef handle1, HandleRef handle2);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixSetXRes")]
    public static extern void PixSetXRes(HandleRef handle, int value);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixSetYRes")]
    public static extern void PixSetYRes(HandleRef handle, int value);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixSubtract")]
    public static extern IntPtr PixSubtract(HandleRef handleRef1, HandleRef handleRef2, HandleRef handleRef3);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixThresholdToBinary")]
    public static extern IntPtr PixThresholdToBinary(HandleRef handle, int v);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixThresholdToValue")]
    public static extern IntPtr PixThresholdToValue(HandleRef handleRef1, HandleRef handleRef2, int v1, int v2);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixWrite")]
    public static extern int PixWrite(string filename, HandleRef handle, ImageFormat actualFormat);

    [LibraryImport(DllName, EntryPoint = "selCreateBrick")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial IntPtr SelCreateBrick(int selSize1, int selSize2, int v1, int v2, SelType type);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "selCreateFromString")]
    public static extern IntPtr SelCreateFromString(string selStr, int v1, int v2, string v3);

    [LibraryImport(DllName, EntryPoint = "selDestroy")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial void SelDestroy(ref IntPtr sel1);
}
