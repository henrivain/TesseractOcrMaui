using System.Reflection.Metadata;
using TesseractOcrMAUILib.Converters;
using TesseractOcrMAUILib.ImportApis;

namespace TesseractOcrMAUILib;
internal class TessApi
{
    internal static int BaseApiInit(HandleRef handle, string language, string traineddataPath,
        EngineMode mode, IDictionary<string, object> initialOptions)
    {
        if (handle.Handle == IntPtr.Zero)
        {
            throw new InvalidOperationException($"{nameof(Handle)} must not be zero pointer.");
        }
        if (language is null)
        {
            throw new InvalidOperationException(nameof(language));
        }
        traineddataPath ??= string.Empty;


        List<string> optionVariables = new();
        List<string> optionValues = new();
        foreach (var (variable, values) in initialOptions)
        {
            string? result = TessConverter.TryToString(values);
            if (result is not null && string.IsNullOrWhiteSpace(variable) is false)
            {
                optionVariables.Add(variable);
                optionValues.Add(result!);
            }
        }
        string[] configs = Array.Empty<string>();
        string[] options = optionVariables.ToArray();
        string[] optVals = optionValues.ToArray();

        int initState = TesseractApi.BaseApi5Init(handle, traineddataPath, 0, language,
            (int)mode, configs, configs.Length, options, optVals,
            (nuint)options.Length, false);

        return initState;
    }

    internal static void DeleteHandle(HandleRef handle) => TesseractApi.DeleteApi(handle);
}
