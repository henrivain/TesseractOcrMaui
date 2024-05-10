#nullable enable

namespace TesseractOcrMauiTestApp;
internal static class ImageSelecter
{
    /* This class is a example how to get user selected path */

    internal static async Task<string?> LetUserSelect()
    {
#if IOS
        var pickResult = await MediaPicker.PickPhotoAsync(new MediaPickerOptions()
        {
            Title = "Pick jpeg or png image"
        });
#else
        var pickResult = await FilePicker.PickAsync(new PickOptions()
        {
            PickerTitle = "Pick jpeg or png image",
            // Currently usable image types
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>()
            {
                [DevicePlatform.Android] = new List<string>() { "image/png", "image/jpeg" },
                [DevicePlatform.WinUI] = new List<string>() { ".png", ".jpg", ".jpeg" },
            })
        });
#endif
        return pickResult?.FullPath;
    }
}


