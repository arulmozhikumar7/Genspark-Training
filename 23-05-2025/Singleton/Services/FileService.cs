using FileReaderApp.Interfaces;
using System.IO;

namespace FileReaderApp.Services
{
    public sealed class FileService : IFileService
    {
        private static FileService instance;
        private StreamReader reader;

        private FileService() { }

        public static FileService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FileService();
                }
                return instance;
            }
        }

        public void Open(string path)
        {
            if (reader == null)
                reader = new StreamReader(path);
        }

        public string ReadAll()
        {
            return reader.ReadToEnd();
        }

        public void Close()
        {
            reader?.Close();
            reader = null;
        }
    }
}
