namespace TesseractOcrMaui.Utilities;
internal class FileSystemHelper
{
    static string? _cacheFolder;

    internal static string GetCacheFolder()
    {
        if (_cacheFolder is not null)
        {
            return _cacheFolder;
        }

#if NET7_0_ONLY || NET8_0_ONLY || NET9_0_ONLY
        _cacheFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
#else
        _cacheFolder = FileSystem.CacheDirectory;
#endif

        if (string.IsNullOrEmpty(_cacheFolder))
        {
            throw new InvalidOperationException("Device does not have cache folder or it cannot be retrieved.");
        }
        return _cacheFolder;
    }





}
