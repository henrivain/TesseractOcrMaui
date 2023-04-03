namespace TesseractOcrMAUILib.Tessdata;
internal class TessDataProvider : ITessDataProvider
{
    public TessDataProvider() : this(NullLogger<TessDataProvider>.Instance) { }
    public TessDataProvider(ILogger<TessDataProvider> logger)
    {
        TessDataFolder = Path.Combine(FileSystem.CacheDirectory, "tessdata");
        Logger = logger;
    }

    public string FileExtension { get; } = "traineddata";
    public required string TessDataFolder { get; init; }
    public string[] AvailableLanguages { get; private set; } = Array.Empty<string>();
    public async Task<DataLoadResult> LoadFromPackagesAsync(string[] files, bool copyAlways = false)
    {
        Logger.LogInformation("Try copy '{count}' app package files to '{dir}'.", files.Length, TessDataFolder);

        if (files.Length <= 0)
        {
            Logger.LogError("Cannot provide any traineddata files, none were provided.");
            return new DataLoadResult
            {
                State = TessDataState.NoneValid,
                InvalidFiles = Array.Empty<string>(),
                Message = "No app package files provided.",
                Errors = new string[] { $"{nameof(files)} array was empty." }
            };
        }

        TessDataState state = TessDataState.NoneValid;
        HashSet<string> errors = new();
        HashSet<string> invalidFiles = new();
        HashSet<string> validFiles = new();
        foreach (var file in files)
        {
            var (success, msg) = await TryCopyFile(file, copyAlways);
            if (success)
            {
                state = TessDataState.AtLeastOneValid;
                validFiles.Add(file);
                continue;
            }
            errors.Add(msg);
            invalidFiles.Add(file);
        }

        AvailableLanguages = validFiles.ToArray();
        if (invalidFiles.Count <= 0) 
        {
            if (state is TessDataState.NoneValid)
            {
                Logger.LogError("All given traineddata files were invalid.");
            }
            else
            {
                Logger.LogWarning("Could not load all traineddata files.");
            }
            return new DataLoadResult
            {
                State = state,
                InvalidFiles = invalidFiles.ToArray(),
                Message = "Could not load all traineddata files.",
                Errors = errors.ToArray()
            };
        }

        Logger.LogInformation("Loaded '{count}' traineddata files.", validFiles.Count);
        return new DataLoadResult
        {
            State = TessDataState.AllValid,
            Message = $"Loaded successfully Tesseract languages: '{string.Join(", ", AvailableLanguages)}'."
        };
    }

    ILogger<TessDataProvider> Logger { get; }

    private async Task<(bool Success, string Msg)> TryCopyFile(string file, bool overwritesFiles)
    {
        if (string.IsNullOrWhiteSpace(file))
        {
            Logger.LogWarning("Cannot copy tessdata file with empty name.");
            return (false, "Empty file name.");
        }
        if (Path.GetExtension(file) != FileExtension)
        {
            Logger.LogWarning("Could not load '{file}', must have '{extension}' extension.", file, FileExtension);
            return (false, $"Invalid file extension, must be {FileExtension}");
        }
        if (await FileSystem.AppPackageFileExistsAsync(file) is false)
        {
            Logger.LogWarning("Cannot copy package file '{file}', it doesn't exist.", file);
            return (false, "Package not found.");
        }
        string destination = Path.Combine(TessDataFolder, file);
        if (File.Exists(destination) && overwritesFiles is false)
        {
            Logger.LogInformation("File '{file}' already exist and '{arg}' is set to false. " +
                "File will not be copied.", destination, nameof(overwritesFiles));
            return (true, "Already exist.");
        }
        DeleteOldEntries(destination);
        return await TryCopyStream(file, destination);
    }
    private async Task<(bool Success, string Msg)> TryCopyStream(string packageFileName, string destination)
    {
        try
        {
            using Stream stream = await FileSystem.OpenAppPackageFileAsync(packageFileName);
            Logger.LogInformation("Copy '{file}' to '{dest}'.", packageFileName, destination);
            using FileStream fileStream = File.Create(destination);
            stream.CopyTo(fileStream);
            return (true, "Copy successful.");
        }
        catch (Exception ex)
        {
            Logger.LogWarning("Cannot copy app package file to tessdata, '{ex}': '{msg}'", 
                ex.GetType().Name, ex.Message);
            return (false, ex.Message);
        }
    }
    private bool DeleteOldEntries(in string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            Logger.LogWarning("Cannot delete files from empty path.");
            return false;
        }
        try
        {
            Directory.CreateDirectory(TessDataFolder);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Logger.LogInformation("Deleted file '{path}' for traineddata file.", filePath);
            }
            if (Directory.Exists(filePath))
            {
                Directory.Delete(filePath, true);
                Logger.LogInformation("Deleted directory '{path}' for traineddata file.", filePath);
            }
            return true;
        }
        catch (Exception ex)
        {
            Logger.LogWarning("Cannot clear old corresponding files from '{path}', " +
                "because of exception '{ex}': '{msg}'", filePath, ex.GetType().Name, ex.Message);
            return false;
        }
    }
}
