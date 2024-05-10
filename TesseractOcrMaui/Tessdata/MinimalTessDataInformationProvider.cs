using System.Text;

namespace TesseractOcrMaui.Tessdata;

/// <summary>
/// Minimal implementation of <see cref="ITessDataInformationProvider"/>
/// </summary>
public class MinimalTessDataInformationProvider : ITessDataInformationProvider
{

    readonly string _languages;

    /// <summary>
    /// Minimal implementation of <see cref="ITessDataInformationProvider"/>
    /// </summary>
    /// <param name="tessDataFolderPath"></param>
    /// <param name="filePaths"></param>
    /// <exception cref="ArgumentException">If any parameter is null or all <paramref name="filePaths"/> values are invalid.</exception>
    public MinimalTessDataInformationProvider(string tessDataFolderPath, params string[] filePaths)
    {
        ArgumentNullException.ThrowIfNull(tessDataFolderPath);
        ArgumentNullException.ThrowIfNull(filePaths);

        StringBuilder builder = new();
        foreach (string path in filePaths)
        {
            string? name = Path.GetFileNameWithoutExtension(path);
            if (name is not null)
            {
                builder.Append(name);
                builder.Append('+');
            }
        }
        if (builder.Length is 0)
        {
            throw new ArgumentException($"No valid values in {filePaths}");
        }
        _languages = builder.ToString();
        TessDataFolder = tessDataFolderPath;
    }



    /// <inheritdoc/>
    public string TessDataFolder { get; }

    /// <inheritdoc/>
    public string GetLanguagesString() => _languages;
}
