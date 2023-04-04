using TesseractOcrMAUILib.Results;

namespace TesseractOcrMAUILib;
public interface ITesseract
{
    RecognizionResult RecognizeText(string imagePath);
    Task<RecognizionResult> RecognizeTextAsync(string imagePath);
    string TessDataFolder { get; }
}
