using System.Threading;

namespace CaptureServer
{
    public interface IDirectoryListener
    {
        void Listen(CancellationToken cancellationToken);
    }
}