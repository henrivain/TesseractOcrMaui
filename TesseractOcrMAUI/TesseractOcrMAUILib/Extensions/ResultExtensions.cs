using TesseractOcrMAUILib.Results;

namespace TesseractOcrMAUILib.Extensions;
public static class ResultExtensions
{

    public static bool Success(this RecognizionStatus status) => status is RecognizionStatus.Success;
    public static bool NotSuccess(this RecognizionStatus status) => Success(status) is false;

    public static bool Success(this RecognizionResult result) => result.Status.Success(); 
    public static bool NotSuccess(this RecognizionResult result) => Success(result) is false;

}
