using CaptureServer.MessageExchanger;
using RabbitMQWrapper;
using System;
using System.IO;

namespace CaptureServer
{
    public class FileProcessor
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
            }

        }

        #region Private methods

        private bool CheckFileSize()
        {
            return false;
        }

        #endregion
    }
}
