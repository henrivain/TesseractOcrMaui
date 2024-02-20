using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesseractOcrMaui.Exceptions;

/// <summary>
/// Exception thrown when multiple images are given to TessEngine at the same time.
/// Old TessPage must be disposed before trying to process new image.
/// </summary>
public class PageNotDisposedException : TesseractException
{
    /// <summary>
    /// New exception thrown when multiple images are given to TessEngine at the same time.
    /// Old TessPage must be disposed before trying to process new image.
    /// </summary>
    public PageNotDisposedException() { }

    /// <summary>
    /// New exception with message thrown when multiple images are given to TessEngine at the same time.
    /// Old TessPage must be disposed before trying to process new image.
    /// </summary>
    /// <param name="message">Error reason</param>
    public PageNotDisposedException(string message) : base(message) { }

    /// <summary>
    /// New exception with message and inner exception thrown when multiple images are given to TessEngine at the same time.
    /// Old TessPage must be disposed before trying to process new image.
    /// </summary>
    /// <param name="message">Error reason</param>
    /// <param name="innerException">Exception that caused this exception</param>
    public PageNotDisposedException(string message, Exception innerException) 
        : base(message, innerException) { }
}
