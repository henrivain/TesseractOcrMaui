namespace TesseractOcrMAUILib.Tessdata;
public readonly struct DataLoadResult
{
    public required TessDataState State { get; init; }
    public string[]? InvalidFiles { get; init; }
}
