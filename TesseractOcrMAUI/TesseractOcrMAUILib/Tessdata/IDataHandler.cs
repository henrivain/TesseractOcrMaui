namespace TesseractOcrMAUILib.Tessdata;
public interface ITessDataHandler
{
    string FileExtension { get; }
    string TessDataFolder { get; }
    string[] GetAvailableLanguages();
    Task<DataLoadResult> CopyFromPackageAsync(string[] files);
}
