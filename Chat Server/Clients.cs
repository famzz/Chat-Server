using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Chat_Server
{
    class Clients
    {
        private readonly Object _lock = new Object();
        private Dictionary<string, TcpClient> _clients = new Dictionary<string, TcpClient>();

        public void AddClient(string clientName, TcpClient client)
        {
            lock (_lock)
            {
                _clients[clientName] = client;
            }
        }

        public TcpClient GetClient(string clientName)
        {
            lock (_lock)
            {
                return _clients[clientName];
            }
        }
        
        public bool HasClient(string clientName)
        {
            lock(_lock)
            {
                return _clients.ContainsKey(clientName);
            }
        }

    }
}
