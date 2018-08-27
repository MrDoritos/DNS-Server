using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNS_Server_v2.DNS_Main.Message;
using DNS_Server_v2.DNS_Main;
using System.Net;

namespace DNS_Server_v2
{
    class Message
    {
        public Header Header { get; }
        public EndPoint endPoint;
        public IEnumerable<Question> Questions { get; }
        public IEnumerable<Answer> Answers { get; }

        /// <summary>
        /// Constructor for a query message
        /// </summary>
        /// <param name="Header"></param>
        /// <param name="Questions"></param>
        public Message(Header Header, IEnumerable<Question> Questions)
        {            
            this.Header = Header;
            this.Questions = Questions;
        }

        /// <summary>
        /// Constructor for a response message
        /// </summary>
        /// <param name="Header"></param>
        /// <param name="Answers"></param>
        public Message(Header Header, IEnumerable<Answer> Answers)
        {
            this.Header = Header;
            this.Answers = Answers;
        }

        public static Message Parse(byte[] message)
        {
            if (message.Length < 29)
                throw new InvalidQueryException(); 
            Header header = Header.Parse(message);
            if (header.ReplyCode.Response)
                throw new InvalidQueryException();
            List<Question> questions = new List<Question>();
            //Skip the bytes used for the header
            message = message.Skip(12).ToArray();

            //Retrieve every queried record
            for (int i = 0; i < header.QuestionCount; i++)
            {
                Record r;
                Record.RecordType rt;
                Record.RecordClass rc;
                List<string> zonesList = new List<string>();
                string[] zones;
                while (true)
                {
                    if (message[0] == 0) { message = message.Skip(1).ToArray(); break; }
                    string zone = Encoding.ASCII.GetString(message.Skip(1).Take(message[0]).ToArray());
                    zonesList.Add(zone);
                    message = message.Skip(message[0] + 1).ToArray();
                }
                zones = zonesList.ToArray();
                //Retrieve record type
                rt = (Record.RecordType)message[0].Combine(message[1]);
                message = message.Skip(2).ToArray();
                //Retrieve record class
                rc = (Record.RecordClass)message[0].Combine(message[1]);
                message = message.Skip(2).ToArray();
                string FQDN = zones.Select(n => n + ".").Combine();
                r = new Record(FQDN, zones.Last());
                questions.Add(new Question(r, rc, rt));
            }
            return new Message(header, questions);
        }
    }
}
