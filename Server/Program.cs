using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        private static byte[] buffer = new byte[1024];

        private static List<Socket> clientSockets = new List<Socket>();

        private static Socket serverSocket = 
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            StartServer();
            Console.ReadLine();
            CloseAllSockets();
        }

        /// <summary>
        /// Closes all sockets from the clientSockets list
        /// </summary>
        private static void CloseAllSockets()
        {
            foreach (Socket socket in clientSockets)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                socket.Dispose();
            }

            serverSocket.Close();
        }

        private static void StartServer()
        {
            Console.WriteLine("[*] Setting up the server...");
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 13979));
            serverSocket.Listen(100);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            Socket socket = serverSocket.EndAccept(ar);
            clientSockets.Add(socket);
            Console.WriteLine("[!] Client connected... Unique handle: " + socket.Handle.ToString());
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            Socket socket = (Socket) ar.AsyncState;
            try
            {
                int received = socket.EndReceive(ar);
                byte[] buff = new byte[received];
                Array.Copy(buffer, buff, received);

                string text = Encoding.ASCII.GetString(buff);

                Console.WriteLine("Text received: " + text);

                SocketMessage response = new SocketMessage(String.Empty);

                if (text.ToLower() == "get time")
                {
                    response.Message = DateTime.Now.ToLongTimeString();
                }
                else
                {
                    response.Message = "Invalid request";
                }

                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);

                byte[] data = Encoding.ASCII.GetBytes(response.Message);
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            }
            catch
            {
                Console.WriteLine("[/] Client disconnected...");
                socket.Close();
                socket.Dispose();
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndSend(ar);
        }
    }
}
