using RabbitMQ.Client;
using System.IO;
using System.Runtime.Serialization.Json;

namespace CaptureServer.RabbitMQ
{
    public class RabbitMqService
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

        public void SendMessage(Message message)
        {
            var stream = new MemoryStream();
            var jsonFormatter = new DataContractJsonSerializer(typeof(Message));
            jsonFormatter.WriteObject(stream, message);

            using (var connection = GetRabbitConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "document_exchange_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                var body = stream.ToArray();
                // TODO: move file to another folder when complete

                channel.BasicPublish(exchange: "", routingKey: "document_exchange_queue", mandatory: false, basicProperties: null, body: body);

                System.Console.WriteLine($"File: '{message.FileName}' with Length: '{message.Content.Length}' was successfully sent.");
            } 
        }
    }
}
