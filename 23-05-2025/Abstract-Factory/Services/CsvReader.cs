using FileApp.Interfaces;
using System.IO;

namespace FileApp.Services
{
    public class CsvReader : IFileReader
    {
        private readonly StreamReader _reader;

        public CsvReader(FileStream stream)
        {
            _reader = new StreamReader(stream, leaveOpen: true);
        }

        public string Read()
        {
            return _reader.ReadToEnd();
        }
    }
}