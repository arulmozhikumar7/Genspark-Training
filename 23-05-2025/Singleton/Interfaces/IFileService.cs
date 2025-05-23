namespace FileReaderApp.Interfaces
{
    public interface IFileService
    {
        string ReadAll();
        void Close();
    }
}
