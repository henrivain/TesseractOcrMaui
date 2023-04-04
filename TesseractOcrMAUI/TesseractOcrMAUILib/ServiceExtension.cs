#if ANDROID
using Java.Lang;
#endif
using TesseractOcrMAUILib.Tessdata;

namespace TesseractOcrMAUILib;
public static class ServiceExtensions
{
    public static IServiceCollection AddTesseractOcr(this IServiceCollection services)
    {
        services.AddTesseractOcr(null);
        return services;
    }

    public static IServiceCollection AddTesseractOcr(
        this IServiceCollection services, 
        Action<ITrainedDataCollection>? tessDataCollection)
    {
        services.AddTesseractOcr(tessDataCollection, null);
        return services;
    }


    public static IServiceCollection AddTesseractOcr(
        this IServiceCollection services, 
        Action<ITrainedDataCollection>? tessDataCollection, 
        Action<ITessDataProviderConfiguration>? providerConfiguration)
    {
#if ANDROID
        // Load libraries
        JavaSystem.LoadLibrary("png");
        JavaSystem.LoadLibrary("leptonica");
        JavaSystem.LoadLibrary("tesseract");
#endif

        tessDataCollection ??= new((files) =>
        {
            files.AddFile("eng.traineddata");
        });

        TrainedDataCollection trainedDataCollection = new();
        tessDataCollection(trainedDataCollection);

        TessDataProviderConfiguration configuration = new();
        if (providerConfiguration is not null)
        {
            providerConfiguration(configuration);
        }

        services.AddSingleton<ITessDataProviderConfiguration>(configuration);
        services.AddSingleton<ITrainedDataCollection>(trainedDataCollection);
        services.AddSingleton<ITessDataProvider, TessDataProvider>();
        services.AddTransient<ITesseract, Tesseract>();
        return services;
    }
}
