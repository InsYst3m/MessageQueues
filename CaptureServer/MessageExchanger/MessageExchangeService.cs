using RabbitMQWrapper;

namespace CaptureServer.MessageExchanger
{
    public class MessageExchangeService
    {
        private readonly RabbitMQService _rabbitConnection;

        public MessageExchangeService()
        {
            _rabbitConnection = new RabbitMQService();
        }

        public void SendMessage(Message message)
        {
            _rabbitConnection.SendMessageToQueue(message);
        }
    }
}
