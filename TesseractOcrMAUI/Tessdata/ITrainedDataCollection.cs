namespace TesseractOcrMaui.Tessdata;

/// <summary>
/// Store which traineddata files are used.
/// </summary>
public interface ITrainedDataCollection
{
    /// <summary>
    /// AddFile Traineddata file. 
    /// Only filename with extension. 
    /// File should be included in your app's 'Resources\Raw folder' folder.
    /// </summary>
    /// <param name="fileName">
    /// Name of file with extension (no path) that should be loaded from app packages. 
    /// App's 'Resources\Raw folder' should include file with same name.
    /// You can get traineddata files from github: https://github.com/tesseract-ocr/tessdata
    /// </param>
    /// <exception cref="ArgumentException">[DEFAULT_IMPL] if filename invalid (not found from app packages).</exception>
    void AddFile(string fileName);

    /// <summary>
    /// Get filenames of trained data files.
    /// </summary>
    /// <returns>Array of string file names.</returns>
    string[] GetTrainedDataFileNames();
}