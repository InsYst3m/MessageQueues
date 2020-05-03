using RabbitMQWrapper;
using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace MainServer
{
    public class Program
    {
        private const string ReceivedFilesFolder = @"C:\Users\InsYst3m~\Source\Repos\MessageQueuesFilesFolder\Received";
        private static readonly RabbitMQService _rabbitMqService = new RabbitMQService();

        public static void Main(string[] args)
        {
            _rabbitMqService.ListenForMessagesFromQueue(SaveDocument);

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();

            _rabbitMqService.Dispose();
        }

        private static void SaveDocument(ReadOnlyMemory<byte> body)
        {
            var message = DeserializeMessage(body.ToArray());

            var path = GenerateNewFileNameIfExists(message.FileName);
            File.WriteAllBytes(path, message.Content);

            Console.WriteLine($"Received file with name: {message.FileName} and content length: {message.Content.Length}.");
        }

        private static Message DeserializeMessage(byte[] message)
        {
            var stream = new MemoryStream(message);
            var jsonFormatter = new DataContractJsonSerializer(typeof(Message));

            return (Message)jsonFormatter.ReadObject(stream);
        }

        private static string GenerateNewFileNameIfExists(string fileName)
        {
            var path = Path.Combine(ReceivedFilesFolder, fileName);
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
