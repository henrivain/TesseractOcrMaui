using DllImport = TesseractOcrMaui.IOS.ResultIteratorApi;

namespace TesseractOcrMaui.ImportApis;

internal static class ResultIteratorApi
{
    internal static void Delete(HandleRef handle)
        => DllImport.Delete(handle);
    internal static /*ResultIterator*/ IntPtr Copy(HandleRef handle)
        => DllImport.Copy(handle);
    internal static /*PageIterator*/ IntPtr GetPageIterator(HandleRef handle)
        => DllImport.GetPageIterator(handle);
    internal static bool Next(HandleRef handle, PageIteratorLevel level)
        => DllImport.Next(handle, (int)level);
    internal static IntPtr GetUTF8Text(HandleRef handle, PageIteratorLevel level)
        => DllImport.GetUTF8Text(handle, (int)level);
    internal static float GetConfidence(HandleRef handle, PageIteratorLevel level)
        => DllImport.GetConfidence(handle, (int)level);
    internal static /*string*/ IntPtr GetRecognizedLanguage(HandleRef handle)
        => DllImport.GetRecognizedLanguage(handle);
    internal static /*string*/ IntPtr GetWordFontAttributes(HandleRef handle, out bool isBold, out bool isItalic,
        out bool isUnderlined, out bool isMonospace, out bool isSerif, out bool isSmallCaps,
        out int pointSize, out int fontId)
        => DllImport.GetWordFontAttributes(handle, out isBold, out isItalic, out isUnderlined,
            out isMonospace, out isSerif, out isSmallCaps, out pointSize, out fontId);
}
