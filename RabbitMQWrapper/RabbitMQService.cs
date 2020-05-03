using RabbitMQ.Client;
using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace RabbitMQWrapper
{
    public class RabbitMQService : IDisposable
    {
        private readonly IConnection _rabbitMqConnection;
        private readonly IModel _channel;

        public RabbitMQService()
        {
            _rabbitMqConnection = GetRabbitConnection();
            _channel = _rabbitMqConnection.CreateModel();
        }

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

            _channel.QueueDeclare(queue: "document_exchange_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var body = stream.ToArray();

            _channel.BasicPublish(exchange: "", routingKey: "document_exchange_queue", mandatory: false, basicProperties: null, body: body);
        }

        public void ListenForMessagesFromQueue(Action<ReadOnlyMemory<byte>> callback)
        {
            _channel.QueueDeclare(queue: "document_exchange_queue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            MessageReceiver messageReceiver = new MessageReceiver(_channel, callback);

            _channel.BasicConsume(queue: "document_exchange_queue",
                                 autoAck: false,
                                 consumer: messageReceiver);

        }

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
                    _rabbitMqConnection.Dispose();
                    _channel.Dispose();
                }

                disposed = true;
            }
        }

        ~RabbitMQService()
        {
            Dispose(false);
        }

        #endregion
    }
}
