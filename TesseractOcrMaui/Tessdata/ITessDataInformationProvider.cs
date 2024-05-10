namespace TesseractOcrMaui.Tessdata;

/// <summary>
/// Provide all traineddata file information required by <see cref="TessEngine"/>.
/// </summary>
public interface ITessDataInformationProvider
{
    /// <summary>
    /// Path to tessdata folder.
    /// </summary>
    string TessDataFolder { get; }

    /// <summary>
    /// Get all loaded languages in '+' -separated list with.traineddata extension removed.
    /// </summary>
    /// <returns>'+' -separated string of traineddata file names without extension. Example: 'fin+eng+swe'</returns>
    string GetLanguagesString();
}
