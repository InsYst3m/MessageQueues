using System;
using System.Threading;

namespace CaptureServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Capture server started working...");

            Console.WriteLine("Initializing Directory Listener...");
            var directoryListener = new DirectoryListener();

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                directoryListener.Listen(cancellationTokenSource.Token);

                while (true)
                {
                    var input = Console.ReadLine();
                    if (input == "exit")
                    {
                        cancellationTokenSource.Cancel();
                        Console.WriteLine("Capture Server shutdown.");
                        break;
                    }
                }
            }
        }
    }
}
