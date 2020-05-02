using CaptureServer.RabbitMQ;
using System.Text;

namespace CaptureServer.MessageExchanger
{
    public class MessageExchangeService
    {
        private readonly RabbitConnection _rabbitConnection;

        public MessageExchangeService()
        {
            _rabbitConnection = new RabbitConnection();
        }

        public void SendMessage(Message message)
        {
            using(var connection = _rabbitConnection.GetRabbitConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    string testMessage = "Hello world";
                    var body = Encoding.UTF8.GetBytes(testMessage);

                    channel.BasicPublish(exchange: "", 
                                         routingKey: "hello",
                                         mandatory: false,
                                         basicProperties: null, 
                                         body: body);

                    System.Console.WriteLine($"Message {testMessage} was successfully sent.");
                }
            }
        }
    }
}
