namespace TesseractOcrMAUILib.Tessdata;

public interface ITrainedDataCollection
{
    /// <summary>
    /// AddFile Traineddata file. 
    /// Default implementation (TrainedDataCollection) may throw ArgumentException if filename invalid.
    /// </summary>
    /// <param name="fileName"></param>
    void AddFile(string fileName);

    /// <summary>
    /// Get filenames of trained data files.
    /// </summary>
    /// <returns>Array of string file names.</returns>
    string[] GetTrainedDataFileNames();
}