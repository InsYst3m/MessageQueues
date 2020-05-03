using System;

namespace MainServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var messageQueueListener = new MessageQueueListener();
            messageQueueListener.Listen();

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();

            messageQueueListener.Dispose();
        }
    }
}
