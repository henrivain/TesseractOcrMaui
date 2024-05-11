#if !IOS

namespace TesseractOcrMaui.ImportApis;

/// <summary>
/// Tess result iterator api, get page iterator handle from <seealso cref="TesseractApi.GetResultIterator(HandleRef)"/>
/// </summary>
internal sealed class ResultIteratorApi
{
#if WINDOWS
    const string DllName = @"tesseract53.dll";
#elif ANDROID21_0_OR_GREATER
    const string DllName = "libtesseract";
#elif IOS
    const string DllName = "This DLL name should never be used, please, file bug report";
#else
#if WINDOWS_OR_WINDOWS_NONMAUI
    const string DllName = @"Windows\tesseract53.dll";
#elif LINUX
    const strin DllName = "Linux is not currently supported, please make a feature request.";
#else
    const string DllName = "Use Windows, Android or iOS Platform";
#endif
#endif

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorDelete")]
    public static extern void Delete(HandleRef handle);
    
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorCopy")]
    public static extern /*TessResultIterator pointer*/ IntPtr Copy(HandleRef handle);

    /* capi.cpp
     TessPageIterator *TessResultIteratorGetPageIterator(TessResultIterator *handle) {
        return handle;
     }
     */

    // GetPageIterator returns same pointer, it just changes return type to different pointer
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorGetPageIterator")]
    public static extern /*TessPageIterator pointer*/ IntPtr GetPageIterator(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorNext")]
    public static extern bool Next(HandleRef handle, PageIteratorLevel level);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorGetUTF8Text")]
    public static extern IntPtr GetUTF8Text(HandleRef handle, PageIteratorLevel level);
    
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorConfidence")]
    public static extern float GetConfidence(HandleRef handle, PageIteratorLevel level);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorWordRecognitionLanguage")]
    public static extern /*string*/ IntPtr GetRecognizedLanguage(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorWordFontAttributes")]
    public static extern /*string*/ IntPtr GetWordFontAttributes(HandleRef handle, out bool isBold, out bool isItalic, 
        out bool isUnderlined, out bool isMonospace, out bool isSerif, out bool isSmallCaps, 
        out int pointSize, out int fontId);
}
#endif