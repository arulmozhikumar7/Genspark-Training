using FileReaderApp.Interfaces;
using FileReaderApp.Services;
using System;

namespace FileReaderApp.Factories
{
    public static class FileReaderFactory
    {
        public static IFileReader CreateReader(string extension)
        {
            return extension.ToLower() switch
            {
                ".txt" => TxtFileReader.Instance,
                ".csv" => CsvFileReader.Instance,
                _ => throw new NotSupportedException($"File extension '{extension}' not supported.")
            };
        }
    }
}
