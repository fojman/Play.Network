using System.Net;

namespace Play.Network
{
    public class ServerConfig
    {
        public IPAddress Address { get; set; } = IPAddress.Any;

        public int Port { get; set; } = 12345;
    }
}