namespace TesseractOcrMaui.ImportApis;
internal static class Definitions
{
    const string _windowsTesseractDllName = @"tesseract55.dll";
    const string _windowsLeptonicaDllName = @"leptonica-1.86.1.dll";


    /*
     * Tesseract API dll name definition based on platform
     */
#if WINDOWS
    public const string TesseractDllName = _windowsTesseractDllName;
#elif ANDROID21_0_OR_GREATER
    public const string TesseractDllName = "libtesseract";
#elif IOS
    public const string TesseractDllName = "This DLL name should never be used, please, file bug report";
#else

#if WINDOWS_OR_WINDOWS_NONMAUI
    public const string TesseractDllName = _windowsTesseractDllName;
#elif LINUX
    public const string TesseractDllName = "Linux is not currently supported, please make a feature request.";
#else
    public const string TesseractDllName = "Use Windows, Android or iOS Platform";
#endif

#endif

    
    /*
     * Leptonica API dll name definition based on platform
     */
#if WINDOWS
    public const string LeptonicaDllName = _windowsLeptonicaDllName;
#elif ANDROID21_0_OR_GREATER
    public const string LeptonicaDllName = "libleptonica";
#elif IOS
    public const string LeptonicaDllName = "This DLL name should never be used, please, file bug report";
#else

#if WINDOWS_OR_WINDOWS_NONMAUI
    public const string LeptonicaDllName = _windowsLeptonicaDllName;
#elif LINUX
    public const string LeptonicaDllName = "Linux is not currently supported, please make a feature request.";
#else
    public const string LeptonicaDllName = "Use Windows, Android or iOS Platform";
#endif

#endif
}
