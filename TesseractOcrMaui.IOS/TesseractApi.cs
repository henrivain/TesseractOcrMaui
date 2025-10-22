#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using DllImport = TesseractOcrMaui.IOS.Imports.TesseractApi_Imports;

namespace TesseractOcrMaui.IOS;

public static class TesseractApi
{
    public static void DeleteStringArray(IntPtr ptr)
     => DllImport.DeleteStringArray(ptr);

    public static void DeleteIntArray(IntPtr ptr)
        => DllImport.DeleteIntArray(ptr);

    public static void DeleteString(IntPtr ptr)
        => DllImport.DeleteString(ptr);

    public static void DeleteApi(HandleRef self)
        => DllImport.DeleteApi(self);

    public static IntPtr CreateApi()
        => DllImport.CreateApi();

    public static int BaseApi4Init(HandleRef self, string datapath, string language,
        int mode, string[] configs, int configLength, string[] optionNames,
        string[] optionValues, UIntPtr optionsSize, bool setOnlyNonDebugParams)
        => DllImport.BaseApi4Init(self, datapath, language, mode, configs, configLength, optionNames,
            optionValues, optionsSize, setOnlyNonDebugParams);

    public static int BaseApi5Init(HandleRef self, string datapath, int dataSize,
        string language, int mode, string[] configs, int configSize, string[] optionNames,
        string[] optionValues, UIntPtr optionsSize, bool setOnlyNonDebugParams)
        => DllImport.BaseApi5Init(self, datapath, dataSize, language, mode, 
            configs, configSize, optionNames, optionValues, optionsSize, setOnlyNonDebugParams);

    public static string GetDataPath(HandleRef self)
        => DllImport.GetDataPath(self);

    public static void SetImage(HandleRef self, HandleRef pixHandle)
        => DllImport.SetImage(self, pixHandle);

    public static string GetUTF8Text(HandleRef self)
        => DllImport.GetUTF8Text(self);
    public static IntPtr GetUTF8Text_Ptr(HandleRef self)
        => DllImport.GetUTF8Text_Ptr(self);

    public static IntPtr GetHOCRText_Ptr(HandleRef self, int pageNumber)
        => DllImport.GetHOCRText_Ptr(self, pageNumber);

    public static IntPtr GetAltoText_Ptr(HandleRef self, int pageNumber)
        => DllImport.GetAltoText_Ptr(self, pageNumber);
    
    public static IntPtr GetPageText_Ptr(HandleRef self, int pageNumber)
        => DllImport.GetPageText_Ptr(self, pageNumber);

    public static IntPtr GetTsvText_Ptr(HandleRef self, int pageNumber)
        => DllImport.GetTsvText_Ptr(self, pageNumber);
    
    public static IntPtr GetBoxText_Ptr(HandleRef self, int pageNumber)
        => DllImport.GetBoxText_Ptr(self, pageNumber);
    
    public static IntPtr GetWordStrBoxText_Ptr(HandleRef self, int pageNumber)
        => DllImport.GetWordStrBoxText_Ptr(self, pageNumber);
    
    public static IntPtr GetUNLVText_Ptr(HandleRef self, int pageNumber)
        => DllImport.GetUNLVText_Ptr(self, pageNumber);
    
    public static IntPtr GetUnichar_Ptr(HandleRef self, int pageNumber)
        => DllImport.GetUnichar_Ptr(self, pageNumber);

    public static /*PageIterator*/ IntPtr AnalyseLayoutToPageIterator(HandleRef self)
        => DllImport.AnalyseLayoutToPageIterator(self);

    public static /*ResultIterator*/ IntPtr GetResultIterator(HandleRef self)
        => DllImport.GetResultIterator(self);

    public static int[] GetConfidences(HandleRef self)
        => DllImport.GetConfidences(self);

    public static void Clear(HandleRef self)
        => DllImport.Clear(self);

    public static void End(HandleRef self)
        => DllImport.End(self);

    public static int GetSourceResolution(HandleRef self)
        => DllImport.GetSourceResolution(self);

    public static void SetOutputName(HandleRef self, string name)
        => DllImport.SetOutputName(self, name);

    public static void SetInputName(HandleRef self, string name)
        => DllImport.SetInputName(self, name);

    public static string GetInputName(HandleRef self)
        => DllImport.GetInputName(self);

    public static void SetInputImage(HandleRef self, HandleRef pixHandle)
        =>  DllImport.SetInputImage(self, pixHandle);

    public static /*Pix*/ IntPtr GetInputImage(HandleRef self)
        => DllImport.GetInputImage(self);

    public static void SetPageSegmentationMode(HandleRef self, /*PageSegmentationMode*/ int mode)
        => DllImport.SetPageSegmentationMode(self, mode);

    public static int Recognize(HandleRef self, HandleRef monitorHandle)
        => DllImport.Recognize(self, monitorHandle);

    public static /*Pix*/ IntPtr GetThresholdedImage(HandleRef self)
        => DllImport.GetThresholdedImage(self);

    public static /*String*/ IntPtr GetVersion()
        => DllImport.GetVersion();

    public static int SetDebugVariable(HandleRef self, string name, string value)
        => DllImport.SetDebugVariable(self, name, value);

    public static int SetVariable(HandleRef self, string name, string value)
        => DllImport.SetVariable(self, name, value);

    public static int GetIntVariable(HandleRef self, string name, out int value)
        => DllImport.GetIntVariable(self, name, out value);

    public static int GetDoubleVariable(HandleRef self, string name, out double value)
        => DllImport.GetDoubleVariable(self, name, out value);

    public static int GetBoolVariable(HandleRef self, string name, out int value)
        => DllImport.GetBoolVariable(self, name, out value);

    public static string GetStringVariable(HandleRef self, string name)
        => DllImport.GetStringVariable(self, name);

    public static int PrintVariablesToFile(HandleRef self, string fileName)
        => DllImport.PrintVariablesToFile(self, fileName);

    public static int GetMeanConfidence(HandleRef self)
        => DllImport.GetMeanConfidence(self);





}
