using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class SocketMessage
    {
        public Guid SenderId { get; set; }
        public string Message { get; set; }
        public Guid ReceiverId { get; set; }

        public SocketMessage()
        {

        }

        public SocketMessage(string message)
        {
            this.SenderId = Guid.NewGuid();
            this.Message  = message;
        }

        public SocketMessage(Guid senderid, string message, Guid receiverid)
        {
            SenderId   = senderid;
            Message    = message;
            ReceiverId = receiverid;
        }

        public byte[] ToByteArray()
        {
            string stringresult = SenderId.ToString() + " "  + Message + " " + ReceiverId.ToString();
            return Encoding.ASCII.GetBytes(stringresult);
        }
    }
}
