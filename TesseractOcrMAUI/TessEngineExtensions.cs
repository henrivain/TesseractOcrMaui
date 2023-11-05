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
    public static bool SetCharacterWhitelist(this ITessEngineConfigurable engine, string? allowedCharacters)
    {
        return engine.SetVariable("tessedit_char_whitelist", allowedCharacters ?? "");
    }

    /// <summary>
    /// Configure characters that ocr cannot use.
    /// </summary>
    /// <param name="engine">Engine to be configured.</param>
    /// <param name="blacklistedCharacters">Characters that ocr cannot recognize.</param>
    public static bool SetCharacterBlacklist(this ITessEngineConfigurable engine, string? blacklistedCharacters)
    {
        return engine.SetVariable("tessedit_char_blacklist", blacklistedCharacters ?? "");
    }
}
