using System.Runtime.Serialization;

namespace TesseractOcrMAUILib.Exceptions;
/// <summary>
/// Thrown when text recognizion cannot be done to given image.
/// </summary>
[Serializable]
internal class ImageRecognizionException : TesseractException
{
    public ImageRecognizionException() { }
    
    public ImageRecognizionException(string message) : base(message) { }

    public ImageRecognizionException(string message, Exception innerException) : base(message, innerException) { }

    protected ImageRecognizionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}