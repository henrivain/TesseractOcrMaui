
#if !IOS
#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments

namespace TesseractOcrMaui.ImportApis;

internal sealed partial class TesseractApi
{
#if WINDOWS
    const string DllName = @"tesseract53.dll";
#elif ANDROID21_0_OR_GREATER
    const string DllName = "libtesseract";
#elif IOS
    const string DllName = "This DLL name should never be used, please, file bug report";
#else
#if WINDOWS_OR_WINDOWS_NONMAUI
    const string DllName = @"tesseract53.dll";
#elif LINUX
    const strin DllName = "Linux is not currently supported, please make a feature request.";
#else
    const string DllName = "Use Windows, Android or iOS Platform";
#endif
#endif

    const CharSet StrEncoding = CharSet.Ansi;



    [LibraryImport(DllName, EntryPoint = "TessDeleteText")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial void DeleteString(IntPtr ptr);

    [LibraryImport(DllName, EntryPoint = "TessBaseAPICreate")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial IntPtr CreateApi();

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIDelete")]
    public static extern void DeleteApi(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIInit4", CharSet = StrEncoding)]
    public extern static int BaseApi4Init(HandleRef handle, string datapath, string language,
        EngineMode mode, string[] configs, int configLength, string[] optionNames,
        string[] optionValues, UIntPtr optionsSize, bool setOnlyNonDebugParams);

    // Note that you must pass dataSize: 0, if you want to use data pata
    // In baseapi.cpp in line 388:
    // std::string datapath = data_size == 0 ? data: language;
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIInit5", CharSet = StrEncoding)]
    public extern static int BaseApi5Init(HandleRef handle, string datapath, int dataSize,
        string language, EngineMode mode, string[] configs, int configSize, string[] optionNames,
        string[] optionValues, UIntPtr optionsSize, bool setOnlyNonDebugParams);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetDatapath")]
    public static extern string GetDataPath(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetImage2")]
    public static extern void SetImage(HandleRef handle, HandleRef pixHandle);

    // This does not work with non acsii characters, use GetUTF8Text_Ptr instead 
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetUTF8Text", CharSet = StrEncoding)]
    public static extern string GetUTF8Text(HandleRef handle);

    // Remember to delete string after copying, use DeleteString(IntPtr ptr)
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetUTF8Text")]
    public static extern IntPtr GetUTF8Text_Ptr(HandleRef handle);



    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetHOCRText")]
    public static extern IntPtr GetHOCRText_Ptr(HandleRef handle, int pageNumber);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetAltoText")]
    public static extern IntPtr GetAltoText_Ptr(HandleRef handle, int pageNumber);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetTsvText")]
    public static extern IntPtr GetTsvText_Ptr(HandleRef handle, int pageNumber);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetBoxText")]
    public static extern IntPtr GetBoxText_Ptr(HandleRef handle, int pageNumber);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetWordStrBoxText")]
    public static extern IntPtr GetWordStrBoxText_Ptr(HandleRef handle, int pageNumber);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetUNLVText")]
    public static extern IntPtr GetUNLVText_Ptr(HandleRef handle, int pageNumber);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetUnichar")]
    public static extern IntPtr GetUnichar_Ptr(HandleRef handle, int unicharId);

    
    /* Tesseract -> baseapi.cpp (to TessBaseAPIAnalyseLayout)
     * WARNING! This class points to data held within the TessBaseAPI class, and
     * therefore can only be used while the TessBaseAPI class still exists and
     * has not been subjected to a call of Init, SetImage, Recognize, Clear, End
     * DetectOS, or anything else that changes the internal PAGE_RES.
     */

    // Gets page iterator handle from TessaApi analysing
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIAnalyseLayout")]
    public static extern /*TessPageIterator Pointer*/ IntPtr AnalyseLayoutToPageIterator(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetIterator")]
    public static extern /* ResultIterator Pointer */ IntPtr GetResultIterator(HandleRef handle);






    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIAllWordConfidences")]
    public static extern int[] GetConfidences(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIClear")]
    public static extern void Clear(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIEnd")]
    public static extern void End(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetSourceYResolution")]
    public static extern int GetSourceResolution(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetOutputName", CharSet = StrEncoding)]
    public static extern void SetOutputName(HandleRef handle, string name);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetInputName", CharSet = StrEncoding)]
    public static extern void SetInputName(HandleRef handle, string name);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetInputName", CharSet = StrEncoding)]
    public static extern string GetInputName(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetInputImage", CharSet = StrEncoding)]
    public static extern void SetInputImage(HandleRef handle, HandleRef pixHandle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetInputImage")]
    internal static extern /*pix*/ IntPtr GetInputImage(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetPageSegMode")]
    public static extern void SetPageSegmentationMode(HandleRef handle, PageSegmentationMode mode);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIRecognize")]
    public static extern int Recognize(HandleRef handle, HandleRef monitor);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetThresholdedImage")]
    public static extern IntPtr GetThresholdedImage(HandleRef handle);

    [LibraryImport(DllName, EntryPoint = "TessVersion", StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial IntPtr GetVersion();

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetDebugVariable", CharSet = StrEncoding)]
    public static extern int SetDebugVariable(HandleRef handle, string name, string value);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetVariable", CharSet = StrEncoding)]
    public static extern int SetVariable(HandleRef handle, string name, string value);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetIntVariable", CharSet = StrEncoding)]
    public static extern int GetIntVariable(HandleRef handle, string name, out int value);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetDoubleVariable", CharSet = StrEncoding)]
    public static extern int GetDoubleVariable(HandleRef handle, string name, out double value);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetBoolVariable", CharSet = StrEncoding)]
    public static extern int GetBoolVariable(HandleRef handle, string name, out int value);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetBoolVariable", CharSet = StrEncoding)]
    public static extern string GetStringVariable(HandleRef handle, string name);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIPrintVariablesToFile", CharSet = StrEncoding)]
    public static extern int PrintVariablesToFile(HandleRef handle, string fileName);


    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIMeanTextConf")]
    public static extern int GetMeanConfidence(HandleRef handle);

}

#endif