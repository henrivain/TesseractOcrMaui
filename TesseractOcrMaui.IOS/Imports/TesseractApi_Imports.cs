#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments
#pragma warning disable IDE0300 // Simplify collection initialization
#pragma warning disable CS1591 // Missing XML comment for internally visible type or member

namespace TesseractOcrMaui.IOS.Imports;

public sealed partial class TesseractApi_Imports
{
    const string DllName = "__Internal";

    const CharSet StrEncoding = CharSet.Ansi;

    [LibraryImport(DllName, EntryPoint = "TessDeleteTextArray")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    internal static unsafe partial void DeleteStringArray(IntPtr ptr);

    [LibraryImport(DllName, EntryPoint = "TessDeleteIntArray")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    internal static partial void DeleteIntArray(IntPtr ptr);

    [LibraryImport(DllName, EntryPoint = "TessDeleteText")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    internal static partial void DeleteString(IntPtr ptr);

    [LibraryImport(DllName, EntryPoint = "TessBaseAPICreate")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    internal static partial IntPtr CreateApi();

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIDelete")]
    internal static extern void DeleteApi(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIInit4", CharSet = StrEncoding)]
    internal extern static int BaseApi4Init(HandleRef handle, string datapath, string language,
        int mode, string[] configs, int configLength, string[] optionNames,
        string[] optionValues, UIntPtr optionsSize, bool setOnlyNonDebugParams);

    // Note that you must pass dataSize: 0, if you want to use data pata
    // In baseapi.cpp in line 388:
    // std::string datapath = data_size == 0 ? data: language;
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIInit5", CharSet = StrEncoding)]
    internal extern static int BaseApi5Init(HandleRef handle, string datapath, int dataSize,
        string language, int mode, string[] configs, int configSize, string[] optionNames,
        string[] optionValues, UIntPtr optionsSize, bool setOnlyNonDebugParams);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetDatapath")]
    internal static extern string GetDataPath(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetImage2")]
    internal static extern void SetImage(HandleRef handle, HandleRef pixHandle);

    // This does not work with non acsii characters, use GetUTF8Text_Ptr instead 
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetUTF8Text", CharSet = StrEncoding)]
    internal static extern string GetUTF8Text(HandleRef handle);  

    // Remember to delete string after copying, use DeleteString(IntPtr ptr)
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetUTF8Text")]
    internal static extern IntPtr GetUTF8Text_Ptr(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetHOCRText")]
    internal static extern IntPtr GetHOCRText_Ptr(HandleRef handle, int pageNumber);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetAltoText")]
    internal static extern IntPtr GetAltoText_Ptr(HandleRef handle, int pageNumber);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetPAGEText")]
    internal static extern IntPtr GetPageText_Ptr(HandleRef handle, int pageNumber);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetTsvText")]
    internal static extern IntPtr GetTsvText_Ptr(HandleRef handle, int pageNumber);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetBoxText")]
    internal static extern IntPtr GetBoxText_Ptr(HandleRef handle, int pageNumber);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetWordStrBoxText")]
    internal static extern IntPtr GetWordStrBoxText_Ptr(HandleRef handle, int pageNumber);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetUNLVText")]
    internal static extern IntPtr GetUNLVText_Ptr(HandleRef handle, int pageNumber);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetUnichar")]
    internal static extern IntPtr GetUnichar_Ptr(HandleRef handle, int unicharId);

    /* Tesseract -> baseapi.cpp (to TessBaseAPIAnalyseLayout)
     * WARNING! This class points to data held within the TessBaseAPI class, and
     * therefore can only be used while the TessBaseAPI class still exists and
     * has not been subjected to a call of Init, SetImage, Recognize, Clear, End
     * DetectOS, or anything else that changes the internal PAGE_RES.
     */

    // Gets page iterator handle from TessaApi analysing
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIAnalyseLayout")]
    internal static extern /*TessPageIterator Pointer*/ IntPtr AnalyseLayoutToPageIterator(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetIterator")]
    internal static extern /* ResultIterator Pointer */ IntPtr GetResultIterator(HandleRef handle);


    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIAllWordConfidences")]
    internal static extern int[] GetConfidences(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIClear")]
    internal static extern void Clear(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIEnd")]
    internal static extern void End(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetSourceYResolution")]
    internal static extern int GetSourceResolution(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetOutputName", CharSet = StrEncoding)]
    internal static extern void SetOutputName(HandleRef handle, string name);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetInputName", CharSet = StrEncoding)]
    internal static extern void SetInputName(HandleRef handle, string name);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetInputName", CharSet = StrEncoding)]
    internal static extern string GetInputName(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetInputImage", CharSet = StrEncoding)]
    internal static extern void SetInputImage(HandleRef handle, HandleRef pixHandle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetInputImage")]
    internal static extern /*pix*/ IntPtr GetInputImage(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetPageSegMode")]
    internal static extern void SetPageSegmentationMode(HandleRef handle, int mode);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIRecognize")]
    internal static extern int Recognize(HandleRef handle, HandleRef monitor);
    
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetThresholdedImage")]
    internal static extern IntPtr GetThresholdedImage(HandleRef handle);

    [LibraryImport(DllName, EntryPoint = "TessVersion", StringMarshalling = StringMarshalling.Custom, 
        StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    internal static partial IntPtr GetVersion();

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetDebugVariable", CharSet = StrEncoding)]
    internal static extern int SetDebugVariable(HandleRef handle, string name, string value);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetVariable", CharSet = StrEncoding)]
    internal static extern int SetVariable(HandleRef handle, string name, string value);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetIntVariable", CharSet = StrEncoding)]
    internal static extern int GetIntVariable(HandleRef handle, string name, out int value);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetDoubleVariable", CharSet = StrEncoding)]
    internal static extern int GetDoubleVariable(HandleRef handle, string name, out double value);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetBoolVariable", CharSet = StrEncoding)]
    internal static extern int GetBoolVariable(HandleRef handle, string name, out int value);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetBoolVariable", CharSet = StrEncoding)]
    internal static extern string GetStringVariable(HandleRef handle, string name);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIPrintVariablesToFile", CharSet = StrEncoding)]
    internal static extern int PrintVariablesToFile(HandleRef handle, string fileName);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIMeanTextConf")]
    internal static extern int GetMeanConfidence(HandleRef handle);
 
}
