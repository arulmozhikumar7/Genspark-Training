using FileReaderApp.Factories;
using FileReaderApp.Interfaces;
using System;

namespace FileReaderApp
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter the full path of the file to read:");
            string? path = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(path) || !System.IO.File.Exists(path))
            {
                Console.WriteLine("Invalid file path.");
                return;
            }

            string ext = System.IO.Path.GetExtension(path);

            try
            {
                IFileReader reader = FileReaderFactory.CreateReader(ext);
                string content = reader.ReadAll(path);
                Console.WriteLine("\n--- File Content Start ---");
                Console.WriteLine(content);
                Console.WriteLine("--- File Content End ---\n");

                reader.Close();
                Console.WriteLine("File closed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
