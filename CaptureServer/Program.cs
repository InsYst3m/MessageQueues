using System;

namespace CaptureServer
{
    // TODO: 4) добавить json config file
    // TODO: 5) refactoring

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
