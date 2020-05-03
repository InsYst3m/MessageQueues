using RabbitMQWrapper;
using System;

namespace CaptureServer.MessageExchanger
{
    public class MessageExchangeService : IDisposable
    {
        private readonly RabbitMQService _rabbitMqService;

        public MessageExchangeService()
        {
            _rabbitMqService = new RabbitMQService();
        }

        public void SendMessage(Message message)
        {
            _rabbitMqService.SendMessageToQueue(message);
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
                    _rabbitMqService.Dispose();
                }

                disposed = true;
            }
        }

        ~MessageExchangeService()
        {
            Dispose(false);
        }

        #endregion
    }
}
