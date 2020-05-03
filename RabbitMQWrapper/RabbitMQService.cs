using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.IO;
using System.Runtime.Serialization.Json;

namespace RabbitMQWrapper
{
    public class RabbitMQService
    {
        // TODO: configuration json file for rabbitMQ settings (UserName + Password; host)
        private IConnection GetRabbitConnection()
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                HostName = "localhost"
            };

            IConnection connection = factory.CreateConnection();

            return connection;
        }

        public void SendMessageToQueue(Message message)
        {
            var stream = new MemoryStream();
            var jsonFormatter = new DataContractJsonSerializer(typeof(Message));
            jsonFormatter.WriteObject(stream, message);

            using (var connection = GetRabbitConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "document_exchange_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                var body = stream.ToArray();

                channel.BasicPublish(exchange: "", routingKey: "document_exchange_queue", mandatory: false, basicProperties: null, body: body);
            }
        }

        public void ReceiveMessagesFromQueue(string folderToSaveDocument)
        {
            using (var connection = GetRabbitConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "document_exchange_queue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;

                    var stream = new MemoryStream(body.ToArray());
                    var jsonFormatter = new DataContractJsonSerializer(typeof(Message));
                    var message = (Message)jsonFormatter.ReadObject(stream);

                    // TODO: save file to folder
                    //var path = GenerateNewFileNameIfExists(message.FileName);
                    //File.WriteAllBytes(path, message.Content);
                };
                channel.BasicConsume(queue: "document_exchange_queue",
                                     autoAck: true,
                                     consumer: consumer);
            }
        }
    }
}
