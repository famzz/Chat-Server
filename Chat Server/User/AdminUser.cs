using Chat_Server.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Chat_Server
{
    class AdminUser
    {

        private TcpClient client;
        private DatabaseHandler databaseHandler;
        
        public AdminUser(TcpClient client, DatabaseHandler databaseHandler)
        {
            this.client = client;
            this.databaseHandler = databaseHandler;
        }

        public void GetMessage(string message)
        {
            while (true)
            {
                if (message.Equals("exit"))
                {
                    this.client.Close();
                    break;
                }

                string[] values = message.Split(':');

                string task = values[0];
                string username = null;
                string password = null;

                switch (task)
                {
                    case "NEWUSER":
                        string newUsername = values[1];
                        password = values[2];
                        databaseHandler.AddNewUser(newUsername, password);
                        break;
                    case "VERIFYUSER":
                        username = values[1];
                        password = values[2];
                        bool hasAccount = databaseHandler.HasAccount(username);
                        if (!hasAccount)
                        {
                            MessageHandler.SendMessage(client.GetStream(), "NOACCOUNT");
                        }
                        else
                        {
                            bool verifyPassword = databaseHandler.VerifyPassword(username, password);
                            if (!verifyPassword)
                            {
                                MessageHandler.SendMessage(client.GetStream(), "WRONGPASSWORD");
                            }
                            else
                            {
                                MessageHandler.SendMessage(client.GetStream(), "CORRECTPASSWORD");
                            }
                        }
                        break;
                }

                message = MessageHandler.GetMessage(this.client.GetStream());
            }

        }

    }
}
