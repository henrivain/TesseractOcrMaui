#if !IOS

namespace TesseractOcrMaui.ImportApis;


/// <summary>
/// Tess page iterator api, get page iterator handle from <seealso cref="TesseractApi.AnalyseLayoutToPageIterator(HandleRef)"/>
/// </summary>
internal sealed class PageIteratorApi
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

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorDelete")]
    public static extern void Delete(HandleRef iterHandle);


    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorDelete")]
    public static extern /*TessPageIterator*/ IntPtr Copy(HandleRef iterHandle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorBegin")]
    public static extern void Begin(HandleRef iterHandle);


    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorBegin")]
    public static extern bool Next(HandleRef iterHandle, PageIteratorLevel level);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorIsAtBeginningOf")]
    public static extern bool IsAtBeginningOf(HandleRef iterHandle, PageIteratorLevel level);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorIsAtFinalElement")]
    public static extern bool IsAtFinalElement(HandleRef iterHandle, PageIteratorLevel level);


    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorBoundingBox")]
    public static extern unsafe bool BoundingBox(HandleRef iterHandle, PageIteratorLevel level,
                                                    int* left, int* top, int* right, int* bottom);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorGetBinaryImage")]
    public static extern /*Pix ptr*/ IntPtr GetBinaryImage(HandleRef iterHandle, PageIteratorLevel level);
    
    
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorGetImage")]
    public static extern unsafe /*Pix ptr*/ IntPtr GetImage(HandleRef iterHandle, PageIteratorLevel level,
        int padding, HandleRef orginalPix, int* left, int* top);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorBaseline")]
    public static extern unsafe bool Baseline(HandleRef iterHandle, PageIteratorLevel level,
        int* x1, int* y1, int* x2, int* y2);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorParagraphInfo")]
    public static extern unsafe void ParagraphInfo(HandleRef handle, ParagraphJustification* justification,
        bool* isListItem, bool* isCrown, int* firstLineIndent);


}
#endif