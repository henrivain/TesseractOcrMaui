namespace TesseractOcrMaui.IOS.Imports;
internal sealed class ResultIterator_Imports
{
    const string DllName = "__Internal";

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorDelete")]
    internal static extern void Delete(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorCopy")]
    internal static extern /*TessResultIterator pointer*/ IntPtr Copy(HandleRef handle);

    /* capi.cpp
     TessPageIterator *TessResultIteratorGetPageIterator(TessResultIterator *handle) {
        return handle;
     }
     */

    // GetPageIterator returns same pointer, it just changes return type to different pointer
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorGetPageIterator")]
    internal static extern /*TessPageIterator pointer*/ IntPtr GetPageIterator(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorNext")]
    internal static extern bool Next(HandleRef handle, /*PageIteratorLevel*/ int level);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorGetUTF8Text")]
    internal static extern IntPtr GetUTF8Text(HandleRef handle, /*PageIteratorLevel*/ int level);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorConfidence")]
    internal static extern float GetConfidence(HandleRef handle, /*PageIteratorLevel*/ int level);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorWordRecognitionLanguage")]
    internal static extern /*string*/ IntPtr GetRecognizedLanguage(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorWordFontAttributes")]
    internal static extern /*string*/ IntPtr GetWordFontAttributes(HandleRef handle, out bool isBold, out bool isItalic,
        out bool isUnderlined, out bool isMonospace, out bool isSerif, out bool isSmallCaps,
        out int pointSize, out int fontId);
}
