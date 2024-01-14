namespace TesseractOcrMaui.Imaging;

/// <summary>
/// Text angle in image.
/// </summary>
public class Scew
{
    /// <summary>
    /// Represents text angle in image.
    /// </summary>
    /// <param name="angle">Text angle.</param>
    /// <param name="confidence">Confidence that angle is correct.</param>
    public Scew(float angle, float confidence)
    {
        Angle = angle;
        Confidence = confidence;
    }

    /// <summary>
    /// Right angle of image.
    /// </summary>
    public float Angle { get; }

    /// <summary>
    /// Tesseract confidence that angle is right.
    /// </summary>
    public float Confidence { get; }


    /// <inheritdoc/>
    public override string ToString() => $"Scew: {Angle} [conf: {Confidence}]";

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Scew scew && Equals(scew);
    }

    /// <inheritdoc/>
    public bool Equals(Scew other)
    {
        return Confidence == other.Confidence && Angle == other.Angle;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int hashCode = 0;
        unchecked
        {
            hashCode += 1000000007 * Angle.GetHashCode();
            hashCode += 1000000009 * Confidence.GetHashCode();
        }
        return hashCode;
    }

    /// <inheritdoc/>
    public static bool operator ==(Scew lhs, Scew rhs) => lhs.Equals(rhs);
    /// <inheritdoc/>
    public static bool operator !=(Scew lhs, Scew rhs) => !(lhs == rhs);
}
