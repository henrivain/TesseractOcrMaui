namespace TesseractOcrMAUILib.Tessdata;
public interface ITessDataProvider
{
    string FileExtension { get; }
    string TessDataFolder { get; }
    string[] AvailableLanguages { get; }
    Task<DataLoadResult> LoadFromPackagesAsync(string[] files, bool copyAlways = false);
}
