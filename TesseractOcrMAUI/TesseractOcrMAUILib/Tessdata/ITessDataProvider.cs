using MauiTesseractOcr.Results;

namespace MauiTesseractOcr.Tessdata;
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
    /// Load required trained data files from app packages and copy them to TessDataFolder.
    /// </summary>
    /// <returns>Task of DataLoadResult representing action status.</returns>
    Task<DataLoadResult> LoadFromPackagesAsync();
}
