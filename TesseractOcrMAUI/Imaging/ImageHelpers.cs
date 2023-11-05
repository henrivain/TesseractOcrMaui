namespace TesseractOcrMaui.Imaging;
internal static class ImageHelpers
{
    internal static bool IsJpeg(in byte[] imageBytes)
    {
        byte first = imageBytes[0];
        byte second = imageBytes[1];
        byte secondToLast = imageBytes[^2];
        byte last = imageBytes[^1];

        return imageBytes.Length > 4
            && first is 0xFF
            && second is 0xD8
            && secondToLast is 0xFF
            && last is 0xD9;
    }
}
