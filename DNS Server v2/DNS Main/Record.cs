using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace DNS_Server_v2
{
    class Record
    {
        public string FQDN { get; }
        public string TopLevelDomain { get; }
        public IPAddress[] A = new IPAddress[0];
        public IPAddress[] AAAA = new IPAddress[0];
        public string[] CNAME = new string[0];

        public Record(string FQDN, string TopLevelDomain) { this.FQDN = FQDN; this.TopLevelDomain = TopLevelDomain; }

        public enum RecordType
        {
           A = 0x0001,
           NS = 0x0002,
           CNAME = 0x0005,
           SOA = 0x0006,
           WKS = 0x000B,
           PTR = 0x000C,
           MX = 0x000F,
           SRV = 0x0021,
           AAAA = 0x01C,
           Any = 0x00FF,
        }

        public enum RecordClass
        {
            IN = 0x0001,
        }
    }
}
