using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS_Server_v2.DNS_Main.Message
{
    class Header
    {
        public ushort Id { get; }
        public ReplyCode ReplyCode { get; }
        public ushort QuestionCount { get; }
        public ushort AnswerCount { get; }
        public ushort NameServerCount { get; }
        public ushort AdditionalRecordCount { get; }

        public Header(ushort Id, ushort QuestionCount, ushort AnswerCount, ushort NameServerCount, ushort AdditionalRecordCount, ReplyCode ReplyCode)
        {
            this.Id = Id;
            this.ReplyCode = ReplyCode;
            this.QuestionCount = QuestionCount;
            this.AnswerCount = AnswerCount;
            this.NameServerCount = NameServerCount;
            this.AdditionalRecordCount = AdditionalRecordCount;
        }

        public static Header Parse(byte[] buffer)
        {
            if (buffer.Length < 29) { return null; }
            UInt16 id = buffer[0].Combine(buffer[1]);
            ReplyCode replyCode = ReplyCode.Parse(new byte[] { buffer[2], buffer[3] });
            UInt16 questions = buffer[4].Combine(buffer[5]);
            UInt16 answers = buffer[6].Combine(buffer[7]);
            UInt16 nameservers = buffer[8].Combine(buffer[9]);
            UInt16 additional = buffer[10].Combine(buffer[11]);
            return new Header(id, questions, answers, nameservers, additional, replyCode);
        }
    }
}
