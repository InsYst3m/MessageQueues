using System;
using System.IO;

namespace CaptureServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Capture server started working...");

            Console.WriteLine("Initializing Settings...");
            var settings = new Settings(args);

            Console.WriteLine("Initializing Directory Listener...");
            var directoryListener = new DirectoryListener(settings);

            Console.WriteLine($"Server starts listening of '{settings.FolderPath}' for files with '{settings.FileFormat}' format...");
            directoryListener.Listen();

            Console.ReadLine();
        }
    }
}
