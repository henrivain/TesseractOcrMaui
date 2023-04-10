#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments

namespace TesseractOcrMaui.ImportApis;

internal sealed partial class TesseractApi
{
#if WINDOWS
    const string DllName = @"lib\Windows\x86_64\tesseract53.dll";
#elif ANDROID21_0_OR_GREATER
    const string DllName = "libtesseract";
#else
    const string DllName = "Use Windows or Android Platform";
#endif



    const CharSet StrEncoding = CharSet.Ansi;

    [LibraryImport(DllName, EntryPoint = "TessBaseAPICreate")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial IntPtr CreateApi();

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIDelete")]
    public static extern void DeleteApi(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIInit4", CharSet = StrEncoding)]
    public extern static int BaseApi4Init(HandleRef handle, string datapath, string language,
        int mode, string[] configs, int configLength, string[] optionNames,
        string[] optionValues, UIntPtr optionsSize, bool setOnlyNonDebugParams);

    // Note that you must pass dataSize: 0, if you want to use data pata
    // In baseapi.cpp in line 388:
    // std::string datapath = data_size == 0 ? data: language;
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIInit5", CharSet = StrEncoding)]
    public extern static int BaseApi5Init(HandleRef handle, string datapath, int dataSize,
        string language, int mode, string[] configs, int configSize, string[] optionNames,
        string[] optionValues, UIntPtr optionsSize, bool setOnlyNonDebugParams);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetDatapath")]
    public static extern string GetDataPath(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetImage2")]
    public static extern void SetImage(HandleRef handle, HandleRef pixHandle);
    
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetUTF8Text", CharSet = StrEncoding)]
    public static extern string GetUTF8Text(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIAllWordConfidences")]
    public static extern int[] GetConfidences(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIClear")]
    public static extern void Clear(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetSourceYResolution")]
    public static extern int GetSourceResolution(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetOutputName", CharSet = StrEncoding)]
    public static extern void SetOutputName(HandleRef handle, string name);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetInputName", CharSet = StrEncoding)]
    public static extern void SetInputName(HandleRef handle, string name);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetInputName", CharSet = StrEncoding)]
    public static extern string GetInputName(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetInputImage", CharSet = StrEncoding)]
    public static extern string SetInputImage(HandleRef handle);

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
