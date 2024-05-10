#pragma warning disable CS1591 // Missing XML comment for internally visible type or member
using DllImport = TesseractOcrMaui.IOS.Imports.LeptonicaApi_Imports;

namespace TesseractOcrMaui.IOS;
public class LeptonicaApi
{
    public static IntPtr PixAddGray(HandleRef handleRef1, HandleRef handleRef2, HandleRef handleRef3)
        => DllImport.PixAddGray(handleRef1, handleRef2, handleRef3);
    public static IntPtr PixBackgroundNormFlex(HandleRef self, int v1, int v2, int v3, int v4, int v5)
        => DllImport.PixBackgroundNormFlex(self, v1, v2, v3, v4, v5);
    public static IntPtr PixClone(HandleRef self)
        => DllImport.PixClone(self);
    public static IntPtr PixCloseGray(HandleRef self, int v1, int v2)
        => DllImport.PixCloseGray(self, v1, v2);
    public static int PixcmapAddBlackOrWhite(HandleRef self, int color, out int index)
        => DllImport.PixcmapAddBlackOrWhite(self, color, out index);
    public static int PixcmapAddColor(HandleRef handle, byte red, byte green, byte blue)
        => DllImport.PixcmapAddColor(handle, red, green, blue);
    public static int PixcmapAddNearestColor(HandleRef handle, byte red, byte green, byte blue, out int index)
        => DllImport.PixcmapAddNearestColor(handle, red, green, blue, out index);
    public static int PixcmapAddNewColor(HandleRef handle, byte red, byte green, byte blue, out int index)
        => DllImport.PixcmapAddNewColor(handle, red, green, blue, out index);
    public static int PixcmapClear(HandleRef handle)
        => DllImport.PixcmapClear(handle);
    public static IntPtr PixcmapCreate(int depth)
        => DllImport.PixcmapCreate(depth);

    public static IntPtr PixcmapCreateLinear(int depth, int levels)
        => DllImport.PixcmapCreateLinear(depth, levels);
    public static IntPtr PixcmapCreateRandom(int depth, int v1, int v2)
        => DllImport.PixcmapCreateRandom(depth, v1, v2);
    public static void PixcmapDestroy(ref IntPtr tmpHandle)
        => DllImport.PixcmapDestroy(ref tmpHandle);
    public static int PixcmapGetColor32(HandleRef handle, int index, out int color)
        => DllImport.PixcmapGetColor32(handle, index, out color);
    public static int PixcmapGetCount(HandleRef handle)
        => DllImport.PixcmapGetCount(handle);
    public static int PixcmapGetDepth(HandleRef handle)
        => DllImport.PixcmapGetDepth(handle);
    public static int PixcmapGetFreeCount(HandleRef handle)
        => DllImport.PixcmapGetFreeCount(handle);
    public static int PixcmapResetColor(HandleRef handle, int index, byte red, byte green, byte blue)
        => DllImport.PixcmapResetColor(handle, index, red, green, blue);
    public static int PixcmapSetBlackAndWhite(HandleRef handle, int v1, int v2)
        => DllImport.PixcmapSetBlackAndWhite(handle, v1, v2);
    public static int PixcmapUsableColor(HandleRef handle, byte red, byte green, byte blue, out int usable)
        => DllImport.PixcmapUsableColor(handle, red, green, blue, out usable);
    public static void PixCombineMasked(HandleRef handleRef1, HandleRef handleRef2, HandleRef handleRef3)
        => DllImport.PixCombineMasked(handleRef1, handleRef2, handleRef3);
    public static IntPtr PixConvertRGBToGray(HandleRef handle, float rwt, float gwt, float bwt)
        => DllImport.PixConvertRGBToGray(handle, rwt, gwt, bwt);
    public static IntPtr PixConvertTo8(HandleRef handle, int cmapflag)
        => DllImport.PixConvertTo8(handle, cmapflag);
    public static IntPtr PixCreate(int width, int height, int depth)
        => DllImport.PixCreate(width, height, depth);
    public static IntPtr PixDeskewGeneral(HandleRef handle, int reduction, float range, float delta,
        int redSearch, int thresh, out float pAngle, out float pConf)
        => DllImport.PixDeskewGeneral(handle, reduction, range, delta, redSearch, thresh, out pAngle, out pConf);

    public static void PixDestroy(ref IntPtr ppixth)
        => DllImport.PixDestroy(ref ppixth);
    public static int PixDestroyColormap(HandleRef handle)
        => DllImport.PixDestroyColormap(handle);
    public static IntPtr PixDilate(HandleRef handleRef1, HandleRef handleRef2, HandleRef handleRef3)
        => DllImport.PixDilate(handleRef1, handleRef2, handleRef3);
    public static void PixEndianByteSwap(HandleRef handle)
        => DllImport.PixEndianByteSwap(handle);
    public static int PixEqual(HandleRef handle1, HandleRef handle2, out int same)
        => DllImport.PixEqual(handle1, handle2, out same);
    public static IntPtr PixErodeGray(HandleRef handleRef, int v1, int v2)
        => DllImport.PixErodeGray(handleRef, v1, v2);
    public static void PixFindSkew(HandleRef handleRef, out float angle, out float conf)
        => DllImport.PixFindSkew(handleRef, out angle, out conf);

