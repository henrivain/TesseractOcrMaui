#if !IOS

namespace TesseractOcrMaui.ImportApis;


/// <summary>
/// Tess page iterator api, get page iterator handle from <seealso cref="TesseractApi.AnalyseLayoutToPageIterator(HandleRef)"/>
/// </summary>
internal sealed class PageIteratorApi
{
    const string _dllName = Definitions.TesseractDllName;

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorDelete")]
    public static extern void Delete(HandleRef iterHandle);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorCopy")]
    public static extern /*TessPageIterator*/ IntPtr Copy(HandleRef iterHandle);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorBegin")]
    public static extern void Begin(HandleRef iterHandle);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorNext")]
    public static extern bool Next(HandleRef iterHandle, PageIteratorLevel level);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorIsAtBeginningOf")]
    public static extern bool IsAtBeginningOf(HandleRef iterHandle, PageIteratorLevel level);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorIsAtFinalElement")]
    public static extern bool IsAtFinalElement(HandleRef iterHandle, PageIteratorLevel level, PageIteratorLevel element);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorBoundingBox")]
    public static extern bool BoundingBox(HandleRef iterHandle, PageIteratorLevel level,
        out int left, out int top, out int right, out int bottom);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorGetBinaryImage")]
    public static extern /*Pix ptr*/ IntPtr GetBinaryImage(HandleRef iterHandle, PageIteratorLevel level);
    
    
    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorGetImage")]
    public static extern /*Pix ptr*/ IntPtr GetImage(HandleRef iterHandle, PageIteratorLevel level,
        int padding, HandleRef orginalPix, out int left, out int top);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorBaseline")]
    public static extern bool Baseline(HandleRef iterHandle, PageIteratorLevel level,
        out int x1, out int y1, out int x2, out int y2);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorParagraphInfo")]
    public static extern void ParagraphInfo(HandleRef handle, out ParagraphJustification justification,
        out bool isListItem, out bool isCrown, out int firstLineIndent);


}
#endif