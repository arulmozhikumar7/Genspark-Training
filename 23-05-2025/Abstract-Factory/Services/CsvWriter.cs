using FileApp.Interfaces;
using System.IO;

namespace FileApp.Services
{
    public class CsvWriter : IFileWriter
    {
        private readonly StreamWriter _writer;

        public CsvWriter(FileStream stream)
        {
            _writer = new StreamWriter(stream, leaveOpen: true);
        }

        public void Write(string content)
        {
            _writer.WriteLine(content);
            _writer.Flush();
        }
    }
}