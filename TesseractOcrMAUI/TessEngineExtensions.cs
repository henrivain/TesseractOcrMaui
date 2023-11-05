using Microsoft.Maui.Controls;

namespace TesseractOcrMaui;

/// <summary>
/// Extensions to help engine configuration
/// </summary>
public static class TessEngineExtensions
{
    /// <summary>
    /// Configure characters that ocr can use.
    /// </summary>
    /// <param name="engine">Engine to be configured.</param>
    /// <param name="allowedCharacters">Characters that ocr can recognize. Null or empty string means all characters.</param>
    public static void SetCharacterWhitelist(this TessEngine engine, string? allowedCharacters)
    {
        engine.SetVariable("tessedit_char_whitelist", allowedCharacters ?? "");
    }

    /// <summary>
    /// Configure characters that ocr can use.
    /// </summary>
    /// <param name="engine">Engine to be configured.</param>
    /// <param name="blockedLetters">Characters that ocr cannot recognize.</param>
    public static void SetCharacterBlacklist(this TessEngine engine, string? blockedLetters)
    {
        engine.SetVariable("tessedit_char_whitelist", blockedLetters ?? "");
    }

    /// <summary>
    /// Configure engine mode that ocr uses
    /// </summary>
    /// <param name="engine">Engine to be configured.</param>
    /// <param name="engineMode">PageSegmentationMode that defines how ocr tries to find characters.</param>
    public static void SetEngineMode(this TessEngine engine, PageSegmentationMode engineMode)
    {
        engine.DefaultSegmentationMode = engineMode;
    }


}
