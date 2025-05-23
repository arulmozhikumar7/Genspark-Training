namespace FileReaderApp.Interfaces
{
    public interface IFileReader
    {
        string ReadAll(string filePath);
        void Close();
    }
}
