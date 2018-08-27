using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS_Server_v2
{
    class AnswerMessage
    {
        public Message Message { get; }

        public AnswerMessage(Message Message)
        {
            this.Message = Message;
        }

        //WE NEED TO SERIALIZE!!!!
        public byte[] getRaw()
        {

            return null;
        }
    }
}
