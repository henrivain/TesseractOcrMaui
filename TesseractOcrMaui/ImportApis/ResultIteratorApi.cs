#if !IOS

namespace TesseractOcrMaui.ImportApis;

/// <summary>
/// Tess result iterator api, get page iterator handle from <seealso cref="TesseractApi.GetResultIterator(HandleRef)"/>
/// </summary>
internal sealed class ResultIteratorApi
{
    const string _dllName = Definitions.TesseractDllName;

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorDelete")]
    public static extern void Delete(HandleRef handle);
    
    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorCopy")]
    public static extern /*TessResultIterator pointer*/ IntPtr Copy(HandleRef handle);

    /* capi.cpp
     TessPageIterator *TessResultIteratorGetPageIterator(TessResultIterator *handle) {
        return handle;
     }
     */

    // GetPageIterator returns same pointer, it just changes return type to different pointer
    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorGetPageIterator")]
    public static extern /*TessPageIterator pointer*/ IntPtr GetPageIterator(HandleRef handle);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorNext")]
    public static extern bool Next(HandleRef handle, PageIteratorLevel level);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorGetUTF8Text")]
    public static extern IntPtr GetUTF8Text(HandleRef handle, PageIteratorLevel level);
    
    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorConfidence")]
    public static extern float GetConfidence(HandleRef handle, PageIteratorLevel level);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorWordRecognitionLanguage")]
    public static extern /*string*/ IntPtr GetRecognizedLanguage(HandleRef handle);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorWordFontAttributes")]
    public static extern /*string*/ IntPtr GetWordFontAttributes(HandleRef handle, out bool isBold, out bool isItalic, 
        out bool isUnderlined, out bool isMonospace, out bool isSerif, out bool isSmallCaps, 
        out int pointSize, out int fontId);
}
#endif