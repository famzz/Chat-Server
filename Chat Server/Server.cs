using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Chat_Server
{
    class Server
    {


        public static void StartServer()
        {
            Int32 port = 5667;
            IPAddress address = IPAddress.Any;

            TcpListener server = new TcpListener(address, port);

            Clients clients = new Clients();

            PendingMessageHandler pMH = new PendingMessageHandler();

            server.Start();

            while (true)
            {
                TcpClient userClient = server.AcceptTcpClient();

                Console.WriteLine("Accepted connection from a client");

                User user = new User();

                Thread t = new Thread(new ThreadStart(() => user.GetMessage(clients, userClient, pMH)));
                t.Start();
            }

        }

    }
}
