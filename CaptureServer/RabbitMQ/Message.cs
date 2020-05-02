namespace CaptureServer.RabbitMQ
{
    public class Message
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }
}
