using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Harpokrat.EncryptionAlgorithms;
using Server;

namespace Harpokrat.ClientCode
{
    public class Client
    {
        private static Socket clientSocket;
        public static Guid UserId { get; set; }

        public SocketMessage Message { get; set; }

        public Client()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            UserId = Guid.NewGuid();
            Message = new SocketMessage();
            Connect();
            Message.SenderId = UserId;
            //GetAllClients();
            //MessageBox.Show(test);
        }

        public Client(string Name)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            UserId = Guid.NewGuid();
            Message = new SocketMessage();
            Connect();
            Message.SenderId = UserId;
            //GetAllClients();
            //MessageBox.Show(test);
        }

        internal void CloseSocket()
        {
            clientSocket.Close();
            clientSocket.Dispose();
        }

        public string Receive()
        {
            byte[] receivebuff = new byte[1024];
            clientSocket.Receive(receivebuff);
            
            return Encoding.ASCII.GetString(receivebuff);
        }

        private static void Connect()
        {
            int attempts = 0;

            while(!clientSocket.Connected)
            {
                try
                {
                    attempts++;
                    clientSocket.Connect(new IPEndPoint(IPAddress.Loopback, 13979));
                }
                catch (SocketException e)
                {
                    MessageBox.Show(e.ToString() + " after " + attempts + " attempts.");

                }
            }
            
            MessageBox.Show("Connected to the server after " + attempts + "...");

            MessageBox.Show("Global Unique identifier of this client is: " + UserId);
        }

        private static void GetAllClients()
        {
            byte[] data = new byte[1024];

            data = Encoding.ASCII.GetBytes("get time");
            clientSocket.Send(data);
        }

        public void SendMessage()
        {
            byte[] data = new byte[1024];
            data = Encoding.ASCII.GetBytes(Name + "says: " + Message.Message);
            clientSocket.Send(data);
            MessageBox.Show(Message.SenderId.ToString());
        }
    }
}
