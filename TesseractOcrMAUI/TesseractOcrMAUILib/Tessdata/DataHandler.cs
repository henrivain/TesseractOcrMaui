namespace TesseractOcrMAUILib.Tessdata;
internal class TessDataHandler : ITessDataHandler
{
    public TessDataHandler()
    {
        DataPath = string.Empty;
    }

    public string DataPath { get; private set; }

    public string FileExtension => throw new NotImplementedException();

    public void CopyData(string directory)
    {
        throw new NotImplementedException();
    }

    public string[] GetDataLanguages()
    {
        throw new NotImplementedException();
    }

    public string GetDirectory()
    {
        throw new NotImplementedException();
    }
}
