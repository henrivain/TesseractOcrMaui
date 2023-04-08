# TesseractOcrMaui

[Tesseract](https://github.com/tesseract-ocr/tesseract) wrapper for windows and Android for .NET MAUI.

## What is this?

I didn't find any good up to date C# wrapper to Tesseract that would function with Maui on Android devices. This library wrapps native Tesseract C/C++ libraries to usable C# interfaces. Currently only Android and Windows are supported, because I don't have resources to test on MacOs or IOS. Also my own projects only support Windows and Android, so I didn't need those other platforms. If you need support for Apple devices, you have to build those libraries yourself and add them to project.

## Supported platforms

Currently supports Windows and Android. Library is meant to be used with .NET MAUI project. You can see supported cpu architechtures down below.

| platform | Architechture |
| -------- | ------------- |
| Windows  | x86_64        |
| Android  | Arm64-v8a     |
| Android  | Arm-v7a       |
| Android  | x86_64        |
| Android  | x86           |

## Get started

Package is not currently available in nuget, but will probably be there soon. If you want to use it, contact me, or pack it yourself.

### Using library

### 1. Add nuget package to your project.

### 2. Add package to dependency injection (see TesseractOcrMauiTestApp)

<br/>

Page should be injected or injecting Tesseract won't work. `AddTesseractOcr` also loads all libraries that are needed to run library. `files.AddFile("eng.traineddata");` adds all your traineddata files to be loaded, when tesseract is used. For example I add `eng.traineddata`, so I must add [traineddata file](https://github.com/tesseract-ocr/tessdata/) with same name to my project's TesseractOcrMauiTestApp\Resources\Raw folder.

<br/>

MauiProgram.cs

```csharp
using Microsoft.Extensions.Logging;
using MauiTesseractOcr;  // include library

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
        // Inject logging, (optional, but gives info)
        builder.Services.AddLogging();

        // Inject library functionality
        builder.Services.AddTesseractOcr(
            files =>
            {
                files.AddFile("eng.traineddata");
            });

        // Inject main page
        builder.Services.AddSingleton<MainPage>();

        return builder.Build();
    }
}
```

### 3. Inject ITesseract to your page

<br/>

Now you can constructor inject ITesseract interface to you page. I have two labels ("confidenceLabel" and "resultLabel") in my main page. I added button with clicked event handler. I you can see my `Button_Clicked` functionality down below.

<br/>

Mainpage.xaml.cs

```csharp
using MauiTesseractOcr;
using MauiTesseractOcr.Extensions;

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
            PickerTitle = "Pick png image",
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

<br/>

<br/>

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

<br/>

## Support

If you have any questions about anything related to this project, open issue with `help wanted` tag or send me an email.

<br/>

Henri Vainio  
matikkaeditorinkaantaja(at)gmail.com
