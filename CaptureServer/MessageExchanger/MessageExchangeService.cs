using CaptureServer.RabbitMQ;

namespace CaptureServer.MessageExchanger
{
    public class MessageExchangeService
    {
        private readonly RabbitMqService _rabbitConnection;

        public MessageExchangeService()
        {
            _rabbitConnection = new RabbitMqService();
        }

        public void SendMessage(Message message)
        {
            _rabbitConnection.SendMessage(message);
        }
    }
}
