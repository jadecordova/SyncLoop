using System.Data.SQLite;
using System.Diagnostics;

namespace SyncLoopLibrary
{
    public static partial class Database
    {

        /// <summary>
        /// Inserts series into database.
        /// </summary>
        /// <param name="channel">The channel to deleted.</param>
        public static void UpdateChannel(Channel channel)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // OPEN CONNECTION.
                connection.Open();
                // CREATE QUERY.
                string sql = $"UPDATE Channels SET Code = '{channel.Code}', Name = '{channel.Name}' WHERE ID={channel.ID}";

                Debug.WriteLine(sql);
                // CREATE COMMAND.
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    // EXECUTE IT.
                    command.ExecuteNonQuery();
                    // CLOSE CONNECTION.
                    connection.Close();
                }
            }
        }
    }
}
