using DllImport = TesseractOcrMaui.IOS.PageIteratorApi;

namespace TesseractOcrMaui.ImportApis;

internal static class PageIteratorApi
{
    internal static void Delete(HandleRef self)
        => DllImport.Delete(self);

    internal static /*PageIterator*/ IntPtr Copy(HandleRef self)
        => DllImport.Copy(self);

    internal static void Begin(HandleRef self)
        => DllImport.Begin(self);

    internal static bool Next(HandleRef self, PageIteratorLevel level)
        => DllImport.Next(self, (int)level);

    internal static bool IsAtBeginningOf(HandleRef self, PageIteratorLevel level)
        => DllImport.IsAtBeginningOf(self, (int)level);

    internal static bool IsAtFinalElement(HandleRef self, PageIteratorLevel level, PageIteratorLevel element)
        => DllImport.IsAtFinalElement(self, (int)level, (int)element);

    internal static bool BoundingBox(HandleRef self, PageIteratorLevel level,
        out int left, out int top, out int right, out int bottom)
        => DllImport.BoundingBox(self, (int)level,
            out left, out top, out right, out bottom);

    internal static /*Pix ptr*/ IntPtr GetBinaryImage(HandleRef self, PageIteratorLevel level)
        => DllImport.GetBinaryImage(self, (int)level);

    internal static /*Pix ptr*/ IntPtr GetImage(HandleRef self, PageIteratorLevel level,
        int padding, HandleRef originalPix, out int left, out int top)
        => DllImport.GetImage(self, (int)level, padding, originalPix, out left, out top);

    internal static bool BaseLine(HandleRef self, PageIteratorLevel level,
        out int x1, out int y1, out int x2, out int y2)
        => DllImport.BaseLine(self, (int)level, out x1, out y1, out x2, out y2);

    internal static void ParagraphInfo(HandleRef self, out ParagraphJustification justification,
        out bool isListItem, out bool isCrown, out int firstLineIndent)
    {

        DllImport.ParagraphInfo(self, out int int_justification, out isListItem, out isCrown, out firstLineIndent);
        justification = (ParagraphJustification)int_justification;
    }



}
