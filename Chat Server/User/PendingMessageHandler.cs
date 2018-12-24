using System;
using System.Collections.Generic;

namespace Chat_Server
{
    class PendingMessageHandler
    {

        private readonly Object _lock = new object();
        private Dictionary<string, List<string>> pendingMessages = new Dictionary<string, List<string>>();

        public void AddMessage(string name, string message)
        {
            lock (_lock)
            {
                if (HasMessages(name))
                {
                    pendingMessages[name].Add(message);
                }
                else
                {
                    pendingMessages[name] = new List<string>();
                    pendingMessages[name].Add(message);
                }
            }
        }

        public List<string> GetMessages(string name)
        {
            lock (_lock)
            {
                return pendingMessages[name];
            }
        }

        public bool HasMessages(string name)
        {
            lock (_lock)
            {
                return pendingMessages.ContainsKey(name);
            }
        }

    }
}
