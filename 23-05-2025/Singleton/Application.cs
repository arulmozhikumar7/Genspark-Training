using FileReaderApp.Interfaces;
using FileReaderApp.Services;

namespace FileReaderApp
{
    public class Application
    {
        private readonly IFileService fileService;
        private readonly IContentDisplayer displayer;

        public Application(IFileService fileService, IContentDisplayer displayer)
        {
            this.fileService = fileService;
            this.displayer = displayer;
        }

        public void Run(string filePath)
        {
            if (fileService is FileService fs)
            {
                fs.Open(filePath);
            }

            string content = fileService.ReadAll();
            displayer.Display(content);
            fileService.Close(); 
        }
    }
}
