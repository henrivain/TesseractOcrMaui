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
    const string DllName = "Use Windows, Android or iOS Platform";
#endif

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorDelete")]
    public static extern void Delete(HandleRef handle);
    
    
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorCopy")]
    public static extern /*TessResultIterator pointer*/ HandleRef Copy(HandleRef handle);
    
    
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorGetPageIterator")]
    public static extern /*TessPageIterator pointer*/ HandleRef GetPageIterator(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorNext")]
    public static extern bool Next(HandleRef handle, PageIteratorLevel level);


    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorGetUTF8Text")]
    public static extern IntPtr GetUTF8Text(HandleRef handle, PageIteratorLevel level);
    
    
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorConfidence")]
    public static extern float GetConfidence(HandleRef handle, PageIteratorLevel level);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorWordRecognitionLanguage")]
    public static extern IntPtr GetRecognizedLanguage(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorWordFontAttributes")]
    public static unsafe extern IntPtr GetWordFontAttributes(HandleRef handle, bool* isBold, bool* isItalic, 
        bool* isUnderlined, bool* isMonospace, bool* isSerif, bool* isSmallCaps, 
        int* pointSize, int* fontId);





}
#endif