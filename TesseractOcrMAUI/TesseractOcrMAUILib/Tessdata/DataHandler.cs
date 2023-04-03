namespace TesseractOcrMAUILib.Tessdata;
internal class TessDataHandler : ITessDataHandler
{
    public TessDataHandler() : this(NullLogger.Instance) { }
    public TessDataHandler(ILogger logger)
    {
        TessDataFolder = Path.Combine(FileSystem.CacheDirectory, "tessdata");
        Logger = logger;
    }

    public string FileExtension { get; } = "traineddata";

    public required string TessDataFolder { get; init; }
    public ILogger Logger { get; }

    public async Task<DataLoadResult> CopyFromPackageAsync(params string[] files)
    {
        Logger.LogInformation("Try copy '{count}' app package files to '{dir}'.", files.Length, TessDataFolder);

        if (files.Length is 0)
        {
            return new DataLoadResult
            {
                State = TessDataState.NoneValid,
                InvalidFiles = Array.Empty<string>()
            };
        }

        TessDataState state = TessDataState.NoneValid;
        List<string> invalidFiles = new();
        foreach (var file in files)
        {
            if (await TryCopyFile(file))
            {
                state = TessDataState.AtLeastOneValid;
                continue;
            }
            invalidFiles.Add(file);
        }

        return new DataLoadResult
        {
            State = invalidFiles.Count > 0 ? state : TessDataState.AllValid,
            InvalidFiles = invalidFiles.Count > 0 ? invalidFiles.ToArray() : null
        };
    }

    private async Task<bool> TryCopyFile(string file)
    {
        throw new NotImplementedException("Not ready");


        if (await FileSystem.AppPackageFileExistsAsync(file) is false)
        {
            return false;
        }

        string destination = Path.Combine(TessDataFolder, file);
        if (File.Exists(destination))
        {
            File.Delete(destination);
        }

        
        using Stream stream = await FileSystem.OpenAppPackageFileAsync(file);

        using StreamWriter writer = new(stream);

        using FileStream fileStream = File.Create(destination);




        return true;
    }

    public string[] GetAvailableLanguages()
    {
        throw new NotImplementedException();
    }


}
