namespace TesseractOcrMAUILib.Tessdata;
public class TessDataProviderConfiguration : ITessDataProviderConfiguration
{
    public TessDataProviderConfiguration()
    {
        TessDataFolder = Path.Combine(FileSystem.Current.CacheDirectory, "tessdata");
        OverwritesOldEntries = false;
    }

    string TessDataFolder { get; set; }
    bool OverwritesOldEntries { get; set; }

    /// <summary>
    /// Set path to tessdata folder.
    /// </summary>
    /// <param name="path"></param>
    /// <exception cref="ArgumentNullException">If path null or empty.</exception>
    public void UseTessDataFolder(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(nameof(path));
        }
        TessDataFolder = path;
    }

    /// <summary>
    /// Get path to tessdata folder. 
    /// By default uses "Microsoft.Maui.Storage.FileSystem.CacheDirectory/tessdata"
    /// </summary>
    /// <returns>Path to tessdata folder.</returns>
    public string GetTessDataFolder() => TessDataFolder;

    /// <summary>
    /// If true, trained data files are always copied to tessdata folder. 
    /// If false, trained data files are copied only if they don't already exist.
    /// </summary>
    /// <param name="overwrite"></param>
    public void OverwritesOldFiles(bool overwrite = false) => OverwritesOldEntries = overwrite;

    /// <summary>
    /// Get value representing if traineddata files should be copied always.
    /// </summary>
    /// <returns>
    /// True if trained data files should be copied every time.
    /// False if trained data files should be copied only if they don't already exist in tessdata directory.
    /// </returns>
    public bool GetOverWriteOldEntries() => OverwritesOldEntries;
}
