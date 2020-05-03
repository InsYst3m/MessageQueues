using System;

namespace RabbitMQWrapper
{
    [Serializable]
    public class Message
    {
        public Guid Guid { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }

        public int MessageNumber { get; set; }
        public int MessageParts { get; set; }
    }
}
