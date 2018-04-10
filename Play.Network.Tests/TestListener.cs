using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Play.Network.Tests
{
    public class TestListener
    {
        [SetUp]
        public void SetUp() { }

        [Test]
        public void Listener_ManyClientsConnected_ShouldBeOk()
        {

            var cts = new CancellationTokenSource();

            var config = new ServerConfig();

            using (var server = new PlayListener(config))
            {
                var servierAsyncTask = server.StartServer(cts.Token);

                var clients = new List<Task>();

                for (int i = 0; i < 10; i++)
                {
                    clients.Add(Task.Run(() =>
                    {

                        using (var client = new TcpClient())
                        {
                         
                            
                            client.Connect("127.0.0.1", config.Port);
                            var request = Encoding.ASCII.GetBytes($"send to client from {client.ToString()}");
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
