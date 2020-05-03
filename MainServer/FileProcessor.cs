using RabbitMQWrapper;
using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace MainServer
{
    public class FileProcessor
    {
        public readonly string _receivedFilesFolder;

        public FileProcessor(string receivedFilesFolder)
        {
            _receivedFilesFolder = receivedFilesFolder;
        }

        public void SaveDocument(ReadOnlyMemory<byte> body)
        {
            var message = DeserializeMessage(body.ToArray());

            var path = GenerateNewFileNameIfExists(message.FileName);
            File.WriteAllBytes(path, message.Content);

            Console.WriteLine($"Received file with name: {message.FileName} and content length: {message.Content.Length}.");
        }

        private Message DeserializeMessage(byte[] message)
        {
            var stream = new MemoryStream(message);
            var jsonFormatter = new DataContractJsonSerializer(typeof(Message));

            return (Message)jsonFormatter.ReadObject(stream);
        }

        private string GenerateNewFileNameIfExists(string fileName)
        {
            var path = Path.Combine(_receivedFilesFolder, fileName);
            if (File.Exists(path))
            {
                var directory = Path.GetDirectoryName(path);
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
                var fileExtension = Path.GetExtension(path);
                var newFileName = $"{fileNameWithoutExtension}_{Guid.NewGuid()}{fileExtension}";

                return Path.Combine(directory, newFileName);
            }

            return path;
        }
    }
}
