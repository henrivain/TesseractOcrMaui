using Microsoft.Extensions.Logging;
using TesseractOcrMAUILib;

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


        var n = FileSystem.AppPackageFileExistsAsync("fin.traineddata");
        n.Wait();
        var v = n.Result;

#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddTesseractOcr();

        return builder.Build();
    }
}
