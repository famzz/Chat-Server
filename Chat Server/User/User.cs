using System.Collections.Generic;
using System.Net.Sockets;
using System;
using Utilities;

namespace Chat_Server
{
    class User
    {
        private string name;
        private string friendName;

        public void GetMessage(Clients clients, TcpClient userClient, PendingMessageHandler pMH)
        {
            NetworkStream stream = userClient.GetStream();

            while (true)
            {
                string message = MessageHandler.GetMessage(stream);

                if (message.StartsWith("CONNECTIONSTART"))
                {
                    string[] values = message.Split(':');
                    name = values[1];
                    friendName = values[2];


                    clients.AddClient(name, userClient);

                    if (pMH.HasMessages(name))
                    {
                        List<string> pendingMessages = pMH.GetMessages(name);
                        for (int j = 0; j < pendingMessages.Count; j++)
                        {
                            MessageHandler.SendMessage(stream, pendingMessages[j] + "ENDOFLINE");
                        }
                    }

                    Console.WriteLine("Client " + name + " has successfully connected.");
                }
                else if (message.Equals("exit"))
                {
                    userClient.Close();
                    clients.RemoveClient(name);
                    pMH.Remove(name);
                    break;
                }
                else
                {
                    if (clients.HasClient(friendName))
                    {
                        TcpClient friendClient = clients.GetClient(friendName);
                        MessageHandler.SendMessage(friendClient.GetStream(), message);
                    }
                    else
                    {
                        pMH.AddMessage(friendName, message);
                    }

                }
            }
        }

    }
}
