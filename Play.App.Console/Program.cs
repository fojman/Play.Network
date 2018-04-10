using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Play.Network;

using NLog;

namespace Play.App.Console
{
    class Program
    {
        private static ILogger Log = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }


        public static async Task MainAsync()
        {
            var stopEvent = new ManualResetEvent(false);
            Log.Info($"+ Starting test server");
            var cts = new CancellationTokenSource();

            var config = new ServerConfig();

            using (var server = new PlayListener(config))
            {
                var servierAsyncTask = server.StartServer(cts.Token);

                var clients = new List<Task>();
/*                Task.Run(() =>
                {
                    System.Console.ReadLine();
                    Log.Debug("Stopping server");
                    stopEvent.Set();
                });*/

                for (int i = 0; i < 22; i++)
                {
                    var i1 = i;
                    clients.Add(Task.Run(() =>
                    {
                        Log.Info($"Client {i1} connecting to server.");
                        using (var client = new TcpClient())
                        {


                            
                            client.Connect("127.0.0.1", config.Port);
                            var request = Encoding.ASCII.GetBytes($"send to client from {i1}");
                            var response = new byte[1024];
                            using (var stream = client.GetStream())
                            {

                                stream.Write(request, 0, request.Length);
                                stream.Read(response, 0, response.Length);
                            }
                        }

                    }));
                }


                Task.WaitAll(clients.ToArray(), TimeSpan.FromSeconds(10));

                // then continue to wait on server task

                servierAsyncTask.GetAwaiter().GetResult();


            }

        }
    }
}
