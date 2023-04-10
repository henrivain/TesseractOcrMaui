using Microsoft.Extensions.Logging;
using TesseractOcrMaui;

namespace TesseractOcrMauiTestApp;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddLogging();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddTesseractOcr(
            files =>
            {
                files.AddFile("fin.traineddata");
            });

        builder.Services.AddSingleton<MainPage>();

        return builder.Build();
    }
}
