using DllImport = TesseractOcrMaui.IOS.Imports.ResultIterator_Imports;

namespace TesseractOcrMaui.IOS;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class ResultIteratorApi
{
    public static void Delete(HandleRef handle)
        => DllImport.Delete(handle);
    public static /*TessResultIterator pointer*/ IntPtr Copy(HandleRef handle)
        => DllImport.Copy(handle);
    public static /*TessPageIterator pointer*/ IntPtr GetPageIterator(HandleRef handle)
        => DllImport.GetPageIterator(handle);
    public static bool Next(HandleRef handle, /*PageIteratorLevel*/ int level)
        => DllImport.Next(handle, level);
    public static IntPtr GetUTF8Text(HandleRef handle, /*PageIteratorLevel*/ int level)
        => DllImport.GetUTF8Text(handle, level);
    public static float GetConfidence(HandleRef handle, /*PageIteratorLevel*/ int level)
        => DllImport.GetConfidence(handle, level);
    public static /*string*/ IntPtr GetRecognizedLanguage(HandleRef handle)
        => DllImport.GetRecognizedLanguage(handle);
    public static /*string*/ IntPtr GetWordFontAttributes(HandleRef handle, out bool isBold, out bool isItalic,
        out bool isUnderlined, out bool isMonospace, out bool isSerif, out bool isSmallCaps,
        out int pointSize, out int fontId)
        => DllImport.GetWordFontAttributes(handle, out isBold, out isItalic, out isUnderlined,
            out isMonospace, out isSerif, out isSmallCaps, out pointSize, out fontId);
}
