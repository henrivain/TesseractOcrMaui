namespace TesseractOcrMaui.Results;

/// <summary>
/// Datatype to store text and its layout.
/// </summary>
/// <param name="Span">Recognized text span, with some additional information.</param>
/// <param name="Layout">Recongized text span layout in image.</param>
/// <param name="Level">Used text block size.</param>
public readonly record struct RecognitionSpan(TextSpan Span, SpanLayout Layout, PageIteratorLevel Level)
{
    /// <summary>
    /// Method enabling deconstruction
    /// <code>
    /// var (span, layout, level) = new RecognitionSpan();
    /// </code>
    /// </summary>
    /// <param name="span"></param>
    /// <param name="layout"></param>
    /// <param name="level"></param>
    public void Deconstruct(out TextSpan span, out SpanLayout layout, out PageIteratorLevel level)
    {
        span = Span;
        layout = Layout;
        level = Level;
    }

    /// <summary>
    /// Method enabling deconstruction
    /// <code>
    /// var (span, layout) = new RecognitionSpan();
    /// </code>
    /// </summary>
    /// <param name="span"></param>
    /// <param name="layout"></param>
    public void Deconstruct(out TextSpan span, out SpanLayout layout)
    {
        span = Span;
        layout = Layout;
    }

}
