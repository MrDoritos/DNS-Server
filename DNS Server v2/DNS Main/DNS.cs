using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Collections;

namespace DNS_Server_v2
{
    class DNS
    {
        private Socket _socket;
        private EndPoint _groupEp;
        private bool _started = false;
        private Resolve _resolver;
        private EndPoint _bind;
        private List<Record> Records { get; }

        public DNS(Socket socket, EndPoint bind, EndPoint endPoint)
        {
            if (socket.ProtocolType == ProtocolType.Udp && socket.SocketType == SocketType.Dgram && socket.IsBound) { _socket = socket; }
            else { throw new InvalidOperationException("The socket should be the UDP protocol, Dgram type, and bound"); }
            _socket = socket;
            _bind = bind;
            _groupEp = endPoint;
            Records = new List<Record>() { new Record("iansweb.org.", "org") };
            Records[0].A = new IPAddress[] { new IPAddress(new byte[] { 192, 168, 0, 1 }) };
            _resolver = new Resolve(Records);
        }
        
        public void Start()
        {
            try
            {
                _socket.Bind(_bind);
            }
            catch (Exception)
            {
                Console.WriteLine($"Could not start socket on {_bind.ToString()}");
                return;
            }
            _started = true;
            while (_started)
            {
                var message = RecieveMessage();
                Console.WriteLine("Recieved Message");
                Console.WriteLine(message.Header.Id);
                var resolve = _resolver.ResolveRecord(message.Questions.First().Record);
                if (resolve != null)
                    Console.WriteLine($"Resolved {resolve.A.Combine()} from {resolve.FQDN}");
                else
                    Console.WriteLine($"Could not resolve {message.Questions.First().Record.FQDN}");
            }
        }

        public void Close()
        {
            _started = false;
        }

        private byte[] Recieve()
        {
            while (true)
            {
                if (_socket.Available > 0)
                {
                    byte[] toReturn = new byte[_socket.Available];
                    _socket.ReceiveFrom(toReturn, ref _groupEp);
                    return toReturn;
                }
                System.Threading.Thread.Sleep(10);
            }
        }

        private Message RecieveMessage()
        {
            while (true)
            {
                if (_socket.Available > 0)
                {
                    byte[] toReturn = new byte[_socket.Available];
                    _socket.ReceiveFrom(toReturn, ref _groupEp);
                    var message = Message.Parse(toReturn);
                    message.endPoint = _groupEp;
                    return message;
                }
                System.Threading.Thread.Sleep(10);
            }
        }
    }

    public static class Extensions
    {
        public static byte ToByte(this BitArray bitArray)
        {
            byte num = 0;
            for (int i = 0; i < bitArray.Length; i++)
            {
                if (bitArray[i])
                {
                    switch (i)
                    {
                        case 7:
                            num = (byte)(num + 128);
                            break;
                        case 6:
                            num = (byte)(num + 64);
                            break;
                        case 5:
                            num = (byte)(num + 32);
                            break;
                        case 4:
                            num = (byte)(num + 16);
                            break;
                        case 3:
                            num = (byte)(num + 8);
                            break;
                        case 2:
                            num = (byte)(num + 4);
                            break;
                        case 1:
                            num = (byte)(num + 2);
                            break;
                        case 0:
                            num = (byte)(num + 1);
                            break;
                    }
                }
            }
            return num;
        }

        static public UInt16 Combine(this byte b1, byte b2)
        {
            UInt16 combined = (UInt16)(b1 << 8 | b2);
            return combined;
        }

        static public string Combine(this IEnumerable<string> s)
        {
            string toreturn = string.Empty;
            foreach (var a in s)
                toreturn += a;
            return toreturn;
        }

        static public string Combine(this IPAddress[] IpAddresses)
        {
            string toreturn = string.Empty;
            foreach (var a in IpAddresses)
                toreturn += (a.ToString() + ", ");
            return toreturn;
        }
    }
}
