# Ocr Character Whitelist/Blacklist

#### Updated 15.9.2023

Refers to issues [Text whitelist #15](https://github.com/henrivain/TesseractOcrMaui/issues/15) and [Add way to configure TessEngine ITesseract is using #16](https://github.com/henrivain/TesseractOcrMaui/issues/16)

### Intro

This document shows how to limit which characters are recognized from image. Process is very easy, but you cannot access it from ITesseract interface at the moment. All functionality is found on nuget package. In future you probably are able to configure also in ITesseract level.

### Note

You can also apply same procedure to other tesseract engine configuration variables. You can find list of available variables from repository document `TesseractEngineConfigurationsVariables.md`.

## Code example

Because ITesseract interface is not used, `traineddata files must loaded manually` from app packages.

1. Load traineddata files from app packages

2. Create engine

3. Configure engine, "mychars" needs to include characters that you want to be whitlisted

4. Recognize image that is loaded using Pix -class

5. Read output

```csharp
using TesseractOcrMaui;

// Folder where traineddata is loaded
string traineddataFolder = FileSystem.Current.CacheDirectory;

// Load data
var traineddataPath = Path.Combine(traineddataFolder, "eng.traineddata");
if (!File.Exists(traineddataPath))
{
    using Stream traineddata = await FileSystem.OpenAppPackageFileAsync("eng.traineddata");
    using FileStream fileStream = File.Create(traineddataPath);
    traineddata.CopyTo(fileStream);
}

// Create engine
using var engine = new TessEngine("eng", traineddataFolder);

//  configure engine
bool success = engine.SetVariable("tessedit_char_whitelist", "mychars");

// Recognize text
using var image = Pix.LoadFromFile(@"\path\to\file.png");
using var result = engine.ProcessImage(image);
output.Text = result.GetText();
```

Document end
