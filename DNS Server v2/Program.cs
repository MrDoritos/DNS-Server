using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections;

namespace DNS_Server_v2
{
    class Program
    {
        public static DNS DNS;
        public static Socket Socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
        public static IPEndPoint EndPoint;

        static void Main(string[] args)
        {
            EndPoint = new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 53);
            DNS = new DNS(Socket, EndPoint, new IPEndPoint(IPAddress.Any, 53));
            DNS.Start();
        }
    }
}