    public static IntPtr PixGammaTRCMasked(HandleRef handleRef1, HandleRef handleRef2, HandleRef handleRef3, float v1, int v2, int v3)
        => DllImport.PixGammaTRCMasked(handleRef1, handleRef2, handleRef3, v1, v2, v3);
    public static IntPtr PixGetColormap(HandleRef handle)
        => DllImport.PixGetColormap(handle);
    public static IntPtr PixGetData(HandleRef handle)
        => DllImport.PixGetData(handle);
    public static int PixGetDepth(HandleRef handle)
        => DllImport.PixGetDepth(handle);
    public static int PixGetHeight(HandleRef handle)
        => DllImport.PixGetHeight(handle);
    public static int PixGetWidth(HandleRef handle)
        => DllImport.PixGetWidth(handle);
    public static int PixGetWpl(HandleRef handle)
        => DllImport.PixGetWpl(handle);
    public static int PixGetXRes(HandleRef handle)
        => DllImport.PixGetXRes(handle);
    public static int PixGetYRes(HandleRef handle)
        => DllImport.PixGetYRes(handle);
    public static IntPtr PixHMT(HandleRef handleRef1, HandleRef handleRef2, HandleRef handleRef3)
        => DllImport.PixHMT(handleRef1, handleRef2, handleRef3);
    public static IntPtr PixInvert(HandleRef handleRef1, HandleRef handleRef2)
        => DllImport.PixInvert(handleRef1, handleRef2);
    public static IntPtr PixOpenGray(HandleRef handleRef, int v1, int v2)
        => DllImport.PixOpenGray(handleRef, v1, v2);
    public static int PixOtsuAdaptiveThreshold(HandleRef handle, int sx, int sy, int smoothx, int smoothy, float scorefract, out IntPtr ppixth, out IntPtr ppixd)
        => DllImport.PixOtsuAdaptiveThreshold(handle, sx, sy, smoothx, smoothy, scorefract, out ppixth, out ppixd);
    public static IntPtr PixRead(string filename)
        => DllImport.PixRead(filename);
    public static IntPtr PixReadFromMultipageTiff(string filename, ref int offset)
        => DllImport.PixReadFromMultipageTiff(filename, ref offset);
    public static unsafe IntPtr PixReadMem(byte* ptr, int length)
        => DllImport.PixReadMem(ptr, length);
    public static unsafe IntPtr PixReadMemTiff(byte* ptr, int length, int v)
        => DllImport.PixReadMemTiff(ptr, length, v);
    public static IntPtr PixRotate(HandleRef handle, float angleInRadians, int method, int fillColor, int value1, int value2)
        => DllImport.PixRotate(handle, angleInRadians, method, fillColor, value1, value2);
    public static IntPtr PixRotate90(HandleRef handle, int direction)
        => DllImport.PixRotate90(handle, direction);
    public static IntPtr PixRotateAMGray(HandleRef handle, float v1, byte v2)
        => DllImport.PixRotateAMGray(handle, v1, v2);
    public static IntPtr PixRotateOrth(HandleRef handle, int rotations)
        => DllImport.PixRotateOrth(handle, rotations);
    public static int PixSauvolaBinarize(HandleRef handle, int whsize, float factor, int v, out IntPtr ppixm, out IntPtr ppixsd, out IntPtr ppixth, out IntPtr ppixd)
        => DllImport.PixSauvolaBinarize(handle, whsize, factor, v, out ppixm, out ppixsd, out ppixth, out ppixd);
    public static int PixSauvolaBinarizeTiled(HandleRef handle, int whsize, float factor, int nx, int ny, out IntPtr ppixth, out IntPtr ppixd)
        => DllImport.PixSauvolaBinarizeTiled(handle, whsize, factor, nx, ny, out ppixth, out ppixd);
    public static IntPtr PixScale(HandleRef handle, float scaleX, float scaleY)
        => DllImport.PixScale(handle, scaleX, scaleY);
    public static int PixSetColormap(HandleRef handle1, HandleRef handle2)
        => DllImport.PixSetColormap(handle1, handle2);
    public static void PixSetXRes(HandleRef handle, int value)
        => DllImport.PixSetXRes(handle, value);
    public static void PixSetYRes(HandleRef handle, int value)
        => DllImport.PixSetYRes(handle, value);
    public static IntPtr PixSubtract(HandleRef handleRef1, HandleRef handleRef2, HandleRef handleRef3)
        => DllImport.PixSubtract(handleRef1, handleRef2, handleRef3);
    public static IntPtr PixThresholdToBinary(HandleRef handle, int v)
        => DllImport.PixThresholdToBinary(handle, v);
    public static IntPtr PixThresholdToValue(HandleRef handleRef1, HandleRef handleRef2, int v1, int v2)
        => DllImport.PixThresholdToValue(handleRef1, handleRef2, v1, v2);
    public static int PixWrite(string filename, HandleRef handle, int actualFormat)
        => DllImport.PixWrite(filename, handle, actualFormat);
    public static IntPtr SelCreateBrick(int selSize1, int selSize2, int v1, int v2, int type)
        => DllImport.SelCreateBrick(selSize1, selSize2, v1, v2, type);
    public static IntPtr SelCreateFromString(string selStr, int v1, int v2, string v3)
        => DllImport.SelCreateFromString(selStr, v1, v2, v3);
    public static void SelDestroy(ref IntPtr sel1)
        => DllImport.SelDestroy(ref sel1);
}
