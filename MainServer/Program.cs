using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace MainServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                HostName = "localhost"
            };

            using (var connection = factory.CreateConnection())
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
                    var message = (Message) jsonFormatter.ReadObject(stream);

                    // TODO: check for filename
                    var path = Path.Combine(@"C:\Users\InsYst3m~\Source\Repos\MessageQueues\ReceivedDocuments", message.FileName);
                    File.WriteAllBytes(path, message.Content);

                    Console.WriteLine($"File: '{message.FileName}' with Length: '{message.Content.Length}' was successfully received.");
                };
                channel.BasicConsume(queue: "document_exchange_queue",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
