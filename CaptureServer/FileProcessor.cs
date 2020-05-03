using CaptureServer.MessageExchanger;
using RabbitMQWrapper;
using System;
using System.IO;

namespace CaptureServer
{
    public class FileProcessor : IDisposable
    {
        private readonly AppSettings _settings;
        private readonly MessageExchangeService _messageExchangeService;

        public FileProcessor(AppSettings settings)
        {
            _settings = settings;
            _messageExchangeService = new MessageExchangeService();
        }

        public void Process(string[] files)
        {
            Console.WriteLine("File processor started working...");

            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var fileContent = File.ReadAllBytes(file);

                if (_settings.IsSplitMessagesEnabled)
                {
                    var messageSize = 1024;
                    var fileContentSize = fileContent.Length * sizeof(byte);

                    if (fileContentSize > messageSize)
                    {
                        int messageParts = fileContentSize % messageSize != 0 
                            ? (fileContentSize / messageSize) + 1 
                            : fileContentSize / messageSize;

                        var messageGuid = Guid.NewGuid();

                        for (int j = 0; j < messageParts; j++)
                        {
                            var startIndex = j * messageSize;
                            var lastIndex = (j + 1) * messageSize;
                            if (j == messageParts - 1)
                            {
                                messageSize = fileContentSize - j * messageSize;
                                lastIndex = fileContentSize;
                            }

                            var message = new Message
                            {
                                Guid = messageGuid,
                                FileName = Path.GetFileName(file),
                                MessageNumber = j,
                                MessageParts = messageParts,
                                Content = GetSplittedArray(fileContent, startIndex, lastIndex, messageSize)
                            };

                            _messageExchangeService.SendMessage(message);
                        }
                    } 
                    else
                    {
                        _messageExchangeService.SendMessage(new Message
                        {
                            FileName = Path.GetFileName(file),
                            Content = fileContent,
                            MessageNumber = 0,
                            MessageParts = 0
                        });
                    }
                } 
                else
                {
                    _messageExchangeService.SendMessage(new Message
                    {
                        FileName = Path.GetFileName(file),
                        Content = fileContent,
                        MessageNumber = 0,
                        MessageParts = 0
                    });
                }   

                MoveDocumentToCompleted(file);
            }

        }

        #region Private methods

        private void MoveDocumentToCompleted(string path)
        {
            var fileName = Path.GetFileName(path);

            var newPath = Path.Combine(_settings.CompletedFolder, fileName);

            if (File.Exists(newPath))
            {
                fileName = $"test_{Guid.NewGuid()}.pdf";
                newPath = Path.Combine(_settings.CompletedFolder, fileName);
            }

            File.Move(path, newPath);
        }

        private byte[] GetSplittedArray(byte[] sourceArray, int startIndex, int lastIndex, int arrayLength)
        {
            var result = new byte[arrayLength];

            for (int i = 0; i < arrayLength; i++)
            {
                if (startIndex + i == lastIndex)
                {
                    break;
                }

                result[i] = sourceArray[startIndex + i];
            }

            return result;
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
