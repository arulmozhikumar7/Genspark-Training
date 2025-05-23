using FileApp.Interfaces;
using FileApp.Services;
using System.IO;

namespace FileApp.Factories
{
    public class CsvFileFactory : IFileFactory
    {
        public IFileReader CreateReader(FileStream stream) => new CsvReader(stream);
        public IFileWriter CreateWriter(FileStream stream) => new CsvWriter(stream);
    }
}