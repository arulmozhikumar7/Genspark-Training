using FileApp.Interfaces;
using FileApp.Services;
using System.IO;

namespace FileApp.Factories
{
    public class TxtFileFactory : IFileFactory
    {
        public IFileReader CreateReader(FileStream stream) => new TxtReader(stream);
        public IFileWriter CreateWriter(FileStream stream) => new TxtWriter(stream);
    }
}