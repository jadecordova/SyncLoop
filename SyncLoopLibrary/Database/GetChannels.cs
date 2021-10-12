using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace SyncLoopLibrary
{
    public static partial class Database
    {

        /// <summary>
        /// Gets channels from database.
        /// </summary>
        /// <returns>Channels.</returns>
        public static ObservableCollection<Channel> GetChannels()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                ObservableCollection<Channel> channels = new ObservableCollection<Channel>();
                // Open connection.
                connection.Open();
                // Create sql string.
                string sql = $"SELECT * FROM Channels";
                // Create command.
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                // Execute it.
                SQLiteDataReader reader = command.ExecuteReader();
                // Iterate.
                while (reader.Read())
                {
                    channels.Add(new Channel
                    {
                        ID = Convert.ToInt64(reader["ID"]),
                        Code = Convert.ToString(reader["Code"]),
                        Name = Convert.ToString(reader["Name"])
                    });
                }

                return channels;
            }
        }
    }
}
