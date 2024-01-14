namespace TesseractOcrMaui;

/// <summary>
/// Interface that give access to certain TessEngine properties
/// </summary>
public interface ITessEngineConfigurable
{
    /// <summary>
    /// Set PageSegmentationMode that is used if no other mode 
    /// is passed in method parameters.
    /// </summary>
    PageSegmentationMode DefaultSegmentationMode { set; }

    /// <summary>
    /// Set tesseract engine variable by its name and string value.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns>True if success, otherwise false.</returns>
    bool SetVariable(string name, string value);
}
