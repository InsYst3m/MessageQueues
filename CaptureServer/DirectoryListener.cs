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
                Console.WriteLine($"Folder '{settings.FolderPath}' not found.");
            }

            _settings = settings;
            _fileProcessor = new FileProcessor();
        }

        public void Listen()
        {
            while (true)
            {
                var files = Directory.GetFiles(_settings.FolderPath, _settings.FileFormat);
                if (files.Length > 0)
                {
                    // main logic
                    _fileProcessor.Process(files);
                }

                Thread.Sleep(3000);
            }
        }
    }
}
