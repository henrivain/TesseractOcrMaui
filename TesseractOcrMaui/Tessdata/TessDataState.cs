namespace TesseractOcrMaui.Tessdata;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// State of traineddata files when trying to load them from packages.
/// </summary>
public enum TessDataState
{
    AllValid, AtLeastOneValid, NoneValid
}
