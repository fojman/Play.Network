using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Play.Network
{
    public class AsyncTestListener
    {
        public async Task StartAsyncListener(ServerConfig config, CancellationToken token)
        {
            var listener = new TcpListener(config.Address, config.Port);
            token.Register(listener.Stop);

            listener.Start();

            while (!token.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync()
                    .ConfigureAwait(continueOnCapturedContext: false);

                client.Client.
            }
        }
    }
}