namespace CaptureServer
{
    public class Settings
    {
        public string FolderPath { get; set; }
        public string FileFormat { get; set; }

        public Settings(string[] args)
        {
            if (args.Length == 0)
            {
                FolderPath = @"C:\Users\InsYst3m~\Source\Repos\MessageQueues\Documents";
                FileFormat = "*.pdf";
            }
            else
            {
                FolderPath = args[0];
                FileFormat = args[1];
            }
        }
    }
}
