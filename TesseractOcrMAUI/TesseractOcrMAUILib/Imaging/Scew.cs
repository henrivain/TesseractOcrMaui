namespace MauiTesseractOcr.Imaging;

public class Scew
{
    public Scew(float angle, float confidence)
    {
        Angle = angle;
        Confidence = confidence;
    }

    public float Angle { get; }
    public float Confidence { get; }

    public override string ToString() => $"Scew: {Angle} [conf: {Confidence}]";

    public override bool Equals(object? obj)
    {
        return obj is Scew scew && Equals(scew);
    }

    public bool Equals(Scew other)
    {
        return Confidence == other.Confidence && Angle == other.Angle;
    }

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

    public static bool operator ==(Scew lhs, Scew rhs) => lhs.Equals(rhs);
    public static bool operator !=(Scew lhs, Scew rhs) => !(lhs == rhs);
}
