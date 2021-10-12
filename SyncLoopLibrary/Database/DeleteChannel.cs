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
        public static void DeleteChannel(Channel channel)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // OPEN CONNECTION.
                connection.Open();
                // CREATE QUERY.
                string sql = $"DELETE FROM Channels WHERE ID={channel.ID}";

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
