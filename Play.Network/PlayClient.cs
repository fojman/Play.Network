using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Play.Network
{
    public class PlayClient
    {
        private readonly TcpClient _client;
        private readonly NetworkStream _steam;

        public PlayClient(TcpClient client)
        {
            _client = client;
            _steam = _client.GetStream();
        }


        public void Run(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                using (var reader = new StreamReader(_steam))
                {
                    using (var writer = new StreamWriter(_steam))
                    {
                     
                        
                    }                    
                }
            }
        }
    }
}