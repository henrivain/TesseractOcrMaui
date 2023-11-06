# TesseractOcrMaui

[Tesseract](https://github.com/tesseract-ocr/tesseract) wrapper for windows and Android for .NET MAUI.

## What is this?

I didn't find any good up to date C# wrapper to Tesseract that would function with Maui on Android devices. This library wrapps native Tesseract C/C++ libraries to usable C# interfaces. Currently only Android and Windows are supported, because I don't have resources to test on MacOs or IOS. My own projects only support Windows and Android, so I didn't need those other platforms. If you need support for Apple devices, you have to build those libraries yourself and add them to project.

## Supported platforms

Currently supports Windows and Android. Library is meant to be used with .NET MAUI project. You can see supported cpu architechtures down below.

| platform | Architechture |
| -------- | ------------- |
| Windows  | x86_64        |
| Android  | Arm64-v8a     |
| Android  | Arm-v7a       |
| Android  | x86_64        |
| Android  | x86           |

Supported runtimes

> net7.0 or newer  
> net7.0-windows10.0.19041 or newer  
> net7.0-android or newer

Only png and jpeg libraries are compiled into tesseract native libraries, so only these image types are supported. Additional image libraries are added if needed later.

## Getting started

### 1. Add nuget package to your project.

Package is available in `nuget.org`. You can use it in your project by adding it in your visual studio `nuget package manager or by running command

```powershell
dotnet add package TesseractOcrMaui
```

### 2. Add package to dependency injection (see TesseractOcrMauiTestApp)

Add `AddTesseractOcr` call in you injections. This loads all libraries that are needed to run library and adds needed injectable interfaces. `files.AddFile("eng.traineddata");` adds all your traineddata files to be loaded, when tesseract is used. For example I add `eng.traineddata`, so I must add [traineddata file](https://github.com/tesseract-ocr/tessdata/) with same name to my project's TesseractOcrMauiTestApp\Resources\Raw folder.

MauiProgram.cs

```csharp
using Microsoft.Extensions.Logging;
using TesseractOcrMaui;  // include library namespace

namespace TesseractOcrMauiTestApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        // Inject logging, (optional, but might give useful information)
        builder.Services.AddLogging();

        // Inject library functionality
        builder.Services.AddTesseractOcr(
            files =>
            {
                // must have matching files in Resources/Raw folder
                files.AddFile("eng.traineddata");
            });

        // Inject main page
        builder.Services.AddSingleton<MainPage>();

        return builder.Build();
    }
}
```

### 3. Inject ITesseract into your page

Now you can constructor inject ITesseract interface to you page. I have two labels ("confidenceLabel" and "resultLabel") in my main page. I added button with clicked event handler. I you can see my `Button_Clicked` functionality down below.

Mainpage.xaml.cs

```csharp
using TesseractOcrMaui; // Include library namespace
using TesseractOcrMaui.Extensions;  // Help for handling result object from recognizion

namespace TesseractOcrMauiTestApp;

public partial class MainPage : ContentPage
{
    // inject ITesseract
    public MainPage(ITesseract tesseract)
    {
        InitializeComponent();
        Tesseract = tesseract;
    }

    ITesseract Tesseract { get; }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        // Make user pick file
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
        // null if user cancelled the operation
        if (pickResult is null)
        {
            return;
        }

        // Recognize image
        var result = await Tesseract.RecognizeTextAsync(pickResult.FullPath);

        // Show output
        confidenceLabel.Text = $"Confidence: {result.Confidence}";
        if (result.NotSuccess())
        {
            resultLabel.Text = $"Recognizion failed: {result.Status}";
            return;
        }
        resultLabel.Text = result.RecognisedText;
    }
}
```

## ITesseract API

You can find following methods in the main high level API.

```csharp
public interface ITesseract
{
    RecognizionResult RecognizeText(string imagePath);
    RecognizionResult RecognizeText(byte[] imageBytes);
    RecognizionResult RecognizeText(Pix image);

    Task<RecognizionResult> RecognizeTextAsync(string imagePath);
    Task<RecognizionResult> RecognizeTextAsync(byte[] imageBytes);
    Task<RecognizionResult> RecognizeTextAsync(Pix pix);

    // Loads traineddata files for use from app packages to appdata folder
    Task<DataLoadResult> LoadTraineddataAsync();


    // Gets tessdata folder path from TessDataProvider (from configuration)
    string TessDataFolder { get; }
   
    // Access used TessEngine for configuration (E.g. Whitelist chafacters)
    Action<ITessEngineConfigurable>? EngineConfiguration { set; }
}
```

## Licence

```
Copyright 2023 Henri Vainio

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
```

NOTE: Tesseract depends on other packages that may be licensed under different open source licenses.

This project does not depend on any third-party C# packages, but it needs [traineddata files](https://github.com/tesseract-ocr/tessdata/) to function. Parts of the code are also is reused from [Charlesw Windows Tesseract wrapper](https://github.com/charlesw/tesseract).

## Contributing, IOS and MacOS

If you are interested developing this project, please, open conversation in issues and describe your changes you want to make. Package doesn't currently support IOS or MacOS so any help for them is well appreciated.

## Documentation 
- To see examples of use, see example project `Mainpage` [TesseractOcrMauiTestApp.Mainpage.xaml.cs](https://github.com/henrivain/TesseractOcrMaui/blob/master/TesseractOcrMauiTestApp/MainPage.xaml.cs)
- Some instructions can be found from repository [`Documentation` -folder](https://github.com/henrivain/TesseractOcrMaui/tree/master/Documentation) 
- Classes and methods have `xml comments` that try to explain code functionality.

## Support

If you have any questions about anything related to this project, open issue or send me an email.

Only Android and Windows are supported currently, because I have no recources to build and test for IOS and MacOS. Integrating these platforms should not be very big problem if someone needs them, but I don't need them. If you want to add them, just contact me.

Henri Vainio  
matikkaeditorinkaantaja(at)gmail.com
