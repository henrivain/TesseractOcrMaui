namespace TesseractOcrMaui.Tessdata;

/// <summary>
/// New Configuration for TessDataProvider. Default implementation for ITessDataProviderConfiguration.
/// </summary>
public class TessDataProviderConfiguration : ITessDataProviderConfiguration
{
    private string _tessDataFolder;
    private bool _overwritesOldFiles;

    /// <summary>
    /// New Configuration for TessDataProvider.
    /// TessDataFolder = Microsoft.Maui.Storage.FileSystem.CacheDirectory/tessdata.
    /// OverwritesOldFiles = false.
    /// </summary>
    public TessDataProviderConfiguration()
    {
        _tessDataFolder = Path.Combine(FileSystem.Current.CacheDirectory, "tessdata");
        _overwritesOldFiles = false;
    }

    /// <summary>
    /// Boolean value representing if traineddata files should be recopied always.
    /// </summary>
    public bool OverwritesOldFiles
    {
        get => _overwritesOldFiles; 
        set
        {
            _overwritesOldFiles = value;
        }
    }


    /// <summary>
    /// Path to folder containing traineddata files.
    /// By default uses "Microsoft.Maui.Storage.FileSystem.CacheDirectory/tessdata".
    /// </summary>
    /// <exception cref="ArgumentNullException">If value is null or empty.</exception>
    public string TessDataFolder
    {
        get => _tessDataFolder; 
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            _tessDataFolder = value;
        }
    }
}
