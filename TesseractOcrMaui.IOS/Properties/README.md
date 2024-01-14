# TesseractOcrMaui.IOS

Tesseract bindings for TesseractOcrMaui nuget package.

[TesseractOcrMaui](https://github.com/henrivain/TesseractOcrMaui) is a [Tesseract](https://github.com/tesseract-ocr/tesseract) wrapper for windows, iOS and Android for .NET MAUI.

## Using the package

This package `TesseractOcrMaui.IOS` only includes bindings for iOS and should not be used on its own. To use Tesseract with your .NET MAUI project add [TesseractOcrMaui](https://www.nuget.org/packages/TesseractOcrMaui/) nuget package:

1. With your IDE package manager

2. Package reference

```xml
<PackageReference Include="TesseractOcrMaui" Version="1.1.0" />
```

3. Dotnet CLI

```ps
dotnet add package TesseractOcrMaui --version 1.1.0
```

Note that you should check what the current package version is and use it in your command.
This package is dependency on the main package `TesseractOcrMaui`.

## iOS support

| platform            | Architechture |
| ------------------- | ------------- |
| iOS emulator        | x86_64        |
| iOS emulator        | Arm64         |
| iOS physical device | Arm64         |

## Licence

```licence
Copyright 2024 Henri Vainio

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

NOTE: Tesseract depends on other packages that may be licensed under different open source licenses

## Support

If you have any questions about anything related to this project, open issue or send me an email.

Henri Vainio  
matikkaeditorinkaantaja(at)gmail.com
