
#if !IOS
#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments

namespace TesseractOcrMaui.ImportApis;

internal sealed partial class TesseractApi
{
    const string _dllName = Definitions.TesseractDllName;
    const CharSet _strEncoding = CharSet.Ansi;



    [LibraryImport(_dllName, EntryPoint = "TessDeleteText")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial void DeleteString(IntPtr ptr);

    [LibraryImport(_dllName, EntryPoint = "TessBaseAPICreate")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial IntPtr CreateApi();

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIDelete")]
    public static extern void DeleteApi(HandleRef handle);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIInit4", CharSet = _strEncoding)]
    public extern static int BaseApi4Init(HandleRef handle, string datapath, string language,
        EngineMode mode, string[] configs, int configLength, string[] optionNames,
        string[] optionValues, UIntPtr optionsSize, bool setOnlyNonDebugParams);

    // Note that you must pass dataSize: 0, if you want to use data pata
    // In baseapi.cpp in line 388:
    // std::string datapath = data_size == 0 ? data: language;
    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIInit5", CharSet = _strEncoding)]
    public extern static int BaseApi5Init(HandleRef handle, string datapath, int dataSize,
        string language, EngineMode mode, string[] configs, int configSize, string[] optionNames,
        string[] optionValues, UIntPtr optionsSize, bool setOnlyNonDebugParams);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetDatapath")]
    public static extern string GetDataPath(HandleRef handle);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetImage2")]
    public static extern void SetImage(HandleRef handle, HandleRef pixHandle);

    // This does not work with non acsii characters, use GetUTF8Text_Ptr instead 
    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetUTF8Text", CharSet = _strEncoding)]
    public static extern string GetUTF8Text(HandleRef handle);

    // Remember to delete string after copying, use DeleteString(IntPtr ptr)
    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetUTF8Text")]
    public static extern IntPtr GetUTF8Text_Ptr(HandleRef handle);



    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetHOCRText")]
    public static extern IntPtr GetHOCRText_Ptr(HandleRef handle, int pageNumber);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetAltoText")]
    public static extern IntPtr GetAltoText_Ptr(HandleRef handle, int pageNumber);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetPAGEText")]
    public static extern IntPtr GetPageText_Ptr(HandleRef handle, int pageNumber);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetTsvText")]
    public static extern IntPtr GetTsvText_Ptr(HandleRef handle, int pageNumber);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetBoxText")]
    public static extern IntPtr GetBoxText_Ptr(HandleRef handle, int pageNumber);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetWordStrBoxText")]
    public static extern IntPtr GetWordStrBoxText_Ptr(HandleRef handle, int pageNumber);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetUNLVText")]
    public static extern IntPtr GetUNLVText_Ptr(HandleRef handle, int pageNumber);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetUnichar")]
    public static extern IntPtr GetUnichar_Ptr(HandleRef handle, int unicharId);

    
    /* Tesseract -> baseapi.cpp (to TessBaseAPIAnalyseLayout)
     * WARNING! This class points to data held within the TessBaseAPI class, and
     * therefore can only be used while the TessBaseAPI class still exists and
     * has not been subjected to a call of Init, SetImage, Recognize, Clear, End
     * DetectOS, or anything else that changes the internal PAGE_RES.
     */

    // Gets page iterator handle from TessaApi analysing
    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIAnalyseLayout")]
    public static extern /*TessPageIterator Pointer*/ IntPtr AnalyseLayoutToPageIterator(HandleRef handle);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetIterator")]
    public static extern /* ResultIterator Pointer */ IntPtr GetResultIterator(HandleRef handle);






    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIAllWordConfidences")]
    public static extern int[] GetConfidences(HandleRef handle);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIClear")]
    public static extern void Clear(HandleRef handle);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIEnd")]
    public static extern void End(HandleRef handle);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetSourceYResolution")]
    public static extern int GetSourceResolution(HandleRef handle);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetOutputName", CharSet = _strEncoding)]
    public static extern void SetOutputName(HandleRef handle, string name);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetInputName", CharSet = _strEncoding)]
    public static extern void SetInputName(HandleRef handle, string name);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetInputName", CharSet = _strEncoding)]
    public static extern string GetInputName(HandleRef handle);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetInputImage", CharSet = _strEncoding)]
    public static extern void SetInputImage(HandleRef handle, HandleRef pixHandle);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetInputImage")]
    internal static extern /*pix*/ IntPtr GetInputImage(HandleRef handle);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetPageSegMode")]
    public static extern void SetPageSegmentationMode(HandleRef handle, PageSegmentationMode mode);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIRecognize")]
    public static extern int Recognize(HandleRef handle, HandleRef monitor);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetThresholdedImage")]
    public static extern IntPtr GetThresholdedImage(HandleRef handle);

    [LibraryImport(_dllName, EntryPoint = "TessVersion", StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial IntPtr GetVersion();

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetDebugVariable", CharSet = _strEncoding)]
    public static extern int SetDebugVariable(HandleRef handle, string name, string value);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetVariable", CharSet = _strEncoding)]
    public static extern int SetVariable(HandleRef handle, string name, string value);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetIntVariable", CharSet = _strEncoding)]
    public static extern int GetIntVariable(HandleRef handle, string name, out int value);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetDoubleVariable", CharSet = _strEncoding)]
    public static extern int GetDoubleVariable(HandleRef handle, string name, out double value);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetBoolVariable", CharSet = _strEncoding)]
    public static extern int GetBoolVariable(HandleRef handle, string name, out int value);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetBoolVariable", CharSet = _strEncoding)]
    public static extern string GetStringVariable(HandleRef handle, string name);

    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIPrintVariablesToFile", CharSet = _strEncoding)]
    public static extern int PrintVariablesToFile(HandleRef handle, string fileName);


    [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIMeanTextConf")]
    public static extern int GetMeanConfidence(HandleRef handle);

}

#endif