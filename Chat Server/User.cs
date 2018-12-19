using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Chat_Server
{
    class User
    {
        private string name;
        private string friendName;

        public void GetMessage(Clients clients, TcpClient userClient, PendingMessageHandler pMH)
        {
            byte[] data = new byte[256];
            string message = null;
            int i;

            NetworkStream stream = userClient.GetStream();

            while ((i = stream.Read(data, 0, data.Length)) != 0)
            {
                message = Encoding.ASCII.GetString(data, 0, i);
               
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
                            SendMessage(stream, pendingMessages[j] + "ENDOFLINE");
                        }
                    }
                }
                else if (message.Equals("exit"))
                {
                    userClient.Close();
                    break;
                }
                else
                {
                    if (clients.HasClient(friendName))
                    {
                        TcpClient friendClient = clients.GetClient(friendName);
                        SendMessage(friendClient.GetStream(), message);
                    }
                    else
                    {
                        pMH.AddMessage(friendName, message);
                    }

                }
            }

        }

        private static void SendMessage(NetworkStream stream, string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }


    }
}
