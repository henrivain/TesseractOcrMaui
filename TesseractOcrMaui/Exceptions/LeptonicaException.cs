namespace TesseractOcrMaui.Exceptions;

/// <summary>
/// Exception that Leptonica might throw when error occurs
/// </summary>
[Serializable]
public class LeptonicaException : Exception
{
    /// <summary>
    /// New Exception that Leptonica might throw when error occurs
    /// </summary>
    public LeptonicaException()
    {
    }

    /// <summary>
    /// New Exception that Leptonica might throw when error occurs
    /// </summary>
    /// <param name="message"></param>
    public LeptonicaException(string message) : base(message)
    {
    }

    /// <summary>
    /// New Exception that Leptonica might throw when error occurs
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public LeptonicaException(string message, Exception innerException) : base(message, innerException)
    {
    }
}