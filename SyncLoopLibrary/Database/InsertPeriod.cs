using System.Data.SQLite;
using System.Diagnostics;

namespace SyncLoopLibrary
{
    public static partial class Database
    {

        /// <summary>
        /// Inserts period into database.
        /// </summary>
        /// <param name="period">The period object to insert.</param>
        /// <returns>ID of inserted period.</returns>
        public static long InsertPeriod(Period period)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // OPEN CONNECTION.
                connection.Open();
                // CREATE QUERY.
                string sql = $"INSERT INTO Periods (StartDate, EndDate) VALUES ('{period.StartDate}', '{period.EndDate}')";

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
