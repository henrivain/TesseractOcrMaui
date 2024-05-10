using DllImport = TesseractOcrMaui.IOS.TesseractApi;

namespace TesseractOcrMaui.ImportApis;
internal static class TesseractApi
{
    internal static void DeleteStringArray(IntPtr ptr)
     => DllImport.DeleteStringArray(ptr);

    internal static void DeleteIntArray(IntPtr ptr)
        => DllImport.DeleteIntArray(ptr);

    internal static void DeleteString(IntPtr ptr)
        => DllImport.DeleteString(ptr);

    internal static void DeleteApi(HandleRef self)
        => DllImport.DeleteApi(self);

    internal static IntPtr CreateApi()
        => DllImport.CreateApi();

    internal static int BaseApi4Init(HandleRef self, string datapath, string language,
        EngineMode mode, string[] configs, int configLength, string[] optionNames,
        string[] optionValues, UIntPtr optionsSize, bool setOnlyNonDebugParams)
        => DllImport.BaseApi4Init(self, datapath, language, (int)mode, configs, configLength, optionNames,
            optionValues, optionsSize, setOnlyNonDebugParams);

    internal static int BaseApi5Init(HandleRef self, string datapath, int dataSize,
        string language, EngineMode mode, string[] configs, int configSize, string[] optionNames,
        string[] optionValues, UIntPtr optionsSize, bool setOnlyNonDebugParams)
        => DllImport.BaseApi5Init(self, datapath, dataSize, language, (int)mode,
            configs, configSize, optionNames, optionValues, optionsSize, setOnlyNonDebugParams);

    internal static string GetDataPath(HandleRef self)
        => DllImport.GetDataPath(self);

    internal static void SetImage(HandleRef self, HandleRef pixHandle)
        => DllImport.SetImage(self, pixHandle);

    internal static string GetUTF8Text(HandleRef self)
        => DllImport.GetUTF8Text(self);
    internal static IntPtr GetUTF8Text_Ptr(HandleRef self)
        => DllImport.GetUTF8Text_Ptr(self);

    internal static IntPtr GetHOCRText_Ptr(HandleRef self, int pageNumber)
        => DllImport.GetHOCRText_Ptr(self, pageNumber);

    internal static IntPtr GetAltoText_Ptr(HandleRef self, int pageNumber)
        => DllImport.GetAltoText_Ptr(self, pageNumber);

    internal static IntPtr GetTsvText_Ptr(HandleRef self, int pageNumber)
        => DllImport.GetTsvText_Ptr(self, pageNumber);

    internal static IntPtr GetBoxText_Ptr(HandleRef self, int pageNumber)
        => DllImport.GetBoxText_Ptr(self, pageNumber);

    internal static IntPtr GetWordStrBoxText_Ptr(HandleRef self, int pageNumber)
        => DllImport.GetWordStrBoxText_Ptr(self, pageNumber);

    internal static IntPtr GetUNLVText_Ptr(HandleRef self, int pageNumber)
        => DllImport.GetUNLVText_Ptr(self, pageNumber);

    internal static IntPtr GetUnichar_Ptr(HandleRef self, int pageNumber)
        => DllImport.GetUnichar_Ptr(self, pageNumber);

    internal static /*PageIterator*/ IntPtr AnalyseLayoutToPageIterator(HandleRef self)
        => DllImport.AnalyseLayoutToPageIterator(self);

    internal static /*ResultIterator*/ IntPtr GetResultIterator(HandleRef self)
        => DllImport.GetResultIterator(self);

    internal static int[] GetConfidences(HandleRef self)
        => DllImport.GetConfidences(self);

    internal static void Clear(HandleRef self)
        => DllImport.Clear(self);

    internal static void End(HandleRef self)
        => DllImport.End(self);

    internal static int GetSourceResolution(HandleRef self)
        => DllImport.GetSourceResolution(self);

    internal static void SetOutputName(HandleRef self, string name)
        => DllImport.SetOutputName(self, name);

    internal static void SetInputName(HandleRef self, string name)
        => DllImport.SetInputName(self, name);

    internal static string GetInputName(HandleRef self)
        => DllImport.GetInputName(self);

    internal static void SetInputImage(HandleRef self, HandleRef pixHandle)
        => DllImport.SetInputImage(self, pixHandle);

    internal static /*Pix*/ IntPtr GetInputImage(HandleRef self)
        => DllImport.GetInputImage(self);

    internal static void SetPageSegmentationMode(HandleRef self, PageSegmentationMode mode)
        => DllImport.SetPageSegmentationMode(self, (int)mode);

    internal static int Recognize(HandleRef self, HandleRef monitorHandle)
        => DllImport.Recognize(self, monitorHandle);

    internal static /*Pix*/ IntPtr GetThresholdedImage(HandleRef self)
        => DllImport.GetThresholdedImage(self);

    internal static /*String*/ IntPtr GetVersion()
        => DllImport.GetVersion();

    internal static int SetDebugVariable(HandleRef self, string name, string value)
        => DllImport.SetDebugVariable(self, name, value);

    internal static int SetVariable(HandleRef self, string name, string value)
        => DllImport.SetVariable(self, name, value);

    internal static int GetIntVariable(HandleRef self, string name, out int value)
        => DllImport.GetIntVariable(self, name, out value);

    internal static int GetDoubleVariable(HandleRef self, string name, out double value)
        => DllImport.GetDoubleVariable(self, name, out value);

    internal static int GetBoolVariable(HandleRef self, string name, out int value)
        => DllImport.GetBoolVariable(self, name, out value);

    internal static string GetStringVariable(HandleRef self, string name)
        => DllImport.GetStringVariable(self, name);

    internal static int PrintVariablesToFile(HandleRef self, string fileName)
        => DllImport.PrintVariablesToFile(self, fileName);

    internal static int GetMeanConfidence(HandleRef self)
        => DllImport.GetMeanConfidence(self);

}
