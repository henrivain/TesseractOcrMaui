#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments
#pragma warning disable SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time

namespace TesseractOcrMAUILib.ImportApis;
internal sealed partial class LeptonicaApi
{
    const string DllName = @"Platforms\Windows\lib\x86_64\leptonica-1.84.0.dll";

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixAddGray")]
    internal static extern IntPtr PixAddGray(HandleRef handleRef1, HandleRef handleRef2, HandleRef handleRef3);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixBackgroundNormFlex")]
    internal static extern IntPtr PixBackgroundNormFlex(HandleRef handle, int v1, int v2, int v3, int v4, int v5);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixClone")]
    internal static extern IntPtr PixClone(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixCloseGray")]
    internal static extern IntPtr PixCloseGray(HandleRef handleRef, int v1, int v2);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapAddBlackOrWhite")]
    internal static extern int PixcmapAddBlackOrWhite(HandleRef handle, int color, out int index);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapAddColor")]
    internal static extern int PixcmapAddColor(HandleRef handle, byte red, byte green, byte blue);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapAddNearestColor")]
    internal static extern int PixcmapAddNearestColor(HandleRef handle, byte red, byte green, byte blue, out int index);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapAddNewColor")]
    internal static extern int PixcmapAddNewColor(HandleRef handle, byte red, byte green, byte blue, out int index);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapClear")]
    internal static extern int PixcmapClear(HandleRef handle);

    [LibraryImport(DllName, EntryPoint = "pixcmapCreate")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    internal static partial IntPtr PixcmapCreate(int depth);

    [LibraryImport(DllName, EntryPoint = "pixcmapCreateLinear")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    internal static partial IntPtr PixcmapCreateLinear(int depth, int levels);

    [LibraryImport(DllName, EntryPoint = "pixcmapCreateRandom")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    internal static partial IntPtr PixcmapCreateRandom(int depth, int v1, int v2);

    [LibraryImport(DllName, EntryPoint = "pixcmapDestroy")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    internal static partial void PixcmapDestroy(ref IntPtr tmpHandle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapGetColor32")]
    internal static extern int PixcmapGetColor32(HandleRef handle, int index, out int color);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapGetCount")]
    internal static extern int PixcmapGetCount(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapGetDepth")]
    internal static extern int PixcmapGetDepth(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapGetFreeCount")]
    internal static extern int PixcmapGetFreeCount(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapResetColor")]
    internal static extern int PixcmapResetColor(HandleRef handle, int index, byte red, byte green, byte blue);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapSetBlackAndWhite")]
    internal static extern int PixcmapSetBlackAndWhite(HandleRef handle, int v1, int v2);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixcmapUsableColor")]
    internal static extern int PixcmapUsableColor(HandleRef handle, byte red, byte green, byte blue, out int usable);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixCombineMasked")]
    internal static extern void PixCombineMasked(HandleRef handleRef1, HandleRef handleRef2, HandleRef handleRef3);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixConvertRGBToGray")]
    internal static extern IntPtr PixConvertRGBToGray(HandleRef handle, float rwt, float gwt, float bwt);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixConvertTo8")]
    internal static extern IntPtr PixConvertTo8(HandleRef handle, int cmapflag);

    [LibraryImport(DllName, EntryPoint = "pixCreate")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    internal static partial IntPtr PixCreate(int width, int height, int depth);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixDeskewGeneral")]    
    internal static extern IntPtr PixDeskewGeneral(HandleRef handle, int reduction, float range, float delta,
        int redSearch, int thresh, out float pAngle, out float pConf);

    [LibraryImport(DllName, EntryPoint = "pixDestroy")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    internal static partial void PixDestroy(ref IntPtr ppixth);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixDestroyColormap")]
    internal static extern int PixDestroyColormap(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixDilate")]
    internal static extern IntPtr PixDilate(HandleRef handleRef1, HandleRef handleRef2, HandleRef handleRef3);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixEndianByteSwap")]
    internal static extern void PixEndianByteSwap(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixEqual")]
    internal static extern int PixEqual(HandleRef handle1, HandleRef handle2, out int same);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixErodeGray")]
    internal static extern IntPtr PixErodeGray(HandleRef handleRef, int v1, int v2);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixFindSkew")]
    internal static extern void PixFindSkew(HandleRef handleRef, out float angle, out float conf);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixGammaTRCMasked")]
    internal static extern IntPtr PixGammaTRCMasked(HandleRef handleRef1, HandleRef handleRef2, HandleRef handleRef3, float v1, int v2, int v3);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixGetColormap")]
    internal static extern IntPtr PixGetColormap(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixGetData")]
    internal static extern IntPtr PixGetData(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixGetDepth")]
    internal static extern int PixGetDepth(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixGetHeight")]
    internal static extern int PixGetHeight(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixGetWidth")]
    internal static extern int PixGetWidth(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixGetWpl")]
    internal static extern int PixGetWpl(HandleRef handle)
;

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixGetXRes")]
    internal static extern int PixGetXRes(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixGetYRes")]
    internal static extern int PixGetYRes(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixHMT")]
    internal static extern IntPtr PixHMT(HandleRef handleRef1, HandleRef handleRef2, HandleRef handleRef3);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixInvert")]
    internal static extern IntPtr PixInvert(HandleRef handleRef1, HandleRef handleRef2);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixOpenGray")]
    internal static extern IntPtr PixOpenGray(HandleRef handleRef, int v1, int v2);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixOtsuAdaptiveThreshold")]
    internal static extern int PixOtsuAdaptiveThreshold(HandleRef handle, int sx, int sy, int smoothx, int smoothy, float scorefract, out IntPtr ppixth, out IntPtr ppixd);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixRead")]
    internal static extern IntPtr PixRead(string filename);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixReadFromMultipageTiff")]
    internal static extern IntPtr PixReadFromMultipageTiff(string filename, ref int offset);

    [LibraryImport(DllName, EntryPoint = "pixReadMem")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    internal static unsafe partial IntPtr PixReadMem(byte* ptr, int length);

    [LibraryImport(DllName, EntryPoint = "pixReadMemTiff")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    internal static unsafe partial IntPtr PixReadMemTiff(byte* ptr, int length, int v);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixRotate")]
    internal static extern IntPtr PixRotate(HandleRef handle, float angleInRadians, RotationMethod method, RotationFill fillColor, int value1, int value2);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixRotate90")]
    internal static extern IntPtr PixRotate90(HandleRef handle, int direction);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixRotateAMGray")]
    internal static extern IntPtr PixRotateAMGray(HandleRef handle, float v1, byte v2);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixRotateOrth")]
    internal static extern IntPtr PixRotateOrth(HandleRef handle, int rotations);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixSauvolaBinarize")]
    internal static extern int PixSauvolaBinarize(HandleRef handle, int whsize, float factor, int v, out IntPtr ppixm, out IntPtr ppixsd, out IntPtr ppixth, out IntPtr ppixd);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixSauvolaBinarizeTiled")]
    internal static extern int PixSauvolaBinarizeTiled(HandleRef handle, int whsize, float factor, int nx, int ny, out IntPtr ppixth, out IntPtr ppixd);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixScale")]
    internal static extern IntPtr PixScale(HandleRef handle, float scaleX, float scaleY);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixSetColormap")]
    internal static extern int PixSetColormap(HandleRef handle1, HandleRef handle2);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixSetXRes")]
    internal static extern void PixSetXRes(HandleRef handle, int value);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixSetYRes")]
    internal static extern void PixSetYRes(HandleRef handle, int value);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixSubtract")]
    internal static extern IntPtr PixSubtract(HandleRef handleRef1, HandleRef handleRef2, HandleRef handleRef3);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixThresholdToBinary")]
    internal static extern IntPtr PixThresholdToBinary(HandleRef handle, int v);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixThresholdToValue")]
    internal static extern IntPtr PixThresholdToValue(HandleRef handleRef1, HandleRef handleRef2, int v1, int v2);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pixWrite")]
    internal static extern int PixWrite(string filename, HandleRef handle, ImageFormat actualFormat);

    [LibraryImport(DllName, EntryPoint = "selCreateBrick")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    internal static partial IntPtr SelCreateBrick(int selSize1, int selSize2, int v1, int v2, SelType type);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "selCreateFromString")]
    internal static extern IntPtr SelCreateFromString(string selStr, int v1, int v2, string v3);

    [LibraryImport(DllName, EntryPoint = "selDestroy")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    internal static partial void SelDestroy(ref IntPtr sel1);
}
