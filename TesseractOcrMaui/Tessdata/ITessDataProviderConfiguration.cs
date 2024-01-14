namespace TesseractOcrMaui.Tessdata;

/// <summary>
/// Configure how 
/// </summary>
public interface ITessDataProviderConfiguration
{
    /// <summary>
    /// Boolean value representing if traineddata files should be recopied always.
    /// </summary>
    bool OverwritesOldFiles { get; set; }

    /// <summary>
    /// Path to folder containing traineddata files.
    /// </summary>
    /// <exception cref="ArgumentNullException">[DEFAULT_IMPL, SET] If given value is null or empty.</exception>
    string TessDataFolder { get; set; }
}