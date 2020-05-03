using Microsoft.Extensions.Configuration;
using RabbitMQWrapper;

namespace MainServer
{
    public class MessageQueueListener
    {
        private readonly AppSettings _settings;
        private readonly RabbitMQService _rabbitMqService;
        private readonly FileProcessor _fileProcessor;

        public MessageQueueListener()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json")
                .Build();

            _settings = new AppSettings();
            configuration.GetSection("MessageQueueListenerSettings").Bind(_settings);

            _rabbitMqService = new RabbitMQService();
            _fileProcessor = new FileProcessor(_settings.ReceivedFolder);
        }

        public void Listen()
        {
            _rabbitMqService.ListenForMessagesFromQueue(_fileProcessor.SaveDocument);
        }

        public void Dispose()
        {
            _rabbitMqService.Dispose();
        }
    }
}
