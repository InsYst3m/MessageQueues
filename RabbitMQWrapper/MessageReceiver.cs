using RabbitMQ.Client;
using System;

namespace RabbitMQWrapper
{
    public class MessageReceiver : DefaultBasicConsumer
    {
        private readonly IModel _channel;
        private readonly Action<ReadOnlyMemory<byte>> _callback;

        public MessageReceiver(IModel channel, Action<ReadOnlyMemory<byte>> callback)
        {
            _channel = channel;
            _callback = callback;
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered,
            string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            _callback(body);
            _channel.BasicAck(deliveryTag, false);
        }
    }
}