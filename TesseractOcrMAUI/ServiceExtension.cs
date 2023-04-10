#if ANDROID
using Java.Lang;
#endif
using MauiTesseractOcr.Tessdata;

namespace MauiTesseractOcr;

/// <summary>
/// Extensions to inject all library dependencies and load libraries.
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Add all dependencies of MauiTesseractOcr to DI container and load libraries.
    /// If you want to configure traineddata languages and more, use other extension in this group => give more params.
    /// With this extension you must have eng.traineddata in you app's 'Resources\Raw' folder.
    /// </summary>
    /// <param name="services">App DI container.</param>
    /// <returns>Same App DI container as in params with services added.</returns>
    public static IServiceCollection AddTesseractOcr(this IServiceCollection services)
    {
        return services.AddTesseractOcr(null);
    }

    /// <summary>
    /// Add all dependencies of MauiTesseractOcr to DI container and load libraries.
    /// You can configure tesseract by giving parameters.
    /// </summary>
    /// <param name="services">App DI container.</param>
    /// <param name="tessDataCollection">
    /// Choose which traineddata files are used. 
    /// If null, eng.traineddata is required to be found in 'Resources\Raw'.
    /// </param>
    /// <returns>Same App DI container as in params with services added.</returns>
    public static IServiceCollection AddTesseractOcr(
        this IServiceCollection services,
        Action<ITrainedDataCollection>? tessDataCollection)
    {
        return services.AddTesseractOcr(tessDataCollection, null);
    }

    /// <summary>
    /// Add all dependencies of MauiTesseractOcr to DI container and load libraries.
    /// You can configure tesseract by giving parameters.
    /// </summary>
    /// <param name="services">App DI container.</param>
    /// <param name="tessDataCollection">
    /// Choose which traineddata files are used. 
    /// If null, eng.traineddata is required to be found in 'Resources\Raw'.
    /// </param>
    /// <param name="providerConfiguration">
    /// Configure how traineddata files are provided. 
    /// If null, default settings are used.
    /// </param>
    /// <returns>Same App DI container as in params with services added.</returns>
    public static IServiceCollection AddTesseractOcr(
        this IServiceCollection services,
        Action<ITrainedDataCollection>? tessDataCollection,
        Action<ITessDataProviderConfiguration>? providerConfiguration)
    {
#if ANDROID
        // Load libraries
        JavaSystem.LoadLibrary("png");
        JavaSystem.LoadLibrary("jpeg");
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
