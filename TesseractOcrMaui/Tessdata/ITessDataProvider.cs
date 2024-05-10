using TesseractOcrMaui.Results;

namespace TesseractOcrMaui.Tessdata;

/// <summary>
/// Interface for handling traineddata files.
/// </summary>
public interface ITessDataProvider
{
    /// <summary>
    /// Path to tessdata folder.
    /// </summary>
    string TessDataFolder { get; }

    /// <summary>
    /// Get array of available trained data paths.
    /// </summary>
    string[] AvailableLanguages { get; }

    
    
    /// <summary>
    /// Boolean value representing if provider has already loaded tessdata.
    /// </summary>
    bool IsAllDataLoaded { get; }

    /// <summary>
    /// Load required trained data files from app packages and copy them to TessDataFolder.
    /// </summary>
    /// <returns>Task of DataLoadResult representing action status.</returns>
    Task<DataLoadResult> LoadFromPackagesAsync();

    /// <summary>
    /// Get all traineddata file names that are provided when initializing.
    /// </summary>
    /// <returns>Array of filenames with extensions.</returns>
    string[] GetAllFileNames();

    /// <summary>
    /// Get all loaded languages in '+' -separated list with.traineddata extension removed.
    /// Example: 'fin+eng+swe'
    /// </summary>
    /// <returns></returns>
    string GetLanguagesString();
}
