using CaptureServer.RabbitMQ;
using System;
using System.IO;

namespace CaptureServer
{
    public class FileProcessor
    {
        public void Process(string[] files)
        {
            // прокинуть в message queue
            // если по размеру не влазит, 
            // разбить на неск массивов байтов с номером и прокинуть это все в очередь

            Console.WriteLine("File processor started working...");

            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var fileContent = File.ReadAllBytes(file);

                SendFileToMessageQueue(new Message
                {
                    FileName = Path.GetFileName(file),
                    Content = fileContent
                });
            }

        }

        #region Private methods

        private void SendFileToMessageQueue(Message message)
        {

        }

        private bool CheckFileSize()
        {
            return false;
        }

        #endregion
    }
}
