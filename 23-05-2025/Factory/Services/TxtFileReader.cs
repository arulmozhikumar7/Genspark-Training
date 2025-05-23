using FileReaderApp.Interfaces;
using System.IO;

namespace FileReaderApp.Services
{
    public sealed class TxtFileReader : IFileReader
    {
        private static readonly TxtFileReader instance = new TxtFileReader();
        private StreamReader? reader;

        // Private constructor for singleton
        private TxtFileReader() { }

        // Singleton Instance property
        public static TxtFileReader Instance => instance;

        public string ReadAll(string filePath)
        {
            if (reader == null)
                reader = new StreamReader(filePath);

            return reader.ReadToEnd();
        }

        public void Close()
        {
            reader?.Close();
            reader = null;
        }
    }
}
