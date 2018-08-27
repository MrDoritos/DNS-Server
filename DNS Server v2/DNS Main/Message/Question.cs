using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS_Server_v2.DNS_Main.Message
{
    class Question
    {
        public Record.RecordClass RecordClass { get; }
        public Record.RecordType RecordType { get; }
        public Record Record { get; }
        public Question(Record Record, Record.RecordClass RecordClass, Record.RecordType RecordType) { this.Record = Record; this.RecordClass = RecordClass; this.RecordType = RecordType; }
    }
}
