using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS_Server_v2.DNS_Main.Message
{
    class ReplyCode
    {
        public bool Response { get; }
        public bool AuthoritativeAnswer { get; }
        public bool Truncation { get; }
        public bool RecursionDesired { get; }
        public bool RecursionAvailable { get; }
        public Opcode OpCode { get; }
        public Responsecode ResponseCode { get; }
        
        public ReplyCode(bool Response, bool AuthoritativeAnswer, bool Truncation, bool RecursionDesired, bool RecursionAvailable, Opcode OpCode, Responsecode ResponseCode)
        {
            this.Response = Response;
            this.AuthoritativeAnswer = AuthoritativeAnswer;
            this.Truncation = Truncation;
            this.RecursionDesired = RecursionDesired;
            this.RecursionAvailable = RecursionAvailable;
            this.OpCode = OpCode;
            this.ResponseCode = ResponseCode;
        }

        public static ReplyCode Parse(byte[] theTwoOctets)
        {
            if (!(theTwoOctets.Length > 1 && theTwoOctets.Length < 3)) { return null; }
            bool qr;
            bool aa;
            bool tc;
            bool rd;
            bool ra;
            Opcode op;
            Responsecode rc;
            BitArray bitArray = new BitArray(theTwoOctets[0].Combine(theTwoOctets[1]));
            bool[] opcode = new bool[] { bitArray[1], bitArray[2], bitArray[3], bitArray[4] };
            bool[] responsecode = new bool[] { bitArray[12], bitArray[13], bitArray[14], bitArray[15] };
            qr = bitArray[0];
            aa = bitArray[5];
            tc = bitArray[6];
            rd = bitArray[7];
            ra = bitArray[8];
            op = (Opcode)(new BitArray(opcode).ToByte());
            rc = (Responsecode)(new BitArray(responsecode).ToByte());
            return new ReplyCode(qr, aa, tc, rd, ra, op, rc);
        }

        public enum Opcode
        {
            QUERY = 0,
            IQUERY = 1,
            STATUS = 2,
        }

        public enum Responsecode
        {
            NOERROR = 0,
            FORMATERROR = 1,
            SERVERFAILURE = 2,
            NAMEERROR = 3,
            NOTIMPLEMENTED = 4,
            REFUSED = 5,
        }
    }
}
