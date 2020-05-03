using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CaptureServer
{
    public class DirectoryListener
    {
        private readonly AppSettings _settings;
        private readonly FileProcessor _fileProcessor;

        public DirectoryListener()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json")
                .Build();

            _settings = new AppSettings();
            configuration.GetSection("DirectoryListenerSettings").Bind(_settings);

            if (!Directory.Exists(_settings.SendFolder))
            {
                Console.WriteLine($"Folder '{_settings.SendFolder}' not found. Bye!");
                Console.WriteLine("Application shutdown.");
                Console.ReadLine();
                Environment.Exit(0);
            }

            _fileProcessor = new FileProcessor(_settings);
        }

        public void Listen(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Server starts listening of '{_settings.SendFolder}' for files with '{_settings.FileFormat}' format...");
            Task.Run(() => ListenDirectory(cancellationToken), cancellationToken);
        }

        private void ListenDirectory(CancellationToken cancellationToken)
        {
            while (true)
            {
                Console.WriteLine("Listening started...");

                var files = Directory.GetFiles(_settings.SendFolder, _settings.FileFormat);
                if (files.Length > 0)
                {
                    Console.WriteLine($"Found {files.Length} files to process...");
                    // main logic
                    _fileProcessor.Process(files);
                }
                else
                {
                    Console.WriteLine("Files not found.");
                }

                Console.WriteLine("Listening ended...");

                if (cancellationToken.IsCancellationRequested)
                {
                    _fileProcessor.Dispose();
                    return;
                }

                Thread.Sleep(6000);
            }
        }
    }
}
