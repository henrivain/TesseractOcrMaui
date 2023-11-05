namespace TesseractOcrMaui.Exceptions;

/// <summary>
/// Exception thrown when known error throws exception 
/// </summary>
public class KnownIssueException : LeptonicaException
{
    /// <summary>
    /// New exception thrown when known error throws exception
    /// </summary>
    public KnownIssueException() { }

    /// <summary>
    /// New exception thrown when known error throws exception
    /// </summary>
    /// <param name="message"></param>
    public KnownIssueException(string message) : base(message) { }

    /// <summary>
    /// New exception thrown when known error throws exception
    /// </summary>
    /// <param name="message">Error reason.</param>
    /// <param name="innerException">Exception that caused this error.</param>
    public KnownIssueException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Information how to find information about issue
    /// </summary>
    public required string IssueInformation { get; set; }

    /// <summary>
    /// Link to Issue in internet
    /// </summary>
    public string? ReferringLink { get; set; }
}
