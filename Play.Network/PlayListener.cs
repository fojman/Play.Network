using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace Play.Network
{
    public class PlayListener: IDisposable
    {
        private static ILogger Log = LogManager.GetCurrentClassLogger();
        private TcpListener _listener;

        public PlayListener(ServerConfig config)
        {
            Log.Debug("Creating listener");
            _listener = new TcpListener(config.Address, config.Port);
        }

        public async Task StartServer(CancellationToken ct)
        {
            _listener.Start();

            while (!ct.IsCancellationRequested)
            {

                await Task.Run( async () =>
                {
                    Log.Info("Listening for new clients...");
                    var tcpClient = await _listener.AcceptTcpClientAsync().ConfigureAwait(continueOnCapturedContext: false); // todo: why do we do ConfigureAwait()?

                    Log.Info($"Client connected - {tcpClient.Client.RemoteEndPoint}");

                    await Task.Run( async () =>
                    {
                        var stream = tcpClient.GetStream();
                        var buffer = new byte[1024];
                        stream.Read(buffer, 0, buffer.Length);

                        Log.Info($"Read from client: {Encoding.ASCII.GetString(buffer)}");
                        Log.Info($"Writing exit command to client - exit, but wait a bit...");

                        await  Task.Delay(new Random().Next(1,3)*1000);

                        var toSend = Encoding.ASCII.GetBytes("exit");
                        stream.Write(toSend, 0, toSend.Length);

                    });

                });

            }
        }

        public void Dispose()
        {
            _listener.Stop();
        }
    }
}