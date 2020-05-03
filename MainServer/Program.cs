using System;
using System.IO;

namespace MainServer
{
    public class Program
    {
        private const string ReceivedFilesFolder = @"C:\Users\InsYst3m~\Source\Repos\MessageQueues\ReceivedDocuments";

        public static void Main(string[] args)
        {
            
        }

        private static string GenerateNewFileNameIfExists(string fileName)
        {
            var path = Path.Combine(ReceivedFilesFolder, fileName);
            if (File.Exists(path))
            {
                var directory = Path.GetDirectoryName(path);
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
                var fileExtension = Path.GetExtension(path);
                var newFileName = $"{fileNameWithoutExtension}_{Guid.NewGuid()}{fileExtension}";

                return Path.Combine(directory, newFileName);
            }

            return path;
        }
    }
}
