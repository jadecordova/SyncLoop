using System.Data.SQLite;
using System.Diagnostics;

namespace SyncLoopLibrary
{
    public static partial class Database
    {

        /// <summary>
        /// Inserts series into database.
        /// </summary>
        /// <param name="series">The series object to insert.</param>
        /// <returns>ID of inserted series.</returns>
        public static long InsertSeries(Series series)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // OPEN CONNECTION.
                connection.Open();
                // CREATE QUERY.
                string sql = $"INSERT INTO Series (ChannelID, NameEnglish, NameSpanish) VALUES (";
                sql+=$"{series.ChannelID}, '{series.NameEnglish}', '{series.NameSpanish}')";

                Debug.WriteLine(sql);
                // CREATE COMMAND.
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    // EXECUTE IT.
                    command.ExecuteNonQuery();
                    // GET ID.
                    long result = connection.LastInsertRowId;
                    // CLOSE CONNECTION.
                    connection.Close();
                    // RETURN ID.
                    return result;
                }
            }
        }
    }
}
