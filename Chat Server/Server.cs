using Chat_Server.Database;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Utilities;

namespace Chat_Server
{
    class Server
    {


        public static void StartServer()
        {
            Int32 port = 5667;
            IPAddress address = IPAddress.Any;

            TcpListener server = new TcpListener(address, port);

            DatabaseHandler databaseHandler = new DatabaseHandler();

            Clients clients = new Clients();

            PendingMessageHandler pMH = new PendingMessageHandler();

            server.Start();

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();

                string message = MessageHandler.GetMessage(client.GetStream());

                if (message.StartsWith("CONNECTIONBEGIN"))
                {
                    // This is a user attempting to communicate with another user.
                    Console.WriteLine("Accepted connection from a client");

                    User user = new User();

                    Thread t = new Thread(new ThreadStart(() => user.GetMessage(clients, client, pMH)));
                    t.Start();
                }
                else
                {
                    // This is the admin user attempting to do something server related.
                    AdminUser adminUser = new AdminUser(client, databaseHandler);

                    Thread t = new Thread(new ThreadStart(() => adminUser.GetMessage(message)));
                    t.Start();
                    
                }


            }

        }
    }
}
