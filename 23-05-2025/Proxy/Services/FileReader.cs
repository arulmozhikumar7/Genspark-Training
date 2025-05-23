using System;
using System.IO;

namespace fileAccessProxy.Services
{
    public sealed class FileReader
    {
        private static readonly Lazy<FileReader> _instance = new Lazy<FileReader>(() => new FileReader());

        private FileReader() { }

        public static FileReader Instance => _instance.Value;

        public string ReadSensitiveContent(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return "[Error] File not found.";

                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                return $"[Error] Could not read file: {ex.Message}";
            }
        }

       public string ReadMetadata(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                return "[Error] File not found.";

            var fileInfo = new FileInfo(filePath);

            return
                $"File Name      : {fileInfo.Name}\n" +
                $"Full Path      : {fileInfo.FullName}\n" +
                $"Size           : {fileInfo.Length} bytes\n" +
                $"Created On     : {fileInfo.CreationTime}\n" +
                $"Last Modified  : {fileInfo.LastWriteTime}\n" +
                $"Last Accessed  : {fileInfo.LastAccessTime}\n" +
                $"Is Read-Only   : {fileInfo.IsReadOnly}\n" +
                $"Extension      : {fileInfo.Extension}";
        }
        catch (Exception ex)
        {
            return $"[Error] Could not read metadata: {ex.Message}";
        }
    }

    }
}
