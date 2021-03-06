﻿using System.IO;
using System.Data.SQLite;

namespace Chat_Server.Database
{
    class DatabaseHandler
    {

        private readonly object _lock = new object();
        private SQLiteConnection connection;

        public DatabaseHandler()
        {
            string databaseDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Database\\users.sqlite";
            connection = new SQLiteConnection("Data Source=" + databaseDirectory + ";Version=3;");
            connection.Open();
        }

        public void AddNewUser(string username, string password)
        {
            lock (_lock)
            {
                SQLiteCommand command = new SQLiteCommand("INSERT INTO Passwords (username, password) " +
                    "VALUES (@0, @1);", connection);
                command.Parameters.AddWithValue("0", username);
                command.Parameters.AddWithValue("1", password);
                command.ExecuteNonQuery();
            }

        }

        public bool VerifyPassword(string username, string password)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT password FROM Passwords WHERE username = @0", connection);
            command.Parameters.AddWithValue("0", username);
            string result = (string) command.ExecuteScalar();
            return password.Equals(result);
        }

        public bool HasAccount(string username)
        {
            lock (_lock)
            {
                SQLiteCommand command = new SQLiteCommand("SELECT username FROM Passwords WHERE username = @0", connection);
                command.Parameters.AddWithValue("0", username);
                return command.ExecuteScalar() != null;
            }
        }

    }
}
