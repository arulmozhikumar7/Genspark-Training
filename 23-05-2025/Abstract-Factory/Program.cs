using FileApp.Factories;
using FileApp.Interfaces;
using System;
using System.IO;

namespace FileApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter file path: ");
            string filePath = Console.ReadLine() ?? "";
            string extension = Path.GetExtension(filePath);

            try
            {
                IFileFactory factory = FactoryProducer.GetFactory(extension);

                using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                {
                    var reader = factory.CreateReader(stream);
                    var writer = factory.CreateWriter(stream);

                    stream.Seek(0, SeekOrigin.Begin);
                    string content = reader.Read();
                    Console.WriteLine("\nExisting content:");
                    Console.WriteLine(string.IsNullOrWhiteSpace(content) ? "(empty)" : content);

                    Console.Write("\nEnter content to append: ");
                    string newContent = Console.ReadLine() ?? "";

                    stream.Seek(0, SeekOrigin.End);
                    writer.Write(newContent);
                    Console.WriteLine("Content written successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nExiting...");
        }
    }
}