namespace TesseractOcrMAUILib.Tessdata;
internal class TrainedDataCollection : ITrainedDataCollection
{
    private HashSet<string> Files { get; set; } = new();

    /// <summary>
    /// AddFile Traineddata file 
    /// </summary>
    /// <param name="fileName"></param>
    /// <exception cref="ArgumentNullException">If filename is null or empty.</exception>
    /// <exception cref="ArgumentException">If filename does not exist in app packages. (See folder "Resources\Raw")</exception>
    public void AddFile(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentNullException(nameof(fileName));
        }
        Task<bool> exist = FileSystem.Current.AppPackageFileExistsAsync(fileName);
        exist.Wait();
        if (exist.Result is false)
        {
            throw new ArgumentException($"'{fileName}' does not exist in app packages. " +
                $"Add matching traineddata file to your app's 'Resources/Raw' folder.");
        }
        Files.Add(fileName);
    }

    /// <summary>
    /// Get array of traineddata file names
    /// </summary>
    /// <returns>String array of file names.</returns>
    public string[] GetTrainedDataFileNames() => Files.ToArray();
}
