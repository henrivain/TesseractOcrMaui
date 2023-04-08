namespace MauiTesseractOcr.Tessdata;

public interface ITessDataProviderConfiguration
{
    void OverwritesOldFiles(bool overwrite);
    bool GetOverWriteOldEntries();
    void UseTessDataFolder(string path);
    string GetTessDataFolder();
}