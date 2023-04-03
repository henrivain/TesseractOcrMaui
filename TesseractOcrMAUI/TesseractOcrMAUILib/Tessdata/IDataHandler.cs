namespace TesseractOcrMAUILib.Tessdata;
public interface ITessDataHandler
{
    string FileExtension { get; }
    string Directory { get; }
    string[] GetDataLanguages();
    Task<bool> CopyDataAsync();
}
