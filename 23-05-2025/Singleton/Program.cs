using FileReaderApp.Display;
using FileReaderApp.Services;
using FileReaderApp;

namespace FileReaderApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "sample.txt"; 
            var app = new Application(FileService.Instance, new ConsoleDisplayer());
            app.Run(filePath);
        }
    }
}
