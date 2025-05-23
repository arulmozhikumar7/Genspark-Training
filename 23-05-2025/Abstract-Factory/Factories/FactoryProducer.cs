using FileApp.Interfaces;
using System;
using System.IO;

namespace FileApp.Factories
{
    public static class FactoryProducer
    {
        public static IFileFactory GetFactory(string extension)
        {
            return extension.ToLower() switch
            {
                ".txt" => new TxtFileFactory(),
                ".csv" => new CsvFileFactory(),
                _ => throw new NotSupportedException($"Extension {extension} is not supported")
            };
        }
    }
}