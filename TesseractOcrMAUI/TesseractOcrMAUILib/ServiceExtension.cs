#if ANDROID
using Android.Database;
using Java.Lang;
#endif
using TesseractOcrMAUILib.Tessdata;

namespace TesseractOcrMAUILib;
public static class ServiceExtensions
{
    public static IServiceCollection AddTesseractOcr(this IServiceCollection services)
    {
#if ANDROID
        // Load libraries
        JavaSystem.LoadLibrary("png");
        JavaSystem.LoadLibrary("leptonica");
        JavaSystem.LoadLibrary("tesseract");
#endif
        services.AddSingleton<ITessDataHandler, TessDataHandler>();
        
        return services;
    }

}
