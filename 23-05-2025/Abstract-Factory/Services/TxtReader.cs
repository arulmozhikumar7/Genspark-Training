using FileApp.Interfaces;
using System.IO;

namespace FileApp.Services
{
    public class TxtReader : IFileReader
    {
        private readonly StreamReader _reader;

        public TxtReader(FileStream stream)
        {
            _reader = new StreamReader(stream, leaveOpen: true);
        }

        public string Read()
        {
            return _reader.ReadToEnd();
        }
    }
}