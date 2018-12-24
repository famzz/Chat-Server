using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Server
{
    public static class MessageHandler
    {

        public static string GetNewMessage(TcpClient client, NetworkStream stream)
        {
            StreamReader reader = new StreamReader(client.GetStream());

            while (client.Connected)
            {
                string message = reader.ReadLine(); 
                return message;
            }

            return null;
        }

    }
}
