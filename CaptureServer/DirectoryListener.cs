using System;
using System.IO;
using System.Threading;

namespace CaptureServer
{
    public class DirectoryListener
    {
        private readonly Settings _settings;
        private readonly FileProcessor _fileProcessor;

        public DirectoryListener(Settings settings)
        {
            if (!Directory.Exists(settings.FolderPath))
            {
                Console.WriteLine($"Folder '{settings.FolderPath}' not found. Bye!");
                Console.WriteLine("Application shutdown.");
                Environment.Exit(0);
            }

            _settings = settings;
            _fileProcessor = new FileProcessor();
        }

        public void Listen()
        {
            while (true)
            {
                Console.WriteLine("Listening started...");

                var files = Directory.GetFiles(_settings.FolderPath, _settings.FileFormat);
                if (files.Length > 0)
                {
                    Console.WriteLine($"Found {files.Length} files to process...");
                    // main logic
                    _fileProcessor.Process(files);
                }

                Console.WriteLine("Listening ended...");

                Thread.Sleep(10000);
            }
        }
    }
}
