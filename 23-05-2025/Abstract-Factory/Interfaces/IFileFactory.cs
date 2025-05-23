using System.IO;

namespace FileApp.Interfaces
{
    public interface IFileFactory
    {
        IFileReader CreateReader(FileStream stream);
        IFileWriter CreateWriter(FileStream stream);
    }
}