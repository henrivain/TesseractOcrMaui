namespace TesseractOcrMaui.IOS.Imports;


internal sealed class PageIteratorApi_Imports
{
    const string DllName = "__Internal";

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorDelete")]
    internal static extern void Delete(HandleRef iterHandle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorCopy")]
    internal static extern /*TessPageIterator*/ nint Copy(HandleRef iterHandle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorBegin")]
    internal static extern void Begin(HandleRef iterHandle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorNext")]
    internal static extern bool Next(HandleRef iterHandle, /*PageIteratorLevel*/ int level);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorIsAtBeginningOf")]
    internal static extern bool IsAtBeginningOf(HandleRef iterHandle, /*PageIteratorLevel*/ int level);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorIsAtFinalElement")]
    internal static extern bool IsAtFinalElement(HandleRef iterHandle, /*PageIteratorLevel*/ int level, /*PageIteratorLevel*/ int element);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorBoundingBox")]
    internal static extern bool BoundingBox(HandleRef iterHandle, /*PageIteratorLevel*/ int level,
        out int left, out int top, out int right, out int bottom);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorGetBinaryImage")]
    internal static extern /*Pix ptr*/ nint GetBinaryImage(HandleRef iterHandle, /*PageIteratorLevel*/ int level);


    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorGetImage")]
    internal static extern /*Pix ptr*/ nint GetImage(HandleRef iterHandle, /*PageIteratorLevel*/ int level,
        int padding, HandleRef originalPix, out int left, out int top);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorBaseline")]
    internal static extern bool BaseLine(HandleRef iterHandle, /*PageIteratorLevel*/ int level,
        out int x1, out int y1, out int x2, out int y2);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorParagraphInfo")]
    internal static extern void ParagraphInfo(HandleRef handle, out int justification,
        out bool isListItem, out bool isCrown, out int firstLineIndent);






}
