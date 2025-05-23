using FileReaderApp.Interfaces;
using System.IO;

namespace FileReaderApp.Services
{
    public sealed class CsvFileReader : IFileReader
    {
        private static readonly CsvFileReader instance = new CsvFileReader();
        private StreamReader? reader;

        private CsvFileReader() { }

        public static CsvFileReader Instance => instance;

        public string ReadAll(string filePath)
        {
            if (reader == null)
                reader = new StreamReader(filePath);

            var lines = new List<string>();
            while (!reader.EndOfStream)
            {
                lines.Add(reader.ReadLine() ?? "");
            }
            return string.Join(", ", lines);
        }

        public void Close()
        {
            reader?.Close();
            reader = null;
        }
    }
}
