#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments

namespace TesseractOcrMAUILib.ImportApis;
internal sealed partial class TesseractApi
{




#if WINDOWS
    const string DllName = @"Platforms\Windows\lib\x86_64\tesseract53.dll";
#elif ANDROID21_0_OR_GREATER
    const string DllName = "libtesseract";
#else
    const string DllName = @"Platform not supported";
#endif



    const CharSet StrEncoding = CharSet.Ansi;

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
    
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetUTF8Text", CharSet = StrEncoding)]
    internal static extern string GetUTF8Text(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIAllWordConfidences")]
    internal static extern int[] GetConfidences(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIClear")]
    internal static extern void Clear(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetSourceYResolution")]
    internal static extern int GetSourceResolution(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetOutputName", CharSet = StrEncoding)]
    internal static extern void SetOutputName(HandleRef handle, string name);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetInputName", CharSet = StrEncoding)]
    internal static extern void SetInputName(HandleRef handle, string name);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetInputName", CharSet = StrEncoding)]
    internal static extern string GetInputName(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetInputImage", CharSet = StrEncoding)]
    internal static extern string SetInputImage(HandleRef handle);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetPageSegMode")]
    internal static extern void SetPageSegmentationMode(HandleRef handle, PageSegmentationMode mode);

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
}
