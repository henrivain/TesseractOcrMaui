using DllImport = TesseractOcrMaui.IOS.Imports.PageIteratorApi_Imports;

namespace TesseractOcrMaui.IOS;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Class giving access to dll imports
/// </summary>
public static class PageIteratorApi
{
    public static void Delete(HandleRef self)
        => DllImport.Delete(self);

    public static /*TessPageIterator*/ IntPtr Copy(HandleRef self)
        => DllImport.Copy(self);

    public static void Begin(HandleRef self)
        => DllImport.Begin(self);

    public static bool Next(HandleRef self, /*PageIteratorLevel*/ int level)
        => DllImport.Next(self, level);

    public static bool IsAtBeginningOf(HandleRef self, /*PageIteratorLevel*/ int level)
        => DllImport.IsAtBeginningOf(self, level);

    public static bool IsAtFinalElement(HandleRef self, int level, int element)
        => DllImport.IsAtFinalElement(self, /*PageIteratorLevel*/ level, /*PageIteratorLevel*/ element);

    public static bool BoundingBox(HandleRef self, int level,
        out int left, out int top, out int right, out int bottom)
        => DllImport.BoundingBox(self, level, 
            out left, out top, out right, out bottom);

    public static /*Pix ptr*/ IntPtr GetBinaryImage(HandleRef self, /*PageIteratorLevel*/ int level)
        => DllImport.GetBinaryImage(self, level);

    public static /*Pix ptr*/ IntPtr GetImage(HandleRef self, /*PageIteratorLevel*/ int level, 
        int padding, HandleRef originalPix, out int left, out int top)
        => DllImport.GetImage(self, level, 
            padding, originalPix, out left, out top);

    public static bool BaseLine(HandleRef self, /*PageIteratorLevel*/ int level,
        out int x1, out int y1, out int x2, out int y2)
        => DllImport.BaseLine(self, level, out x1, out y1, out x2, out y2);

    public static void ParagraphInfo(HandleRef self, out int justification,
        out bool isListItem, out bool isCrown, out int firstLineIndent)
        => DllImport.ParagraphInfo(self, out justification, 
                out isListItem, out isCrown, out firstLineIndent);

}
