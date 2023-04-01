using System.Runtime.Serialization;

namespace TesseractOcrMAUILib.Exceptions;
[Serializable]
internal class TesseractException : Exception
{
    public TesseractException()
    {
    }

    public TesseractException(string message) : base(message)
    {
    }

    public TesseractException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected TesseractException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}