using System;
using FileReaderApp.Interfaces;

namespace FileReaderApp.Display
{
    public class ConsoleDisplayer : IContentDisplayer
    {
        public void Display(string content)
        {
            Console.WriteLine("----- FILE CONTENT -----");
            Console.WriteLine(content);
        }
    }
}
