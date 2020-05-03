using System;

namespace CaptureServer
{
    // TODO: 1) вынести RabbitMQ в отдельную dll
    
    // TODO: 3) CaptureServer - перенести обработанный файл в другую папку
    // TODO: 4) добавить json config file
    // TODO: 5) разбить сообщение на несколько мелких (как их связать друг с другом)

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
