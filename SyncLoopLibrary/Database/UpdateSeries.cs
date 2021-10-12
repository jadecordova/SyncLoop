using System.Data.SQLite;
using System.Diagnostics;

namespace SyncLoopLibrary
{
    public static partial class Database
    {

        /// <summary>
        /// Inserts series into database.
        /// </summary>
        /// <param name="series">The series object to update.</param>
        /// <returns>ID of inserted series.</returns>
        public static void UpdateSeries(Series series)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // OPEN CONNECTION.
                connection.Open();
                // CREATE QUERY.
                string sql = $"UPDATE Series SET ";
                sql += $"ChannelID = {series.ChannelID}, ";
                sql += $"NameEnglish = '{series.NameEnglish}', ";
                sql += $"NameSpanish = '{series.NameSpanish}' ";
                sql += $"WHERE ID = {series.ID}";

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
