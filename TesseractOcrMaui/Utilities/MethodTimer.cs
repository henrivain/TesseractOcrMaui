using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TesseractOcrMaui.Utilities;
internal static class MethodTimer
{
    internal static void Time(Action action, ILogger? logger, [CallerArgumentExpression(nameof(action))] string? methodName = null)
    {
        Stopwatch sw = Stopwatch.StartNew();
        action();
        sw.PrintTime(methodName, logger);
    }
    
    internal static void Time<TParam>(Action<TParam> action, TParam param, ILogger? logger, [CallerArgumentExpression(nameof(action))] string? methodName = null)
    {
        Stopwatch sw = Stopwatch.StartNew();
        action(param);
        sw.PrintTime(methodName, logger);
    }
    
    internal static void Time<TParam1, TParam2>(Action<TParam1, TParam2> action, TParam1 param1,
        TParam2 param2, ILogger? logger, [CallerArgumentExpression(nameof(action))] string? methodName = null)
    {
        Stopwatch sw = Stopwatch.StartNew();
        action(param1, param2);
        sw.PrintTime(methodName, logger);
    }

    internal static TResult Time<TResult>(Func<TResult> func,  ILogger? logger, [CallerArgumentExpression(nameof(func))] string? methodName = null)
    {
        Stopwatch sw = Stopwatch.StartNew();
        TResult result = func();
        sw.PrintTime(methodName, logger);
        return result;
    }
    
    internal static TResult Time<TResult, TParam>(Func<TParam, TResult> func, TParam param,  ILogger? logger, [CallerArgumentExpression(nameof(func))] string? methodName = null)
    {
        Stopwatch sw = Stopwatch.StartNew();
        TResult result = func(param);
        sw.PrintTime(methodName, logger);
        return result;
    }
    
    internal static TResult Time<TResult, TParam1, TParam2>(
        Func<TParam1, TParam2, TResult> func, TParam1 param1, TParam2 param2,
        ILogger? logger, [CallerArgumentExpression(nameof(func))] string? methodName = null)
    {
        Stopwatch sw = Stopwatch.StartNew();
        TResult result = func(param1, param2);
        sw.PrintTime(methodName, logger);
        return result;
    }


    private static void PrintTime(this Stopwatch sw, string? methodName, ILogger? logger)
    {
        logger ??= NullLogger.Instance;
        long ms = sw.ElapsedMilliseconds;
        logger.LogInformation("Executing {method} took {ms} ms.", methodName ?? "Name_Not_Found", ms);
    }

}
