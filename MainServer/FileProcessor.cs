using RabbitMQWrapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;

namespace MainServer
{
    public class FileProcessor
    {
        private readonly string _receivedFilesFolder;
        private List<Message> uncompletedMessages;

        public FileProcessor(string receivedFilesFolder)
        {
            _receivedFilesFolder = receivedFilesFolder;
            uncompletedMessages = new List<Message>();
        }

        public void SaveDocument(ReadOnlyMemory<byte> body)
        {
            var message = DeserializeMessage(body.ToArray());

            if (message.MessageParts == 0)
            {
                SaveFile(message);      
            } 
            else
            {
                MakeDocumentFromParts(message);
            }

            Console.WriteLine($"Received file with name: {message.FileName} and content length: {message.Content.Length}.");
        }

        private void MakeDocumentFromParts(Message message)
        {
            // если уникален гуид и номер пакета, добавляем в лист
            if (!uncompletedMessages.Exists(m => m.Guid == message.Guid && m.MessageNumber == message.MessageNumber))
            {
                uncompletedMessages.Add(message);
            }

            // попытка склеить сообщения
            var messagesParts = uncompletedMessages.Where(m => m.Guid == message.Guid).ToList();
            if (messagesParts.Count == messagesParts[0].MessageParts)
            {
                // склейка сообщений
                var resultMessage = new Message
                {
                    FileName = messagesParts[0].FileName,
                    Content = messagesParts.SelectMany(m => m.Content).ToArray()
                };

                // сохраняем документ
                SaveFile(resultMessage);
            }
        }

        private void SaveFile(Message message)
        {
            var path = GenerateNewFileNameIfExists(message.FileName);
            File.WriteAllBytes(path, message.Content);
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
