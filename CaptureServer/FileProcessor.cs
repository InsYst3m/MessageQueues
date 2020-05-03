using CaptureServer.MessageExchanger;
using RabbitMQWrapper;
using System;
using System.IO;

namespace CaptureServer
{
    public class FileProcessor : IDisposable
    {
        private readonly MessageExchangeService _messageExchangeService;

        public FileProcessor()
        {
            _messageExchangeService = new MessageExchangeService();
        }

        public void Process(string[] files)
        {
            Console.WriteLine("File processor started working...");

            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var fileContent = File.ReadAllBytes(file);

                _messageExchangeService.SendMessage(new Message
                {
                    FileName = Path.GetFileName(file),
                    Content = fileContent
                });

                MoveDocumentToCompleted(file);
            }

        }

        #region Private methods

        private void MoveDocumentToCompleted(string path)
        {
            var fileName = Path.GetFileName(path);
            var completedFolder = @"C:\Users\InsYst3m~\Source\Repos\MessageQueuesFilesFolder\Completed";

            var newPath = Path.Combine(completedFolder, fileName);

            if (File.Exists(newPath))
            {
                fileName = $"test_{Guid.NewGuid()}.pdf";
                newPath = Path.Combine(completedFolder, fileName);
            }

            File.Move(path, newPath);
        }

        #endregion

        #region IDisposable Support

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _messageExchangeService.Dispose();
                }

                disposed = true;
            }
        }

        ~FileProcessor()
        {
            Dispose(false);
        }

        #endregion
    }
}
