using System.Runtime.Serialization;

namespace TesseractOcrMaui.Exceptions;
[Serializable]
internal class LeptonicaException : Exception
{
    public LeptonicaException()
    {
    }

    public LeptonicaException(string message) : base(message)
    {
    }

    public LeptonicaException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected LeptonicaException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}