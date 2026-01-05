using TesseractOcrMaui.Tessdata;

namespace TesseractOcrMaui;



/// <summary>
/// Interface for Tesseract implementations that use and want to expose <see cref="ITessDataProvider"/>.
/// </summary>
public interface ITessdataProviderExposingTesseract
{
    /// <summary>
    /// get used <see cref="ITessDataProvider"/> instance if current 
    /// <see cref="ITesseract"/> implementation is using one.
    /// </summary>
    /// <returns>Used <see cref="ITessDataProvider"/> instance.</returns>
    ITessDataProvider GetTessdataProvideInstance();
}

/// <summary>
/// Interface that allows swapping <see cref="ITessDataProvider"/> at runtime.
/// </summary>
public interface ITessDataProviderSwappable : ITessdataProviderExposingTesseract
{
    /// <summary>
    /// Change the <see cref="ITessDataProvider"/> used by the current 
    /// <see cref="ITesseract"/> implementation during runtime. Calling this does not 
    /// automatically load the data, so that must be done separately.
    /// </summary>
    /// <param name="provider"></param>
    /// <returns>True if success, otherwise false.</returns>
    bool SwapTessdataProvider(ITessDataProvider provider);
}

