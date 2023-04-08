using MauiTesseractOcr.Results;
using MauiTesseractOcr.Tessdata;

namespace MauiTesseractOcr.Extensions;
public static class ResultExtensions
{
    public static bool Success(this RecognizionStatus status) => status is RecognizionStatus.Success;
    public static bool NotSuccess(this RecognizionStatus status) => Success(status) is false;

    public static bool Success(this RecognizionResult result) => result.Status.Success(); 
    public static bool NotSuccess(this RecognizionResult result) => result.Status.NotSuccess();


    public static bool Success(this TessDataState state) => state is TessDataState.AllValid or TessDataState.AtLeastOneValid;
    public static bool NotSuccess(this TessDataState state) => state.Success() is false;

    public static bool Success(this DataLoadResult result) => result.State.Success();
    public static bool NotSuccess(this DataLoadResult result) => result.State.NotSuccess();

}
